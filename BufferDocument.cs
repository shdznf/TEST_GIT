using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace SA.Classes
{
    /// <summary> Типи файлів імпорту </summary>
    public enum ImportFileType
    {
        None = 0,

        /// <summary>NN-MAIN - Налоговая накладная (заголовок) = НН з Сумового обліку</summary>
        InvoiceHeader = 1,

        /// <summary> RK-MAIN  - Корректировка к НН (заголовок) = КН з Сумового обліку</summary>
        CorrectionInvoiceHeader = 2,

        /// <summary> NN-TAB    - Налоговая накладная (табличная часть) </summary>
        InvoiceDetails = 10,

        /// <summary> RK-TAB     - Корректировка к НН  (табличная часть) </summary>
        CorrectionInvoiceDetails = 20,

        /// <summary> KVT-LIST  - Квитанция на документ от ДФС </summary>
        Receipt = 50,

        /// <summary> LIST-DOC – Управляющий файл </summary>
        MainFile = 100,
    }

}
