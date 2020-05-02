using System;


namespace R5T.Gepidia
{
    public class FileSystemSite
    {
        public static FileSystemSite New(string directoryPath, IFileSystemOperator fileSystemOperator)
        {
            var fileSystemSite = new FileSystemSite(directoryPath, fileSystemOperator);
            return fileSystemSite;
        }


        public string DirectoryPath { get; }
        public IFileSystemOperator FileSystemOperator { get; }


        public FileSystemSite(string directoryPath, IFileSystemOperator fileSystemOperator)
        {
            this.DirectoryPath = directoryPath;
            this.FileSystemOperator = fileSystemOperator;
        }
    }
}
