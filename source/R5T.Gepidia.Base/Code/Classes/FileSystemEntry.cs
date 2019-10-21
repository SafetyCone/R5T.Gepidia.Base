using System;


namespace R5T.Gepidia
{
    public class FileSystemEntry
    {
        #region Static

        public static FileSystemEntry New(string path, FileSystemEntryType type)
        {
            var entry = new FileSystemEntry(path, type);
            return entry;
        }

        #endregion


        public string Path { get; }
        public FileSystemEntryType Type { get; }


        public FileSystemEntry(string path, FileSystemEntryType type)
        {
            this.Path = path;
            this.Type = type;
        }
    }
}
