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
}
