using System;
using System.Collections.Generic;


namespace R5T.Gepidia
{
    public class PathOnlyFileSystemEntryEqualityComparer : IEqualityComparer<FileSystemEntry>
    {
        public bool Equals(FileSystemEntry x, FileSystemEntry y)
        {
            var output = x.Path == y.Path;
            return output;
        }

        public int GetHashCode(FileSystemEntry obj)
        {
            var output = obj.Path.GetHashCode();
            return output;
        }
    }
}
