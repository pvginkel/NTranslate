// Heavily based on http://www.codeproject.com/Articles/73802/NHunspell-Component-for-Visual-Studio.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NTranslate.Win32;

namespace NTranslate.SpellCheck
{
    internal partial class SpellCheckTextBox : RichTextBox
    {
        private bool _whiteSpacePressed;
        private bool _otherSignificantPressed;
        private CharacterRange _lastSelectedWord;
        private string _oldText = String.Empty;
        private Dictionary<int, int> _underlined = new Dictionary<int, int>();
        private readonly List<Point> _pointCache = new List<Point>();
        private ContextMenuStrip _contextMenu;
        private bool _disposed;
        private ToolStripMenuItem _cutButton;
        private ToolStripMenuItem _copyButton;
        private ToolStripMenuItem _addToDictionaryButton;
        private ToolStripMenuItem _languagesButton;
        private bool _acceptsTab;
        private bool _acceptsReturn;

        [DefaultValue(false)]
        [Category("Behavior")]
        public new bool DetectUrls
        {
            get { return base.DetectUrls; }
            set { base.DetectUrls = value; }
        }

        [DefaultValue(true)]
        [Category("Behavior")]
        public bool AcceptsReturn
        {
            get { return _acceptsReturn; }
            set
            {
                _acceptsReturn = value;

                base.AcceptsTab = _acceptsTab || _acceptsReturn;
            }
        }

        [DefaultValue(false)]
        [Category("Behavior")]
        public new bool AcceptsTab
        {
            get { return _acceptsTab; }
            set
            {
                _acceptsTab = value;

                base.AcceptsTab = _acceptsTab || _acceptsReturn;
            }
        }

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Tab:
                case Keys.Shift | Keys.Tab:
                    if (!_acceptsTab)
                        return false;
                    break;

                case Keys.Return:
                    if (!_acceptsReturn)
                        return false;
                    break;
            }

