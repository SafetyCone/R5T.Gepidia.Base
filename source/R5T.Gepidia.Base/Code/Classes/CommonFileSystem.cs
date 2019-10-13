using System;
using System.IO;


namespace R5T.Gepidia
{
    public static class CommonFileSystem
    {
        #region Exceptions

        public static string GetCannotOverwriteFileExceptionMessage(string filePath)
        {
            var message = $"Cannot overwrite file:\n{filePath}";
            return message;
        }

        public static IOException GetCannotOverwriteFileIOException(string filePath)
        {
            var message = CommonFileSystem.GetCannotOverwriteFileExceptionMessage(filePath);

            var exception = new IOException(message);
            return exception;
        }

        #endregion
    }
}
