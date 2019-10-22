using System;


namespace R5T.Gepidia
{
    public static class FileSystemEntryExtensions
    {
        public static bool IsDirectory(this FileSystemEntry fileSystemEntry)
        {
            var output = fileSystemEntry.Type == FileSystemEntryType.Directory;
            return output;
        }
        
        public static bool IsFile(this FileSystemEntry fileSystemEntry)
        {
            var output = !fileSystemEntry.IsDirectory();
            return output;
        }
    }
}
