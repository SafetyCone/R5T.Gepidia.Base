using System;

using R5T.Gepidia;


namespace R5T.Gepidia
{
    public class FileSystemSite
    {
        public string DirectoryPath { get; }
        public IFileSystemOperator FileSystemOperator { get; }


        public FileSystemSite(string directoryPath, IFileSystemOperator fileSystemOperator)
        {
            this.DirectoryPath = directoryPath;
            this.FileSystemOperator = fileSystemOperator;
        }
    }
}
