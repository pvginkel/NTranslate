using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTranslate
{
    internal static class CollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> values)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (values == null)
                throw new ArgumentNullException("values");

            foreach (var value in values)
            {
                collection.Add(value);
            }
        }
    }
}
