using System;


namespace R5T.Gepidia
{
    public class FileSystemEntry : IEquatable<FileSystemEntry>
    {
        #region Static

        public static FileSystemEntry New(string path, FileSystemEntryType type, DateTime lastModifiedUTC)
        {
            var entry = new FileSystemEntry(path, type, lastModifiedUTC);
            return entry;
        }

        #endregion


        public string Path { get; }
        public FileSystemEntryType Type { get; }
        public DateTime LastModifiedUTC { get; }


        public FileSystemEntry(string path, FileSystemEntryType type, DateTime lastModifiedUTC)
        {
            this.Path = path;
            this.Type = type;
            this.LastModifiedUTC = lastModifiedUTC;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !obj.GetType().Equals(this.GetType()))
            {
                return false;
            }

            var objAsTypedString = obj as FileSystemEntry;

            var isEqual = this.Equals_Internal(objAsTypedString);
            return isEqual;
        }

        public override int GetHashCode()
        {
            var hashCode = this.Path.GetHashCode(); // Ignore other properties for now (since the HashCode API to combine other values is not yet available for .NET Standard). Also, the path really is the primary definition of a file-system entry (although a file and directory could have the same path on two different systems).
            return hashCode;
        }

        public override string ToString()
        {
            var representation = this.Path;
            return representation;
        }

        public bool Equals(FileSystemEntry other)
        {
            if (other == null || !other.GetType().Equals(this.GetType()))
            {
                return false;
            }

            var isEqual = this.Equals_Internal(other);
            return isEqual;
        }

        private bool Equals_Internal(FileSystemEntry other)
        {
            var output = this.Path.Equals(other.Path)
                && this.Type.Equals(other.Type)
                && this.LastModifiedUTC.Equals(other.LastModifiedUTC)
                ;
            return output;
        }
    }
}
