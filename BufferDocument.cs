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

    /// <summary> Клас в який вкладаєьтся структура налогової з МЕДОК </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class BufferDocument
    {
        /// <summary> Тип документу накладної </summary>
        public int taxTypeId{ get; private set; }

        /// <summary> Номер НН/КН (МеДОК). Уникальный идентификатор документа из Медок </summary>
        public string bufferDocumentGuid{ get; private set; }

        /// <summary> ОКПО покупателя </summary>
        public string legalUnitOKPO{ get; private set; }

        /// <summary> ОКПО поставщика, выписавшего НН/КН </summary>
        public string creditorOKPO{ get; private set; }

        /// <summary> Наименование поставщика, выписавшего НН/КН </summary>
        public string creditorShortName{ get; private set; }

        /// <summary> ИНН поставщика, выписавшего НН/КН </summary>
        public string creditorKodNDS{ get; private set; }

        /// <summary> Дата выписки НН/КН поставщиком </summary>
        public DateTime operationDate{ get; private set; }

        /// <summary> Порядковый номер НН/КН  </summary>
        public string taxInvoiceNumber{ get; private set; }

        /// <summary> Тип формы документа НН/КН (например, J1201007)  </summary>
        public string taxInvoiceCharCode{ get; private set; }

        /// <summary> Общая сумма документа НН/КН с НДС  </summary>
        public decimal sumOperationWithNDS{ get; private set; }

        /// <summary> Сумма НДС документа НН/КН </summary>
        public decimal sumOperationNDS{ get; private set; }

        /// <summary> Сумма НДС документа НН/КН без НДС</summary>
        public decimal sumOperation{ get; private set; }
        
        /// <summary> Дата регистрации в  ЕРНН документа НН/КН </summary>
        public DateTime? taxInvoiceRegistryDate{ get; private set; }

        /// <summary> Номер квитанции о регистрации документа НН/КН  </summary>
        public string taxInvoiceRegistryTicketNumber{ get; private set; }

        /// <summary> Порядковый номер НН – основания. Только для КН  </summary>
        public string taxInvoiceParentNumber{ get; private set; }

        /// <summary> Дата выписки НН– основания. Только для КН </summary>
        public DateTime? taxInvoiceParentDate{ get; private set; }

        /// <summary> Номер договора для НН/КН  </summary>
        public string taxInvoiceContractNumber{ get; private set; }

        /// <summary> Дата договора для НН/КН </summary>
        public DateTime taxInvoiceContractDate{ get; private set; }

        /// <summary> Порядковый номер НН/КН  </summary>
        public string num1{ get; private set; }

        /// <summary> Код спецрежима налогообложения НН/КН  </summary>
        public string num2{ get; private set; }

        /// <summary> Код филиала НН/КН </summary>
        public string num3{ get; private set; }

        /// <summary> Отметка удачного импорта </summary>
        public bool isImportOk{ get; set; }

        /// <summary> Отметка  </summary>
        public bool isChecked{ get; set; }

        /// <summary> Конструктор </summary>
        public BufferDocument()
        {
            taxTypeId = int.MinValue;
            isImportOk = false;
            isChecked = true;
        }

        /// <summary> Створення запису по різним даним </summary>
        /// <param name="fileType">Тип файлу</param>
        /// <param name="row">Запис вказаного типу</param>
        public bool Update(ImportFileType fileType, DataRow row)
        {
            switch(fileType)
            {
                case ImportFileType.MainFile:
                    bufferDocumentGuid = string.Format("{0}", row["DOC_ID"]);
                    creditorOKPO = string.Format("{0}", row["EDRPOU"]);
                    taxInvoiceCharCode = string.Format("{0}", row["CHARCODE"]);
                    break;
                case ImportFileType.InvoiceHeader:
                    taxTypeId = (int)ImportFileType.InvoiceHeader;

                    legalUnitOKPO = string.Format("{0}", row["EDR_POK"]);
                    operationDate = Convert.ToDateTime(row["N11"]);

                    num1 = string.Format("{0}", row["N2_11"]);
                    num2 = string.Format("{0}", row["N2_12"]);
                    num3 = string.Format("{0}", row["N2_13"]);

                    taxInvoiceNumber = num1;
                    // Друга частина не порожня
                    if(!string.IsNullOrWhiteSpace(num2))
                    {
                        // Третя частина не порожня
                        if (!string.IsNullOrWhiteSpace(num3))
                            taxInvoiceNumber += string.Format("/{0}/{1}", num2, num3);
                        else
                            taxInvoiceNumber += string.Format("/{0}", num2);
                    }
                    else 
                    {
                        // Третя частина не порожня
                        if (!string.IsNullOrWhiteSpace(num3))
                            taxInvoiceNumber += string.Format("//{0}", num3);
                    }

                    sumOperationWithNDS = Math.Round(Convert.ToDecimal(row["A7_11"]), DocumentHelper.SumPoint);
                    sumOperationNDS = Math.Round(Convert.ToDecimal(row["A6_11"]), DocumentHelper.SumPoint);
                    sumOperation = sumOperationWithNDS - sumOperationNDS;

                    creditorShortName = string.Format("{0}", row["FIRM_NAME"]);
                    creditorKodNDS = string.Format("{0}", row["FIRM_INN"]);

                    taxInvoiceContractNumber = string.Format("{0}", row["N81"]);
                    taxInvoiceContractDate = Convert.ToDateTime(row["N82"]);
                    break;
                case ImportFileType.CorrectionInvoiceHeader:
                    taxTypeId = (int)ImportFileType.CorrectionInvoiceHeader;

                    legalUnitOKPO = string.Format("{0}", row["EDR_POK"]);
                    operationDate = Convert.ToDateTime(row["N15"]);

                    num1 = string.Format("{0}", row["N1_11"]);
                    num2 = string.Format("{0}", row["N1_12"]);
                    num3 = string.Format("{0}", row["N1_13"]);

                    taxInvoiceNumber = num1;
                    // Друга частина не порожня
                    if (!string.IsNullOrWhiteSpace(num2))
                    {
                        // Третя частина не порожня
                        if (!string.IsNullOrWhiteSpace(num3))
                            taxInvoiceNumber += string.Format("/{0}/{1}", num2, num3);
                        else
                            taxInvoiceNumber += string.Format("/{0}", num2);
                    }
                    else 
                    {
                        // Третя частина не порожня
                        if (!string.IsNullOrWhiteSpace(num3))
                            taxInvoiceNumber += string.Format("//{0}", num3);
                    }

                    sumOperationWithNDS = Math.Round(Convert.ToDecimal(row["A1_9"]), DocumentHelper.SumPoint);
                    sumOperationNDS = Math.Round(Convert.ToDecimal(row["A2_9"]), DocumentHelper.SumPoint);
                    sumOperation = sumOperationWithNDS - sumOperationNDS;

                    if(row["N2"].IsNull())
                        taxInvoiceParentDate = null;
                    else
                        taxInvoiceParentDate = Convert.ToDateTime(row["N2"]);

                    taxInvoiceParentNumber = string.Format("{0}", row["N2_11"]);
                    // Друга частина не порожня
                    if(!string.IsNullOrWhiteSpace(string.Format("{0}", row["N2_12"])))
                    {
                        // Третя частина не порожня
                        if (!string.IsNullOrWhiteSpace(string.Format("{0}", row["N2_13"])))
                          taxInvoiceParentNumber += string.Format("/{0}/{1}", row["N2_12"],row["N2_13"]);
                        else                      
                          taxInvoiceParentNumber += string.Format("/{0}", row["N2_12"]);
                    }
                    else 
                    {
                        // Третя частина не порожня
                        if (!string.IsNullOrWhiteSpace(string.Format("{0}", row["N2_13"])))
                            taxInvoiceParentNumber += string.Format("//{0}", row["N2_13"]);
                    }

                    creditorShortName = string.Format("{0}", row["FIRM_NAME"]);
                    creditorKodNDS = string.Format("{0}", row["FIRM_INN"]);

                    taxInvoiceContractNumber = string.Format("{0}", row["N81"]);
                    taxInvoiceContractDate = Convert.ToDateTime(row["N82"]);
                    break;
                case ImportFileType.Receipt:
                    var operType = string.Format("{0}", row["OPERTYPE"]).Trim();
                    var status = string.Format("{0}", row["STATUS"]).Trim();
                    if(operType.In("7") && status.In("0", "2"))
                    {
                        taxInvoiceRegistryDate = Convert.ToDateTime(row["RECEPTDATE"]);
                        taxInvoiceRegistryTicketNumber = string.Format("{0}", row["REGNUM"]);
                    }
                    break;
            }
            return true;
        }

        /// <summary> Створення запису по різним даним </summary>
        /// <param name="fileType">Тип файлу</param>
        /// <param name="table">Таблиця вказаного типу</param>
        public bool Update(ImportFileType fileType, DataTable table)
        {
            var r = table.Rows[0];
            return Update(fileType,r);
        }
    }
}