            return base.IsInputKey(keyData);
        }

        public SpellCheckTextBox()
        {
            DetectUrls = false;
            AcceptsReturn = true;
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (Visible)
                UnderlineIncorrectWords();

            base.OnVisibleChanged(e);
        }

        /// <summary>
        /// This is called when the textbox is being redrawn.
        /// When it is, for the textbox to get refreshed, call it's default
        /// paint method and then call our method
        /// </summary>
        /// <param name="m">The windows message</param>
        /// <remarks></remarks>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case NativeMethods.WM_PAINT:
                    Invalidate();
                    base.WndProc(ref m);
                    if (!ReadOnly)
                        CustomPaint();
                    return;

                case NativeMethods.WM_CONTEXTMENU:
                    int x = NativeMethods.Util.SignedLOWORD(m.LParam);
                    int y = NativeMethods.Util.SignedHIWORD(m.LParam);

                    Point point;
                    if ((int)m.LParam == -1)
                        point = new Point(Width / 2, Height / 2);
                    else
                        point = PointToClient(new Point(x, y));

                    if (ClientRectangle.Contains(point))
                        ShowContextMenu(point);
                    return;
            }

            base.WndProc(ref m);
        }

        private void ShowContextMenu(Point point)
        {
            string[] suggestions = null;

            var word = Word.GetWordFromPosition(Text, GetCharIndexFromPosition(point), true);
            if (!word.IsEmpty && _underlined.ContainsKey(word.Range.First))
                suggestions = Program.SpellCheck.GetSuggestions(word.Text);

            BuildContextMenu(word, suggestions);

            _contextMenu.Show(PointToScreen(point));
        }

        private void BuildContextMenu(Word word, string[] suggestions)
        {
            if (_contextMenu == null)
            {
                _contextMenu = new ContextMenuStrip();

                _contextMenu.Items.Add(new ToolStripSeparator());

                _addToDictionaryButton = new ToolStripMenuItem
                {
                    Text = "Add to Dictionary"
                };

                _addToDictionaryButton.Click += _addToDictionaryButton_Click;

                _contextMenu.Items.Add(_addToDictionaryButton);

                _contextMenu.Items.Add(new ToolStripSeparator());

                _cutButton = new ToolStripMenuItem
                {
                    Text = "Cut"
                };

                _cutButton.Click += (s, e) => Cut();

                _contextMenu.Items.Add(_cutButton);

                _copyButton = new ToolStripMenuItem
                {
                    Text = "Copy"
                };

                _copyButton.Click += (s, e) => Copy();

                _contextMenu.Items.Add(_copyButton);

                var pasteButton = new ToolStripMenuItem
                {
                    Text = "Paste"
                };

                pasteButton.Click += (s, e) => CustomPaste();

                _contextMenu.Items.Add(pasteButton);

                var selectAllButton = new ToolStripMenuItem
                {
                    Text = "Select All"
                };

                selectAllButton.Click += (s, e) => SelectAll();

                _contextMenu.Items.Add(selectAllButton);

                _contextMenu.Items.Add(new ToolStripSeparator());

                _languagesButton = new ToolStripMenuItem
                {
                    Text = "Languages"
                };

                _contextMenu.Items.Add(_languagesButton);

                _languagesButton.DropDownItems.Add(new ToolStripSeparator());

                var addDictionariesButton = new ToolStripMenuItem
                {
                    Text = "Add Dictionaries..."
                };

                addDictionariesButton.Click += addDictionariesButton_Click;

                _languagesButton.DropDownItems.Add(addDictionariesButton);
            }

            _addToDictionaryButton.Tag = word;
            _cutButton.Enabled = SelectionLength > 0;
            _copyButton.Enabled = SelectionLength > 0;

            // Remove previous suggestions.

            while (!(_contextMenu.Items[0] is ToolStripSeparator))
            {
                _contextMenu.Items.RemoveAt(0);
            }

            // Are we showing suggestions? If not, hide the first two separators
            // and the add to dictionary menu item.

            _contextMenu.Items[0].Visible = suggestions != null;
            _contextMenu.Items[1].Visible = suggestions != null;
            _contextMenu.Items[2].Visible = suggestions != null;

            if (suggestions != null)
            {
                for (int i = 0; i < suggestions.Length; i++)
                {
                    var suggestionButton = new ToolStripMenuItem
                    {
                        Text = suggestions[i],
                        Font = new Font(Font, FontStyle.Bold),
                        Tag = word
                    };

                    suggestionButton.Click += suggestionButton_Click;

                    _contextMenu.Items.Insert(i, suggestionButton);
                }
            }

            // Rebuild the languages list.

            while (!(_languagesButton.DropDownItems[0] is ToolStripSeparator))
            {
                _languagesButton.DropDownItems.RemoveAt(0);
            }

            var dictionaries = Program.SpellCheck.GetDictionaries();

            for (int i = 0; i < dictionaries.Length; i++)
            {
                var dictionary = dictionaries[i];

                var dictionaryButton = new ToolStripMenuItem
                {
                    Text = dictionary.Label,
                    Checked = dictionary.IsActive,
                    Tag = dictionary
                };

                dictionaryButton.Click += dictionaryButton_Click;

                _languagesButton.DropDownItems.Insert(i, dictionaryButton);
            }
        }

        void dictionaryButton_Click(object sender, EventArgs e)
        {
            Program.SpellCheck.SelectDictionary(((SpellCheckDictionary)((ToolStripMenuItem)sender).Tag).Code);

            UnderlineIncorrectWords();
        }

        void suggestionButton_Click(object sender, EventArgs e)
        {
            var button = (ToolStripMenuItem)sender;

            ReplaceWord((Word)button.Tag, button.Text);
        }

        void addDictionariesButton_Click(object sender, EventArgs e)
        {
            Program.SpellCheck.ShowAddDictionaries(FindForm());

            UnderlineIncorrectWords();
        }

        void _addToDictionaryButton_Click(object sender, EventArgs e)
        {
            Program.SpellCheck.AddToIgnore(((Word)((ToolStripMenuItem)sender).Tag).Text);

            UnderlineIncorrectWords();
        }

        private void CustomPaint()
        {
            using (var graphics = Graphics.FromHwnd(Handle))
            {
                var textLength = TextLength;

                foreach (var word in _underlined)
                {
                    int wordStart = word.Key;
                    int endIndex = wordStart + word.Value - 1;

                    int index = wordStart;
                    int safetyDrewOnce = -1;
                    if (index >= textLength)
                        continue;

                    do
                    {
                        var start = GetPositionFromCharIndex(index);

                        // Determine the first line of waves to draw
                        while (index <= endIndex)
                        {
                            if (index < textLength && GetPositionFromCharIndex(index).Y == start.Y)
                            {
                                index += 1;
                            }
                            else
                            {
                                index--;
                                break;
                            }
                        }

                        var end = GetPositionFromCharIndex(index);

                        // The position above now points to the top left corner of the character.
                        // We need to account for the character height so the underlines go
                        // to the right place.
                        //end.X += 1;

                        int yOffset = NativeMethods.GetBaselineOffsetAtCharIndex(this, wordStart);

                        start.Y += yOffset;
                        end.Y += yOffset;

                        //Add a new wavy line using the starting and ending point
                        DrawWave(graphics, start, end);

                        if (safetyDrewOnce != index)
                            safetyDrewOnce = index;
                        else
                            break;

                        index += 1;
                    }
                    while (index <= endIndex);
                }
            }
        }

        private void DrawWave(Graphics graphics, Point startOfLine, Point endOfLine)
        {
            _pointCache.Clear();

            var y = startOfLine.Y - 1;

            // We need to move the waves up a bit in single line mode because
            // otherwise they won't fit in the text box.

            if (!Multiline)
                y--;

            int offset = startOfLine.X;
            var end = endOfLine.X - 2;

            if (offset % 2 != 0)
            {
                _pointCache.Add(new Point(offset, y + 1));
                offset++;
            }

            while (offset <= end)
            {
                _pointCache.Add(new Point(offset, y + offset % 4));
                offset += 2;
            }

            if (end % 2 != 0)
                _pointCache.Add(new Point(end, y + 1));

            if (_pointCache.Count > 1)
                graphics.DrawLines(Pens.Red, _pointCache.ToArray());
        }

        // The place where we detect keys to which spelling criteria should react later.
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (
                Char.IsWhiteSpace((char)e.KeyValue) ||
                Char.IsPunctuation((char)e.KeyValue) && Char.IsSymbol((char)e.KeyValue)
            )
                _whiteSpacePressed = true;

            if (e.KeyCode == Keys.Delete)
                _otherSignificantPressed = true;
        }

        // The place where we detect weather we should checkspelling or not.
        protected override void OnSelectionChanged(EventArgs e)
        {
            base.OnSelectionChanged(e);

            bool needUnderline = false;
            if (_whiteSpacePressed)
            {
                _whiteSpacePressed = false;
                needUnderline = true;
            }
            else
            {
                var currentWord = GetCharRangeFromPosition(SelectionStart);
                if (_lastSelectedWord.First != currentWord.First) //mouse click in different position
                {
                    if (!_underlined.ContainsKey(_lastSelectedWord.First))
                        UnderlineIncorrectWords();
                }
                else
                {
                    // Remove underlining under cursor if length of curWord has changed.
                    // The same word length is different now.

                    int length;
                    if (
                        _underlined.TryGetValue(currentWord.First, out length) &&
                        length != currentWord.Length ||
                        _lastSelectedWord.Length != currentWord.Length)
                    {
                        // Should determine every offset after text changes and make offsets for underlining list
                        // OR as a variant unconditionally underline all incorrect words.

                        UnderlineNewPositionWords(true);
                        RemoveUnderliningAt(currentWord.First);
                    }
                }

                _lastSelectedWord = currentWord;

                var text = Text;

                int collapseIndex = FindCollapseIndex(_oldText, text);
                if (collapseIndex < text.Length)
                    UnderlineNewPositionWords(true);
            }

            if (needUnderline)
                UnderlineNewPositionWords(true);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (_otherSignificantPressed)
            {
                UnderlineNewPositionWords(true);
                _otherSignificantPressed = false;
            }

            base.OnTextChanged(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            UnderlineIncorrectWords();

            base.OnLeave(e);
        }

        // Update UnderlinedSections variable of the editor.
        private void UnderlineIncorrectWords()
        {
            UnderlineNewPositionWords(false);

            // Should Invalidate because of no Invalidate called on cursor position changed.

            Invalidate(true);

            _oldText = Text;
        }

        // Update UnderlinedSections variable of the editor, but depending on text changing logic.
        // Only words that changed it's position are rechecked.
        // Logic is something that can be always improved.
        private void UnderlineNewPositionWords(bool newPositionWordsOnly)
        {
            int startIndex = 0;

            var text = Text;

            if (newPositionWordsOnly)
            {
                startIndex = FindCollapseIndex(_oldText, text);
                startIndex = GetCharRangeFromPosition(startIndex).First;

                var tmp = new Dictionary<int, int>();

                foreach (var word in _underlined)
                {
                    if (word.Key < startIndex)
                        tmp.Add(word.Key, word.Value);
                }

                _underlined = tmp;
            }
            else
            {
                _underlined.Clear();
            }

            foreach (var word in GetMisspelledWordsRanges(startIndex))
            {
                if (!_underlined.ContainsKey(word.Range.First))
                    _underlined.Add(word.Range.First, word.Range.Length);
            }

            _oldText = text;
        }

        // Remove underlining under word.
        private void RemoveUnderliningAt(int wordCharIndex)
        {
            int closestKeyLeft = int.MaxValue;

            foreach (int key in _underlined.Keys)
            {
                if (wordCharIndex - key >= 0)
                    closestKeyLeft = key;
                else
                    break;
            }

            var curWordRange = GetCharRangeFromPosition(SelectionStart);
            if (
                (
                    curWordRange.First <= wordCharIndex &&
                    wordCharIndex < curWordRange.First + curWordRange.Length
                ) &&
                _underlined.ContainsKey(curWordRange.First)
            )
                _underlined.Remove(closestKeyLeft);
        }

        // Detects the place where text changed.
        private int FindCollapseIndex(string initialString, string newString)
        {
            int length = Math.Min(initialString.Length, newString.Length);

            for (int i = 0; i < length; i++)
            {
                if (initialString[i] != newString[i])
                    return i;
            }

            return length;
        }

        protected void ReplaceWord(string replacement)
        {
            var word = Word.GetWordFromPosition(Text, SelectionStart, true);
            ReplaceWord(word, replacement);
        }

        private void ReplaceWord(Word word, string replacement)
        {
            string text = Text;

            Text =
                text.Substring(0, word.Range.First) +
                replacement +
                text.Substring(word.Range.First + word.Range.Length);

            SelectionStart = word.Range.First + replacement.Length;
        }

        protected string GetWordFromPosition(int startIndex)
        {
            var word = Word.GetWordFromPosition(Text, startIndex, false);
            if (word.IsEmpty)
                return null;

            return word.Text;
        }

        protected Rectangle GetWordFromPositionBoundingBox(int startIndex)
        {
            int offset;

            var word = Word.GetWordFromPosition(Text, startIndex, false);
            if (word.IsEmpty)
            {
                var position = GetPositionFromCharIndex(startIndex);
                offset = NativeMethods.GetBaselineOffsetAtCharIndex(this, startIndex);

                if (offset == 0)
                {
                    offset = TextRenderer.MeasureText(
                        "W",
                        Font,
                        new Size(int.MaxValue, int.MaxValue),
                        TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine
                    ).Height;
                }

                return new Rectangle(
                    position.X,
                    position.Y,
                    0,
                    offset
                );
            }

            var start = GetPositionFromCharIndex(word.Range.First);
            int left = start.X;
            int top = start.Y;
            int right = start.X;
            int bottom = start.Y;

            for (int i = 1; i < word.Range.Length; i++)
            {
                var end = GetPositionFromCharIndex(word.Range.First + i);
                left = Math.Min(left, end.X);
                right = Math.Max(right, end.X);
                bottom = Math.Max(bottom, end.Y);
            }

            offset = NativeMethods.GetBaselineOffsetAtCharIndex(this, word.Range.First);

            return new Rectangle(
                left,
                top,
                right - left,
                (bottom - top) + offset
            );
        }

        private CharacterRange GetCharRangeFromPosition(int startIndex)
        {
            return Word.GetWordFromPosition(Text, startIndex, true).Range;
        }

        private List<Word> GetMisspelledWordsRanges(int startIndex)
        {
            var result = new List<Word>();

            if (Program.SpellCheck != null)
            {
                var text = Text;

                for (int i = startIndex; i < text.Length; i++)
                {
                    var word = Word.GetWordFromPosition(text, i, true);
                    if (!word.IsEmpty && Program.SpellCheck.HasSpellingError(word.Text))
                        result.Add(word);

                    i += word.Range.Length;
                }
            }

            return result;
        }

        protected override bool ProcessCmdKey(ref Message m, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Control | Keys.V:
                case Keys.Shift | Keys.Insert:
                    CustomPaste();
                    return true;
            }

            return base.ProcessCmdKey(ref m, keyData);
        }

        private void CustomPaste()
        {
            string toInsert = Clipboard.GetText();
            if (toInsert.Length == 0)
                return;

            int offset = SelectionStart;
            string text = Text;

            Text =
                text.Substring(0, offset) +
                toInsert +
                text.Substring(offset + SelectionLength);

            Select(offset + toInsert.Length, 0);
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (_contextMenu != null)
                {
                    _contextMenu.Dispose();
                    _contextMenu = null;
                }

                _disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
