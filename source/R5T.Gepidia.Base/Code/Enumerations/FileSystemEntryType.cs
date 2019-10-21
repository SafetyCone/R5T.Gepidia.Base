using System;


namespace R5T.Gepidia
{
    public enum FileSystemEntryType
    {
        // Allow unknown, at the cost of enumeration helper handling.
        Unknown = 0,

        File,
        Directory
    }
}
