using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;
using NHunspell;

namespace NTranslate.SpellCheck
{
    internal class SpellCheck
    {
        private const string UserDictionaryFileName = "user.txt";
        private const string DefaultLanguage = "en-US";

        private string _dictionaryPath;
        private Hunspell _hunspell;
        private readonly HashSet<string> _ignoreList = new HashSet<string>();
        private string _language;

        public bool IgnoreWordsWithDigits { get; set; }

        public SpellCheck()
        {
            SetupDictionaries();
            LoadDictionary();
            LoadUserDictionary();
        }

        private void LoadUserDictionary()
        {
            string fileName = Path.Combine(_dictionaryPath, UserDictionaryFileName);

            if (File.Exists(fileName))
            {
                foreach (string line in File.ReadAllLines(fileName).Where(p => p.Length > 0))
                {
                    _ignoreList.Add(line);
                }
            }
        }

        private void SaveUserDictionary()
        {
            File.WriteAllLines(
                Path.Combine(_dictionaryPath, UserDictionaryFileName),
                _ignoreList.OrderBy(p => p)
            );
        }

        private void LoadDictionary()
        {
            var dictionaries = GetDictionaries();

            string language;

            using (var key = Program.BaseKey)
            {
                language = key.GetValue("Spell Check Language") as string;
            }

            if (
                !String.IsNullOrEmpty(language) &&
                dictionaries.Any(p => p.Code == language)
            )
                _language = language;

            if (_hunspell != null)
                _hunspell.Dispose();

            _hunspell = new Hunspell(
                Path.Combine(_dictionaryPath, _language + ".aff"),
                Path.Combine(_dictionaryPath, _language + ".dic")
            );
        }

        private void SetupDictionaries()
        {
            _dictionaryPath = Path.Combine(
                Program.BasePath,
                "Dictionaries"
            );

            Directory.CreateDirectory(_dictionaryPath);

            _language = DefaultLanguage;

            if (GetDictionaries().All(p => !p.IsActive))
            {
                // Load the default English dictionary if it isn't there yet.

                SaveResource("en-US.aff");
                SaveResource("en-US.dic");
            }
        }

        private void SaveResource(string fileName)
        {
            using (var source = GetType().Assembly.GetManifestResourceStream(GetType().Namespace + "." + fileName))
            using (var target = File.Create(Path.Combine(_dictionaryPath, fileName)))
            {
                source.CopyTo(target);
            }
        }

        public SpellCheckDictionary[] GetDictionaries()
        {
            var result = new List<SpellCheckDictionary>();

            foreach (string path in Directory.GetFiles(_dictionaryPath, "*.dic"))
            {
                string code = Path.GetFileNameWithoutExtension(path);

                if (!File.Exists(Path.Combine(_dictionaryPath, code + ".aff")))
                    continue;

                result.Add(new SpellCheckDictionary(code, code == _language));
            }

            result.Sort((a, b) => String.Compare(a.Label, b.Label, StringComparison.CurrentCultureIgnoreCase));

            return result.ToArray();
        }

        public string[] GetSuggestions(string text)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            return _hunspell.Suggest(text).ToArray();
        }

        public bool HasSpellingError(string text)
        {
            if (String.IsNullOrEmpty(text) || _ignoreList.Contains(text))
                return false;

            bool anyDigit = false;
            bool anyNonDigit = false;

            foreach (char c in text)
            {
                if (Char.IsDigit(c))
                    anyDigit = true;
                else
                    anyNonDigit = true;
            }

            if (!anyNonDigit || (IgnoreWordsWithDigits && anyDigit))
                return false;

            return !_hunspell.Spell(text);
        }

        public void AddToIgnore(string text)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            if (_ignoreList.Add(text))
                SaveUserDictionary();
        }

        public void ShowAddDictionaries(IWin32Window owner)
        {
            using (var form = new AddDictionariesForm(this))
            {
                form.ShowDialog(owner);
            }
        }

        public bool SelectDictionary(string code)
        {
            if (code == null)
                throw new ArgumentNullException("code");

            var dictionary = GetDictionaries().SingleOrDefault(p => p.Code == code);

            if (dictionary == null || dictionary.IsActive)
                return false;

            using (var key = Program.BaseKey)
            {
                key.SetValue("Spell Check Language", code);
            }

            LoadDictionary();

            return true;
        }

        public void AddDictionary(IWin32Window owner, Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");

            using (var target = File.Create(Path.GetTempFileName(), 4096, FileOptions.DeleteOnClose))
            {
                var request = (HttpWebRequest)WebRequest.Create(uri);

                using (var response = request.GetResponse())
                using (var source = response.GetResponseStream())
                {
                    var buffer = new byte[4096];
                    //long length = response.ContentLength;

                    int read;
                    //int totalRead = 0;

                    while ((read = source.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        //totalRead += read;

                        //if (length > 0)
                        //    job.Progress = (double)totalRead / length;

                        target.Write(buffer, 0, read);
                    }
                }

                target.Position = 0;

                var zipFile = new ZipFile(target);

                InstallDictionary(zipFile);
            }

            MessageBox.Show(owner, "The dictionary was installed", "NTranslate", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void InstallDictionary(ZipFile zipFile)
        {
            string installed = null;

            foreach (ZipEntry entry in zipFile)
            {
                if (entry.Name.EndsWith(".dic"))
                {
                    string path;
                    string code;

                    int pos = entry.Name.LastIndexOf('/');

                    if (pos == -1)
                    {
                        path = null;
                        code = entry.Name;
                    }
                    else
                    {
                        path = entry.Name.Substring(0, pos + 1);
                        code = entry.Name.Substring(pos + 1);
                    }

                    code = code.Substring(0, code.Length - 4);

                    try
                    {
                        CultureInfo.GetCultureInfo(code);
                    }
                    catch (CultureNotFoundException)
                    {
                        continue;
                    }

                    int affEntryIndex = zipFile.FindEntry(
                        path + code + ".aff",
                        true
                    );

                    if (affEntryIndex == -1)
                    {
                        continue;
                    }

                    using (var source = zipFile.GetInputStream(entry))
                    {
                        SaveFile(source, code + ".dic");
                    }

                    using (var source = zipFile.GetInputStream(affEntryIndex))
                    {
                        SaveFile(source, code + ".aff");
                    }

                    // Install the first found, if there are multiple present.

                    if (installed == null)
                        installed = code;
                }
            }

            if (installed != null)
            {
                using (var key = Program.BaseKey)
                {
                    key.SetValue("Spell Check Language", installed);
                }

                LoadDictionary();
            }

            return;
        }

        private void SaveFile(Stream source, string fileName)
        {
            using (var target = File.Create(Path.Combine(_dictionaryPath, fileName)))
            {
                source.CopyTo(target);
            }
        }
    }
}
