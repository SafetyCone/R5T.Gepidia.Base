using System;
using System.IO;


namespace R5T.Gepidia
{
    /// <summary>
    /// Note! Can only use operations defined on the <see cref="IFileSystemOperator"/> since these extensions will not be part of any static implementation behind the implementing class facade.
    /// </summary>
    public static class IFileSystemOperatorExtensions
    {
        public static void FileOrDirectorySwitch(this IFileSystemOperator fileSystemOperator, string path, Action fileAction, Action directoryAction)
        {
            var pathIsFile = fileSystemOperator.ExistsFile(path);
            if (pathIsFile)
            {
                fileAction();
            }
            else
            {
                directoryAction();
            }
        }

        public static T FileOrDirectorySwitch<T>(this IFileSystemOperator fileSystemOperator, string path, Func<T> fileFunction, Func<T> directoryFunction)
        {
            var pathIsFile = fileSystemOperator.ExistsFile(path);
            if (pathIsFile)
            {
                var output = fileFunction();
                return output;
            }
            else
            {
                var output = directoryFunction();
                return output;
            }
        }

        public static bool Exists(this IFileSystemOperator fileSystemOperator, string path)
        {
            var output = fileSystemOperator.FileOrDirectorySwitch(path,
                () => true, // If the path is a file, then it exists.
                () => fileSystemOperator.ExistsDirectory(path)); // Else the path is either a directory, or does not exist.

            return output;
        }

        public static bool IsExistingFile(this IFileSystemOperator fileSystemOperator, string path)
        {
            var output = fileSystemOperator.ExistsFile(path);
            return output;
        }

        public static bool IsExistingDirectory(this IFileSystemOperator fileSystemOperator, string path)
        {
            var output = fileSystemOperator.ExistsDirectory(path);
            return output;
        }

        public static bool IsFile(this IFileSystemOperator fileSystemOperator, string path)
        {
            var output = fileSystemOperator.ExistsFile(path);
            return output;
        }

        public static bool IsDirectory(this IFileSystemOperator fileSystemOperator, string path)
        {
            var output = fileSystemOperator.ExistsDirectory(path);
            return output;
        }

        public static void CreateDirectoryOnlyIfNotExists(this IFileSystemOperator fileSystemOperator, string directoryPath)
        {
            fileSystemOperator.CreateDirectory(directoryPath);
        }

        public static void DeleteDirectoryOnlyIfExists(this IFileSystemOperator fileSystemOperator, string directoryPath, bool recursive = true)
        {
            fileSystemOperator.DeleteDirectory(directoryPath, recursive);
        }

        public static void DeleteFileOnlyIfExists(this IFileSystemOperator fileSystemOperator, string filePath)
        {
            fileSystemOperator.DeleteFile(filePath);
        }

        public static void Delete(this IFileSystemOperator fileSystemOperator, string path)
        {
            fileSystemOperator.FileOrDirectorySwitch(path,
                () => fileSystemOperator.DeleteFile(path),
                () => fileSystemOperator.DeleteDirectory(path));
        }

        public static DateTime GetLastModifiedTime(this IFileSystemOperator fileSystemOperator, string path)
        {
            var lastModifiedTimeUTC = fileSystemOperator.GetLastModifiedTimeUTC(path);

            var lastModifiedTime = lastModifiedTimeUTC.ToLocalTime();
            return lastModifiedTime;
        }

        public static DateTime GetDirectoryLastModifiedTime(this IFileSystemOperator fileSystemOperator, string directoryPath)
        {
            var lastModifiedTimeUTC = fileSystemOperator.GetDirectoryLastModifiedTimeUTC(directoryPath);

            var lastModifiedTime = lastModifiedTimeUTC.ToLocalTime();
            return lastModifiedTime;
        }

        public static DateTime GetFileLastModifiedTime(this IFileSystemOperator fileSystemOperator, string filePath)
        {
            var lastModifiedTimeUTC = fileSystemOperator.GetFileLastModifiedTimeUTC(filePath);

            var lastModifiedTime = lastModifiedTimeUTC.ToLocalTime();
            return lastModifiedTime;
        }

        public static DateTime GetLastModifiedTimeUTC(this IFileSystemOperator fileSystemOperator, string path)
        {
            var output = fileSystemOperator.FileOrDirectorySwitch(path,
                () => fileSystemOperator.GetFileLastModifiedTimeUTC(path),
                () => fileSystemOperator.GetDirectoryLastModifiedTimeUTC(path));

            return output;
        }

        public static void Copy(this IFileSystemOperator fileSystemOperator, string sourcePath, string destinationPath, bool overwrite = true)
        {
            fileSystemOperator.FileOrDirectorySwitch(sourcePath,
                () => fileSystemOperator.CopyFile(sourcePath, destinationPath, overwrite),
                () => fileSystemOperator.CopyDirectory(sourcePath, destinationPath));
        }

        public static void CopyTo(this IFileSystemOperator fileSystemOperator, Stream source, string destinationFilePath, bool overwrite = true)
        {
            fileSystemOperator.Copy(source, destinationFilePath, overwrite);
        }

        public static void CopyFrom(this IFileSystemOperator fileSystemOperator, string sourceFilePath, Stream destination)
        {
            fileSystemOperator.Copy(sourceFilePath, destination);
        }

        public static void Copy(this IFileSystemOperator sourceFileSystem, string sourceFilePath, IFileSystemOperator destinationFileSystem, string destinationFilePath, bool overwrite = true)
        {
            using (var source = sourceFileSystem.ReadFile(sourceFilePath))
            {
                destinationFileSystem.CopyTo(source, destinationFilePath, overwrite);
            }
        }

        public static void Move(this IFileSystemOperator fileSystemOperator, string sourcePath, string destinationPath, bool overwrite = true)
        {
            fileSystemOperator.FileOrDirectorySwitch(sourcePath,
                () => fileSystemOperator.MoveFile(sourcePath, destinationPath, overwrite),
                () => fileSystemOperator.MoveDirectory(sourcePath, destinationPath));
        }

        // Extensions for filtering enumerated files/directories.

        // Extensions for reading/writing text lines.
        public static TextWriter CreateFileText(this IFileSystemOperator fileSystemOperator, string filePath, bool overwrite = true)
        {
            var writerStream = fileSystemOperator.CreateFile(filePath, overwrite);
            var writerTextStream = new StreamWriter(writerStream); // Will close the stream, so ok!
            return writerTextStream;
        }
    }
}
