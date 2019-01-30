using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace SA.Classes
{
    public enum ImportFileType
    {
        None = 0,

        InvoiceHeader = 1,

        CorrectionInvoiceHeader = 2,

        InvoiceDetails = 10,

        CorrectionInvoiceDetails = 20,

        Receipt = 50,

        MainFile = 100,
    }

}
