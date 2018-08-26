using System;
using System.Collections.Generic;
using System.Text;

namespace FileHandlerService
{
    public enum TransferType
    {
        Ascii,
        Ebcdic,
        Image,
        Local,
    }

    public enum FormatControlType
    {
        NonPrint,
        Telnet,
        CarriageControl,
    }

    public enum DataConnectionType
    {
        Passive,
        Active,
    }

    public enum FileStructureType
    {
        File,
        Record,
        Page,
    }
}
