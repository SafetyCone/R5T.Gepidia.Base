using System;
using System.Collections.Generic;
using System.IO;


namespace R5T.Gepidia
{
    public interface IFileSystemOperator
    {
        bool ExistsFile(string filePath);
        bool ExistsDirectory(string directoryPath);

        FileSystemEntryType GetFileSystemEntryType(string path);

        void DeleteFile(string filePath); // Checks that entry exists, if it exists, that it's a file, and then deletes it. Idempotent, can be called multiple times with no ill effect.
        void DeleteDirectory(string directoryPath, bool recursive = true); // Checks that the entry exists, if it exists, that it's a diretory, then deletes it (recursive and all!). Idempotent, can be called multiple times with no ill effect.

        Stream CreateFile(string filePath, bool overwrite = true); // If overwrite is false and file path exists, throws an access exception? Found exception? Invalid operation exception? Exception?
        Stream OpenFile(string filePath); // File must exist.

        Stream ReadFile(string filePath);

        void CreateDirectory(string directoryPath);

        IEnumerable<string> EnumerateFileSystemEntryPaths(string directoryPath, bool recursive = false);
        IEnumerable<string> EnumerateDirectories(string directoryPath); // Not placed as an extension for native-support speed. Avoid requiring calls to the file system for each file system entry to determine if the entry is a file or directory.
        IEnumerable<string> EnumerateFiles(string directoryPath); // Not placed as an extension for native-support speed. Avoid requiring calls to the file system for each file system entry to determine if the entry is a file or directory.
        IEnumerable<FileSystemEntry> EnumerateFileSystemEntries(string directoryPath, bool recursive = false);

        DateTime GetDirectoryLastModifiedTimeUTC(string directoryPath);
        DateTime GetFileLastModifiedTimeUTC(string filePath);

        void ChangePermissions(string path, short mode);

        // Copy internally to the file system.
        void CopyFile(string sourceFilePath, string destinationFilePath, bool overwrite = true);
        void CopyDirectory(string sourceDirectoryPath, string destinationDirectoryPath); // Overwrite files.

        // Copy files externally to/from the file system.
        void Copy(Stream source, string destinationFilePath, bool overwrite = true); // Copy from source to destination.
        void Copy(string sourceFilePath, Stream destination);

        // Move files and directories internally within the file system.
        void MoveFile(string sourceFilePath, string destinationFilePath, bool overwrite = true);
        void MoveDirectory(string sourceDirectoryPath, string destinationDirectoryPath);


        #region Exceptions

        string GetCannotOverwriteFileExceptionMessage(string filePath);
        IOException GetCannotOverwriteFileIOException(string filePath);

        #endregion
    }


    /// <summary>
    /// Note! Can only use operations defined on the <see cref="IFileSystemOperator"/> since these extensions will not be part of any static implementation behind the implementing class facade.
    /// </summary>
    public static class IFileSystemOperatorExtensions
    {
        public static void FileOrDirectorySwitch(this IFileSystemOperator fileSystemOperator, string path, Action fileAction, Action directoryAction)
        {
            var pathIsFile = fileSystemOperator.ExistsFile(path);
            if(pathIsFile)
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

        public static void Move(this IFileSystemOperator fileSystemOperator, string sourcePath, string destinationPath, bool overwrite = true)
        {
            fileSystemOperator.FileOrDirectorySwitch(sourcePath,
                () => fileSystemOperator.MoveFile(sourcePath, destinationPath, overwrite),
                () => fileSystemOperator.MoveDirectory(sourcePath, destinationPath));
        }

        // Extensions for filtering enumerated files/directories.

        // Extensions for reading/writing text lines.
    }
}
