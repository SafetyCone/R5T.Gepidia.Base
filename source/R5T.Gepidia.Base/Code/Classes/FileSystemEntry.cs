using System;


namespace R5T.Gepidia
{
    public class FileSystemEntry
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
    }
}
