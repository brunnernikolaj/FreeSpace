using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace FreeSpaceLib
{
    public static class ExtensionMethods
    {
        public static IEnumerable<T> Sort<T,X>(this IEnumerable<T> source, Func<T, X> sortBy, bool descendingOrder = false)
        {
            return descendingOrder ? source.OrderByDescending(sortBy) : source.OrderBy(sortBy);
        }

        public static IEnumerable<KeyValuePair<T, Y>> FindFromList<T, Y>(this IEnumerable<KeyValuePair<T, Y>> source, List<string> folderStrings)
        {
            foreach (var entry in source)
            {
                foreach (var folderString in folderStrings.Where(folderString => folderString.Equals(entry.Key)))
                {
                    yield return entry;
                }               
            }
        }

        public static string FormatDataSize(this long sizeInBytes)
        {
            if (sizeInBytes < 1024)
                return sizeInBytes + " Bytes";

            if (sizeInBytes < 1048576)
                return (sizeInBytes / 1024) + " KB";

            if (sizeInBytes < 1073741824)
                return (sizeInBytes / 1048576) + " MB";

            return (sizeInBytes / 1073741824) + " GB";
        }

        public static List<string> ParentFoldersToList(this string path)
        {
            var split = path.Split('\\');
            var folderlist = new List<string>();

            for (var count = 0; count < split.Length - 2; count++)
            {
                var temp = split[0];
                for (var i = 0; i < split.Length - 2 - count; i++)
                {                  
                    temp += "\\" + split[i + 1];
                }
                folderlist.Add(temp);
            }
            return folderlist;
        }
    }
}
