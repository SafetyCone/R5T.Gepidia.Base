using System;


namespace R5T.Gepidia
{
    public static class FileSystemEntryTypeHelper
    {
        public static FileSystemEntryType IsDirectoryToFileSystemEntryType(bool isDirectory)
        {
            if(isDirectory)
            {
                return FileSystemEntryType.Directory;
            }
            else
            {
                return FileSystemEntryType.File;
            }
        }
    }
}
