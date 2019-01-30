using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Mask;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using FozzySystems;
using FozzySystems.Controls;
using FozzySystems.Proxy;
using FozzySystems.Reporting;
using FozzySystems.Reporting.Controls;
using FozzySystems.Types;
using SA.Forms;
using UEditor.Classes;
using UEditor.Controls;
using Utils.Assist;
using Utils.Helpful;
using Application = System.Windows.Forms.Application;

namespace SA.Classes
{
    /// <summary> Побудова таблиць різною структури для відображення у грідах </summary>
    public static class DocumentHelper
    {
        #region Основні константи

        /// <summary> Весь документ </summary>
        public const string Document = "Document";

        /// <summary> Таблица заголовков документов </summary>
        public const string DocumentHeader = "DocumentHeaders";

        /// <summary> Таблица строк документов </summary>
        public const string DocumentDetails = "DocumentDetails";

        /// <summary> Таблица додаткових виртат по документу з розбивкою по артикулам</summary>
        public const string DocumentExpenses = "DocumentExpenses";

        /// <summary> Таблица додаткових виртат по документу загальна</summary>
        public const string DocumentExpensesGroup = "DocumentExpensesGroup";

        /// <summary> Імя таблиці повідомлень </summary>
        public const string MessageTableName = "Messages";

        /// <summary> Імя таблиці заголовка </summary>
        public const string HeaderTableName = "Header";

        /// <summary> Імя документів із МЕДОК </summary>
        public const string BufferDocuments = "BufferDocuments";

        /// <summary> Ідентифікатор правила </summary>
        public const string RuleId = "ruleId";

        /// <summary> Ідентифікатор документу </summary>
        public const string DocumentId = "documentId";

        /// <summary> Статус документу </summary>
        public const string DocumentStatus = "documentStatusId";

        /// <summary> Ідентифікатор операції </summary>
        public const string OperationId = "operationId";

        /// <summary> Ідентифікатор операції </summary>
        public const string ExternalOperationId = "externalOperationId";

        /// <summary> Ідентифікатор батьківського документу </summary>
        public const string ParentId = "parentId";

        /// <summary> Ідентифікатор дитячого документу </summary>
        public const string ChildId = "childId";

        /// <summary> Ідентифікатор проблеми </summary>
        public const string ProblemId = "problemId";

        /// <summary> Дата </summary>
        public const string OpertionDate = "operationDate";

        /// <summary> Дата проводки </summary>
        public const string EntriesDate = "entriesDate";

        /// <summary> Назва операції </summary>
        public const string OperationName = "operationName";

        /// <summary> Сап Ідентифікатор документу </summary>
        public const string SapCounter = "sapCounter";

        /// <summary> Сап Ідентифікатор батьківського документу </summary>
        public const string SapCounterParent = "sapCounterParent";

        /// <summary> Сап Ідентифікатор контрагенту </summary>
        public const string ContragentSapId = "creditorSapId";

        /// <summary>  Ідентифікатор контрагенту </summary>
        public const string ContragentId = "creditorId";

        /// <summary> Guid </summary>
        public const string Guid = "guid";

        /// <summary> Guid ошибки </summary>
        public const string GuidError = "guidError";

        /// <summary> Пустой </summary>
        public const string GuidEmpty = "00000000-0000-0000-0000-000000000000";

        /// <summary> GuidImage </summary>
        public const string GuidImage = "guidImage";

        /// <summary> Тип документу накладної </summary>
        public const string TaxTypeId = "taxTypeId";

        /// <summary>  </summary>
        public const string TaxInvoiceNumber = "taxInvoiceNumber";

        /// <summary> Відмітка </summary>
        public const string IsChecked = "isChecked";

        /// <summary> Документ проверен </summary>
        public const string DocumentIsChecked = "documentIsChecked";

        /// <summary> Время обновления документа </summary>
        public const string DocumentLastUpdate = "documentLastUpdate";

        /// <summary> Заметки </summary>
        public const string Note = "note";

        /// <summary> БЕ </summary> 
        public const string LegalUnitId = "legalUnitId";
        /// <summary> БЕ </summary>
        public const string LegalUnitSapId = "legalUnitSapId";
        /// <summary> БЕ имя </summary>
        public const string LegalUnitSapName = "legalUnitSapName";

        /// <summary> Филиал </summary>
        public const string FilialId = "filialId";
        /// <summary> Филиал название </summary>
        public const string FilialName = "filialName";
        /// <summary> Сап Ідентифікатор філіалу </summary>
        public const string FilialSapId = "filialSapId";

        /// <summary> Счет </summary>
        public const string AccountId = "accountId";
        /// <summary> Счет краткое название </summary>
        public const string AccountShortName = "accountShortName";
        /// <summary> Счет название </summary>
        public const string AccountName = "accountName";

        /// <summary>Додаткова витрата </summary>
        public const string ExpenseTypeId = "expenseTypeId";

        /// <summary> Кількість </summary>
        public const string Quantity = "quantity";

        /// <summary> Додаткова умова </summary>
        public const string InternalWhere = "internalWhere";

        /// <summary> miCheckState </summary>
        public const string MiCheckState = "miCheckState";

        /// <summary> miCheckState </summary>
        public const string MiCheckStateOk1 = "206";

        /// <summary> miCheckState </summary>
        public const string MiCheckStateOk2 = "301";

        /// <summary> miCheckState </summary>
        public const string MiCheckStateOk3 = "303";

        /// <summary> Размер окна сообщений </summary>
        public static Size LagreMessageSize = new Size(800, 500);

        /// <summary> Кількість знаків після коми у сумах </summary>
        public const int SumPoint = 2;

        /// <summary> Кількість знаків після коми у кількості </summary>
        public const int QuantityPoint = 7;

        /// <summary> Кількість знаків після коми у курсі </summary>
        public const int RatePoint = 6;

        /// <summary> Кількість знаків після коми у ціні </summary>
        public const int PricePoint = 9;

        /// <summary> Максимальна кількість в історії </summary>
        public const int MaxQueryHistory = 20;

        /// <summary> Запит чи змінився запис </summary>
        public static ICollection<string> RequestColumns = new Collection<string> { DocumentId, DocumentLastUpdate };

        #endregion

        /// <summary> Помилки </summary>
        public static ErrorsHelper ErrorsHelper = new ErrorsHelper();

        #region Допоміжні пасочки

        /// <summary> Чекаю завантаження </summary>
        public static void AwaitingLoad()
        {
            while (SaHelper.EntryAttributes == null)
            {
                Application.DoEvents();
            }
        }

        /// <summary> Додати таблицю до таблиці. Повинні бути однакової структури </summary>
        /// <param name="dest">Куди додати</param>
        /// <param name="source">Що додати</param>
        /// <returns>Результат</returns>
        public static DataTable AddToTable(DataTable dest, DataTable source)
        {
            foreach (var r in source.Rows.Cast<DataRow>())
                dest.Rows.Add(r.ItemArray);
            dest.AcceptChanges();
            return dest;
        }

        /// <summary> Додати запис до таблиці. Повинні бути однакової структури </summary>
        /// <param name="dest">Куди додати</param>
        /// <param name="sourceRow">Що додати</param>
        /// <param name="acceptChanges">Після додавання запису робити AcceptChanges у dest</param>
        /// <returns>Результат</returns>
        public static DataTable AddToTable(DataTable dest, DataRow sourceRow, bool acceptChanges = false)
        {
            if (sourceRow == null)
                return dest;
            dest.Rows.Add(sourceRow.ItemArray);
            if (acceptChanges)
                dest.AcceptChanges();
            return dest;
        }

        /// <summary> Оновити дані з одного запису у інший</summary>
        /// <param name="dest">Запис який поновити</param>
        /// <param name="source">А цим поновити</param>
        /// <param name="excludedName">Виключати імена з переносу</param>
        public static bool RefreshData(DataRow dest, DataRow source, IDictionary<string, bool> excludedName = null)
        {
            if (dest == null || source == null)
                return false;

            // Виключити з оновлення колонки
            if (excludedName == null)
                excludedName = new Dictionary<string, bool> { { OperationId, true }, { DocumentId, true }, { "bufferDocumentGuid", true } };

            // Якщо колонку виключати з оновлення то по ній буду зрівнювати дані
            if (excludedName.Where(e => e.Value)
                           .Any(eName => dest.Table.Columns.Contains(eName.Key) && source.Table.Columns.Contains(eName.Key) && ComparerHelper.CompareObjects(dest[eName.Key], source[eName.Key]) != 0))
                return false;

            // Виключити з оновлення колонку відмітки
            excludedName.Add(IsChecked, false);

            var isChanged = false;
            foreach (var columnSource in source.Table.Columns.Cast<DataColumn>().Where(c => !c.ColumnName.In(excludedName.Keys.ToArray())))
            {
                var columnSource1 = columnSource;
                foreach (var columnDest in dest.Table.Columns.Cast<DataColumn>().Where(c => string.Equals(columnSource1.ColumnName, c.ColumnName, StringComparison.OrdinalIgnoreCase)))
                {
                    if (ComparerHelper.CompareObjects(dest[columnDest.ColumnName], source[columnSource1.ColumnName]) != 0)
                        isChanged = true;
                    try
                    {
                        dest[columnDest.ColumnName] = source[columnSource1.ColumnName];
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            return isChanged;
        }

        /// <summary> Встановити даны у строку соурса із даних редагування </summary>
        /// <param name="dest">Запис який поновити</param>
        /// <param name="headers">Список контролів</param>
        /// <param name="excludedName">Виключати імена з переносу</param>
        public static bool RefreshData(DataRow dest, ICollection<IHeaderControl> headers, IDictionary<string, bool> excludedName = null)
        {
            var isChanged = false;
            foreach (var h in headers.Where(h => h is UCombo).OfType<UCombo>())
            {
                if (RefreshData(dest, h.ValueRow, excludedName))
                    isChanged = true;
            }
            return isChanged;
        }

        /// <summary> Делегат оновлення записів </summary>
        /// <param name="gridView">Грід</param>
        /// <param name="dest">Запис який є</param>
        /// <param name="update">Запис оновлення</param>
        /// <param name="keyField">Ключове поле</param>
        /// <param name="makeFocusedItemVisible">Робити сфокусований запис видимим</param>
        /// <returns>Чи оновили(якщо співпали записи по ключах наприклад)</returns>
        public delegate bool BackGridViewItemUpdaterDelegate(GridView gridView, object dest, DataRow update, string keyField, bool makeFocusedItemVisible = false);

        /// <summary> Оновлення записів </summary>
        public static BackGridViewItemUpdaterDelegate BackItemUpdater = UpdateDestRow;

        /// <summary> Функція оновлення записів(Кожен з кожнним) </summary>
        /// <param name="gridView">Грід</param>
        /// <param name="dest">Запис який дали для слідкування</param>
        /// <param name="update">Запис який прийшов по оновленню</param>
        /// <param name="keyField">Ключове поле</param>
        /// <param name="makeFocusedItemVisible">Робити сфокусований запис видимим</param>
        public static bool UpdateDestRow(GridView gridView, object dest, DataRow update, string keyField, bool makeFocusedItemVisible = false)
        {
            var r = dest as DataRow;
            if (r == null || update == null)
                return false;
            // Не співпало по ключу то все
            if (string.Compare(string.Format("{0}", r[keyField]), string.Format("{0}", update[keyField]), StringComparison.OrdinalIgnoreCase) != 0)
                return false;

            gridView.BeginDataUpdate();
            var isChanged = RefreshData(r, update);
            gridView.EndDataUpdate();
            if (!makeFocusedItemVisible || !isChanged)
                return true;

            var fr = gridView.GetDataRow(gridView.FocusedRowHandle);
            if (fr != null && string.Compare(string.Format("{0}", r[keyField]), string.Format("{0}", fr[keyField]), StringComparison.OrdinalIgnoreCase) == 0)
                gridView.MakeRowVisible(gridView.FocusedRowHandle);

            return true;
        }

        /// <summary> Отримати стандартний фільтерсет </summary>
        /// <param name="r">Строка</param>
        /// <param name="documentIdAsParendId">Додавати ид.документа як парент</param>
        /// <returns>Фільтерсет</returns>
        public static FilterSet GetStandartFilterSet(DataRow r, bool documentIdAsParendId = false)
        {
            var fS = new FilterSet();
            if (r == null)
                return fS;
            if (documentIdAsParendId)
            {
                fS[ParentId] = new FilterSetItem(ParentId, FilterType.Static, string.Format("{0}", r[DocumentId]));
                fS[SapCounterParent] = new FilterSetItem(SapCounterParent, FilterType.Static, string.Format("{0}", r[SapCounter]));
                fS[TaxInvoiceNumber] = new FilterSetItem(TaxInvoiceNumber, FilterType.Static, string.Format("{0}", r[TaxInvoiceNumber]));
            }
            else
            {
                fS[DocumentId] = new FilterSetItem(DocumentId, FilterType.Static, string.Format("{0}", r[DocumentId]));
                fS[OperationId] = new FilterSetItem(OperationId, FilterType.Static, string.Format("{0}", r[OperationId]));

                fS[FilialId] = new FilterSetItem(FilialId, FilterType.Static, string.Format("{0}", r[FilialId]));
                fS[FilialSapId] = new FilterSetItem(FilialSapId, FilterType.Static, string.Format("{0}", r[FilialSapId]));
                fS[SapCounter] = new FilterSetItem(SapCounter, FilterType.Static, string.Format("{0}", r[SapCounter]));

                if (r.Table.Columns.Contains(DocumentStatus))
                    fS[DocumentStatus] = new FilterSetItem(DocumentStatus, FilterType.Static, string.Format("{0}", r[DocumentStatus]));
                else
                    fS[DocumentStatus] = new FilterSetItem(DocumentStatus, FilterType.Static, 0);

                if (r.Table.Columns.Contains(ExternalOperationId))
                    fS[ExternalOperationId] = new FilterSetItem(ExternalOperationId, FilterType.Static, string.Format("{0}", r[ExternalOperationId]));
                else
                    fS[ExternalOperationId] = new FilterSetItem(ExternalOperationId, FilterType.Static, string.Format("{0}", r[OperationId]));

                fS[ContragentSapId] = new FilterSetItem(ContragentSapId, FilterType.Static, string.Format("{0}", r[ContragentSapId]));
                fS[TaxTypeId] = new FilterSetItem(TaxTypeId, FilterType.Static, string.Format("{0}", r[TaxTypeId]));
                fS[TaxInvoiceNumber] = new FilterSetItem(TaxInvoiceNumber, FilterType.Static, string.Format("{0}", r[TaxInvoiceNumber]));

                if (r.Table.Columns.Contains(OpertionDate))
                    fS[OpertionDate] = new FilterSetItem(OpertionDate, FilterType.Static, MicroSerializer.CreateValue(r[EntriesDate], "D"));

                //                fS[EntriesDate] = new FilterSetItem(EntriesDate, FilterType.Static, MicroSerializer.CreateValue(r[EntriesDate],"D"));
            }
            return fS;
        }

        /// <summary> Отримати стандартний фільтерсет </summary>
        /// <param name="r">Строка</param>
        /// <returns>Фільтерсет</returns>
        public static FilterSet GetNewFilterSet(DataRow r)
        {
            var fS = new FilterSet();
            // Новый
            fS[DocumentStatus] = new FilterSetItem(DocumentStatus, FilterType.Static, 1);
            // Родителем будет текущий документ
            if (r != null)
                fS[ParentId] = new FilterSetItem(ParentId, FilterType.Static, string.Format("{0}", r[DocumentId]));

            return fS;
        }

        /// <summary> Отримати стандартний фільтерсет з доданими значеннями</summary>
        /// <param name="f">Фільтерсет</param>
        /// <param name="r">Строка</param>
        /// <returns>Фільтерсет</returns>
        public static FilterSet AddStandartValue(FilterSet f, DataRow r)
        {
            var fS = new FilterSet();
            foreach (var item in f.items)
            {
                var sb = new StringBuilder(item.Value);
                if (r.Table.Columns.Contains(item.name))
                    sb.AppendFormat(",{0}", r[item.name]);
                fS[item.name] = new FilterSetItem(item.name, item.type, sb.ToString());
            }
            return fS;
        }

        /// <summary> Заголовок документу </summary>
        /// <param name="row">Запис документу</param>
        /// <param name="owner">Овнер</param>
        public static DataRow GetDocumentHeader(DataRow row, Control owner = null)
        {
            if (row == null || row.IsNull(DocumentId))
                return null;
            var fS = GetStandartFilterSet(row);
            var sourceTable = GetDocumentHeader(fS, owner);
            return sourceTable == null || sourceTable.Rows.Count == 0 ? null : sourceTable.Rows[0];
        }

        /// <summary> Заголовок документу </summary>
        /// <param name="filter">Запис документу</param>
        /// <param name="owner">Овнер</param>
        public static DataTable GetDocumentHeader(FilterSet filter, Control owner = null)
        {
            if (filter == null || filter[DocumentId] == null)
                return null;
            try
            {
                var data = UEditDataReader.GetData(filter, "SA.Forms.GetDocumentHeaders", owner);
                if (data.ContainsKey("Data"))
                    return data["Data"];
            }
            catch (Exception)
            {
                return null;
            }
            return null;
        }

        #endregion

        #region Пошукові маневри

        /// <summary> Підставити по статусу його назву </summary>
        /// <param name="statusId">Статус</param>
        /// <returns>Назва</returns>
        public static object GetNameByStatus(object statusId)
        {
            return SaHelper.HashStatuses.Where(r => ComparerHelper.CompareObjects(r.Key, statusId) == 0)
                .Select(r => r.Value)
                .FirstOrDefault();
        }

        /// <summary> Вызывается на группе</summary>
        /// <param name="statusId">Статус</param>
        /// <param name="operationId">Операція</param>
        /// <param name="usedOperation">Одна із операцій які контролюються у статусі</param>
        /// <param name="isUsed">Включать или исключать usedOperation</param>
        /// <returns>Набор действий</returns>
        public static IEnumerable<OperationByActions> GetGroupAction(object statusId, object operationId, IList<object> usedOperation = null, bool isUsed = true)
        {
            var ret = new Collection<OperationByActions>();
            // Вызывается на группе
            SaHelper.HashOperationByActions.Where(r => ComparerHelper.CompareObjects(r.StatusId, statusId) == 0 &&
                                                       r.ModeGroupOperation.In(OperationByActions.GroupAction) &&
                                                       (operationId == null || ComparerHelper.CompareObjects(r.OperationId, operationId) == 0) &&
                                                       (usedOperation == null || usedOperation.Count == 0 || usedOperation.Any(o => ComparerHelper.CompareObjects(r.OperationId, o) == 0) == isUsed) &&
                                                       ret.All(c => (c.ActionId != r.ActionId) || (c.ActionId == r.ActionId && string.Compare(c.ActionType, r.ActionType, StringComparison.OrdinalIgnoreCase) != 0)))
                .ForEach(g => ret.Add(new OperationByActions(g)));

            return ret.OrderByDescending(r => r.ActionType).ThenBy(r => r.OrderId);
        }


        /// <summary> Вызывается на документе </summary>
        /// <param name="row">Документ</param>
        /// <returns>Набор действий</returns>
        public static OperationByActions GetSinglePayAction(DataRow row)
        {
            var ret = SaHelper.HashOperationByActions.FirstOrDefault(r => r.ModeGroupOperation.In(OperationByActions.SingleAction)
                                                                          && r.ActionType == OperationByActions.DefaultActionType
                                                                          && r.ActionId.In(1, 2, 3)
                                                                          && row != null
                                                                          && CompareRowEx(row, r) == r);

            return ret == null ? null : new OperationByActions(ret);
        }

        /// <summary> Вызывается на документе </summary>
        /// <param name="row">Документ</param>
        /// <returns>Набор действий</returns>
        public static IEnumerable<OperationByActions> GetSingleAction(DataRow row)
        {
            var ret = new Collection<OperationByActions>();
            SaHelper.HashOperationByActions.Where(r => r.ModeGroupOperation.In(OperationByActions.SingleAction) &&
                                                       ret.All(c => (c.ActionId != r.ActionId) || (c.ActionId == r.ActionId && string.Compare(c.ActionType, r.ActionType, StringComparison.OrdinalIgnoreCase) != 0))
                                                       && row != null
                                                       && CompareRowEx(row, r) == r)
                .ForEach(g => ret.Add(new OperationByActions(g)));

            return ret.OrderByDescending(r => r.ActionType).ThenBy(r => r.OrderId);
        }

        /// <summary> Чи можлива для даної оперції у даному статусі для даного документу дія деякого типу </summary>
        /// <param name="row">Документ</param>
        /// <param name="action">Дія</param>
        /// <returns>Операція або null</returns>
        public static bool IsActionEnabled(DataRow row, OperationByActions action)
        {
            return SaHelper.HashOperationByActions.Any(r => r.ActionId == action.ActionId
                                                            && r.ModeGroupOperation == action.ModeGroupOperation
                                                            && string.Compare(r.ActionType, action.ActionType, StringComparison.OrdinalIgnoreCase) == 0
                                                            && row != null
                                                            && CompareRowEx(row, r) == r);
        }

        /// <summary> Магічне порівняння </summary>
        /// <param name="row">Документ</param>
        /// <param name="action">Дія</param>
        /// <returns>Операція або null</returns>
        private static OperationByActions CompareRowEx(DataRow row, OperationByActions action)
        {
            // Коли редагування нового документу
            var documentId = row[DocumentId];
            if (documentId.IsNull())
                return null;

            var statusId = row[DocumentStatus];
            var operationId = row[OperationId];
            var miCheckState = row[MiCheckState];

            if (ComparerHelper.CompareObjects(action.StatusId, statusId) != 0 || ComparerHelper.CompareObjects(action.OperationId, operationId) != 0)
                return null;

            // Якщо проведення та поточна операція 169 Приход от поставщика (MI) та статус перевірки не ОК та операція сплатити у любому статусі недоступна
            if(action.ActionId == 3 &&
               ComparerHelper.CompareObjects(169, operationId) == 0 &&
               ComparerHelper.CompareObjects(MiCheckStateOk1, miCheckState) != 0 &&
               ComparerHelper.CompareObjects(MiCheckStateOk2, miCheckState) != 0 &&
               ComparerHelper.CompareObjects(MiCheckStateOk3, miCheckState) != 0 &&
               !FZCoreProxy.Session.IsOperationAvailable("SA.Forms.PayAllMiDocuments"))
                return null;

            var dt = row.Table.Clone();
            dt.Rows.Add(row.ItemArray);
            dt.AcceptChanges();

            // Основна умова, операція + статус + не в черзі + не вилученый + немає глобальної помилки
            var selectExpression = new StringBuilder(string.Empty);
            if (!string.IsNullOrWhiteSpace(action.Where) && action.Where.Contains("errorId"))
                selectExpression.AppendFormat("{0}={1} and {2}={3} and guid is null and isnull(isDeleted,0)=0", OperationId, operationId, DocumentStatus, statusId);
            else
                selectExpression.AppendFormat("{0}={1} and {2}={3} and errorId is null and guid is null and isnull(isDeleted,0)=0", OperationId, operationId, DocumentStatus, statusId);

            if (!string.IsNullOrWhiteSpace(action.Where))
                selectExpression.AppendFormat(" and ({0})", action.Where);

            return dt.Select(selectExpression.ToString()).Length == 1 ? action : null;
        }

        #endregion

        #region Проводки

        /// <summary> Проводки документу </summary>
        /// <param name="row">Запис документу</param>
        /// <param name="parent">Хто викликає</param>
        /// <param name="confirmText">Текст кнопки</param>
        public static int? DocumentEntries(DataRow row, Control parent, string confirmText = null)
        {
            if (row == null)
                return null;
            var fS = GetStandartFilterSet(row);
            int? run = null;
            var rulesForm = new DocumentRules(fS);
            if (confirmText != null)
            {
                rulesForm.AllowConfirm = true;
                rulesForm.ConfirmText = confirmText;
                if (rulesForm.ShowDialog() == DialogResult.OK)
                    run = int.MaxValue;
            }
            else
            {
                FormsActivator.Activate(rulesForm, parent);
            }
            return run;
        }

        /// <summary> Проводки документу </summary>
        /// <param name="gridView">Грид</param>
        /// <param name="rowHandle">Ід. документа</param>
        /// <param name="parent">Хто викликає</param>
        /// <param name="confirmText">Текст кнопки</param>
        public static int? DocumentEntries(GridView gridView, int rowHandle, Control parent, string confirmText = null)
        {
            return gridView.IsGroupRow(rowHandle) ? null : DocumentEntries(gridView.GetDataRow(rowHandle), parent, confirmText);
        }

        #endregion

        #region Стан документів

        /// <summary> Стан документу </summary>
        /// <param name="row">Запис документу</param>
        /// <param name="owner">Овнер</param>
        public static void DocumentState(DataRow row, Control owner)
        {
            var r = GetDocumentHeader(row, owner);
            if (r != null)
                ShowWfError(r, owner);
        }

        /// <summary> Стан документу </summary>
        /// <param name="gridView">Грид</param>
        /// <param name="rowHandle">Ід. документа</param>
        /// <param name="owner">Овнер</param>
        public static void DocumentState(GridView gridView, int rowHandle, Control owner)
        {
            if (!gridView.IsGroupRow(rowHandle))
                DocumentState(gridView.GetDataRow(rowHandle), owner);
        }

        /// <summary> Звязані документі </summary>
        /// <param name="row">Запис документу</param>
        /// <param name="owner">Овнер</param>
        public static void DocumentLink(DataRow row, Control owner)
        {
            if (row == null)
                return;
            var fS = GetStandartFilterSet(row);
            var hierarchy = new DocumentHierarchy(fS)
            {
                SourceRow = row
            };
            if (owner != null)
                hierarchy.Parameters = new Dictionary<string, object> { { "Owner", owner } };
            FormsActivator.Activate(hierarchy, owner);
        }

        /// <summary> Створити нову ГТД з нуля </summary>
        /// <param name="owner">Овнер</param>
        /// <param name="hintPanel"></param>
        public static void CreateNewCargoDeclaration(Control owner, ReportFormHintPanel hintPanel)
        {
            var mbf = new MessageBoxForm(true, "CreateNewCargoDeclaration");
            FormsActivator.Activate(
                mbf,
                owner,
                () => mbf.Show(
                    (
                        result,
                        dictionary,
                        arg3) =>
                    {
                        if(result != DialogResult.Yes)
                            return;

                        var fs = new FilterSet();
                        foreach(var d in dictionary)
                            fs.SetStaticFilter(d.Key, d.Value);

                        Exception exception = null;
                        DataRow row = null;

                        using(var w = WaitControl.Show(
                            owner,
                            (
                                s,
                                a) =>
                            {
                                exception = new Exception("Прервано пользователем выполнение операции 'Создание ГТД'");

                            },
                            int.MaxValue))
                        {
                            w.Title = "Работаем...";
                            w.Text = "Выполнение операции 'Создание ГТД'";

                            try
                            {
                                var data = UEditDataReader.GetData(fs, "SA.CargoDeclarationNew");
                                row = data["DocumentHeaders"].Rows[0];
                            }
                            catch(Exception ex)
                            {
                                exception = ex;
                            }
                            if(exception != null)
                            {
                                ErrorsHelper.Errors.Add(new KeyValuePair<int, string>(-100, exception.InnerException == null ? exception.Message : exception.InnerException.Message));
                            }

                            var isErrors = ShowMessages(hintPanel);
                            if(isErrors.Key != DialogResult.OK)
                                return;

                            if(row != null)
                                DocumentEdit(GetStandartFilterSet(row), row, owner, hintPanel);
                        }
                    },
                    string.Format("{0}Заполните реквизиты шапки и добавьте артикулы:", Environment.NewLine),
                    "Создание ГТД",
                    MessageBoxButtons.YesNo,
                    additionalInfo:
                    new AdditionalInfoContainer(new CargoDeclarationNew(), "Создание документа ГТД по договору передачи на комиссию")),
                toolTipText: string.Format("Форма была открыта ранее и теперь отображена снова{0}Закончите создание документа ГТД", Environment.NewLine));
        }

        /// <summary> Звязані документі </summary>
        /// <param name="gridView">Грид</param>
        /// <param name="rowHandle">Ід. документа</param>
        /// <param name="owner">Овнер</param>
        public static void DocumentLink(GridView gridView, int rowHandle, Control owner)
        {
            if (!gridView.IsGroupRow(rowHandle))
                DocumentLink(gridView.GetDataRow(rowHandle), owner);
        }

        /// <summary> Відобразити помилку із воркфлоу </summary>
        /// <param name="row">Запись</param>
        /// <param name="owner">Хто викликав</param>
        public static void ShowWfError(DataRow row, Control owner)
        {
            if (row == null)
                return;
            var d = new DocumentStateForm(row);
            FormsActivator.Activate(d, owner);
        }

        /// <summary> Різні варіанти розбору </summary>
        /// <param name="source">Строка</param>
        /// <returns>Те що вийшло в результаті танців</returns>
        public static ICollection<KeyValuePair<string, string>> TryDeserialize(string source)
        {
            var retCollection = new Collection<KeyValuePair<string, string>>();
            bool isDeserialize;

            // Пробую поднять как набор строк
            try
            {
                var messages = Serialization.Deserialize<string[]>(source);
                messages.ForEach(m => SplitMessage(m).ForEach(retCollection.Add));
                isDeserialize = true;
            }
            catch
            {
                isDeserialize = false;
            }

            if (isDeserialize)
                return retCollection;

            // Пробую поднять как коллекцию ошибок
            try
            {
                var messages = Serialization.Deserialize<Collection<MessageError>>(source);
                messages.ForEach(m => SplitMessage(m.Message, m.Type).ForEach(retCollection.Add));
                isDeserialize = true;
            }
            catch
            {
                isDeserialize = false;
            }

            if (isDeserialize)
                return retCollection;

            // Пробую поднять как датасет
            try
            {
                var errSet = new DataSet();
                var reader = new MemoryStream(Encoding.UTF8.GetBytes(source));
                errSet.ReadXml(reader);
                errSet.Tables.Cast<DataTable>().ForEach(t =>
                {
                    t.Rows.Cast<DataRow>().ForEach(r => SplitMessage(string.Format("{0}", r[1]), r[0]).ForEach(retCollection.Add));
                });
                isDeserialize = true;
            }
            catch
            {
                isDeserialize = false;
            }
            if (isDeserialize)
                return retCollection;

            // Просто как строку использую
            SplitMessage(source).ForEach(retCollection.Add);
            return retCollection;
        }

        /// <summary> Спліт повідомлень </summary>
        /// <param name="forSplit">Що бити</param>
        /// <param name="type">Тип повідомлення</param>
        /// <returns></returns>
        private static IEnumerable<KeyValuePair<string, string>> SplitMessage(string forSplit, object type = null)
        {
            var retCollection = new Collection<KeyValuePair<string, string>>();

            var splitted = forSplit.IsNull(string.Empty).Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            splitted.ForEach(s => retCollection.Add(new KeyValuePair<string, string>(string.Format("{0}", type.IsNull(0)), string.Format("{0}", s))));
            return retCollection;
        }

        #endregion

        #region Редагування

        /// <summary> Створення классу операції </summary>
        /// <param name="action">Действие</param>
        /// <param name="fS">Фільтер</param>
        /// <param name="row">Батьківський запис</param>
        /// <param name="owner">Батьківський контрол</param>
        /// <param name="hintPanel">Хинт панель для сообщений</param>
        public static KeyValuePair<DialogResult, KeyValuePair<ReportFormHintPanel.HintState, string>> OperationForm(OperationByActions action, FilterSet fS, DataRow row = null, Control owner = null, ReportFormHintPanel hintPanel = null)
        {
            if (fS == null)
            {
                if (row != null)
                    fS = GetStandartFilterSet(row);
            }

            var err = string.Format("Не задан класс для выполнения операции '{0}'", action.OperationRun);

            var creator = new InstanceCreator();

            // Знайду який XML використовується для редагування
            var operationEditor = SaHelper.HashOperationEditor.FirstOrDefault(p => ComparerHelper.CompareObjects(p.OperationId, action.OperationId) == 0 && action.ActionType == OperationByActions.DocumentActionType);

            var loadedClass = creator.CreateInstance(action.OperationRun);
            if (loadedClass == null)
            {
                UMessage.Show(null, err);
                return new KeyValuePair<DialogResult, KeyValuePair<ReportFormHintPanel.HintState, string>>(DialogResult.Cancel, new KeyValuePair<ReportFormHintPanel.HintState, string>(ReportFormHintPanel.HintState.error, err));
            }

            var iEditform = loadedClass as IEditorForm;
            if (iEditform == null)
            {
                UMessage.Show(null, err);
                return new KeyValuePair<DialogResult, KeyValuePair<ReportFormHintPanel.HintState, string>>(DialogResult.Cancel, new KeyValuePair<ReportFormHintPanel.HintState, string>(ReportFormHintPanel.HintState.error, err));
            }

            iEditform.Filter = fS;
            iEditform.SourceRow = row;
            iEditform.XmlPath = string.IsNullOrWhiteSpace(action.XmlPath)
                ? operationEditor == null
                    ? null
                    : operationEditor.XmlPath
                : action.XmlPath;

            if (hintPanel != null)
                iEditform.Parameters = new Dictionary<string, object> { { "ReportFormHintPanel", hintPanel } };
            if (owner != null)
                iEditform.Parameters = new Dictionary<string, object> { { "Owner", owner } };

            var form = loadedClass as Form;
            return form == null
                ? new KeyValuePair<DialogResult, KeyValuePair<ReportFormHintPanel.HintState, string>>(iEditform.Run(), iEditform.AllMessageInOne)
                : new KeyValuePair<DialogResult, KeyValuePair<ReportFormHintPanel.HintState, string>>(FormsActivator.Activate(form, owner, () => iEditform.Run()), iEditform.AllMessageInOne);
        }

        /// <summary> Створення додаткового діалогу </summary>
        /// <param name="action">Дія</param>
        /// <param name="infoForControl">Інформація для контолу</param>
        /// <param name="hintPanel">Панелька</param>
        /// <returns>Результати</returns>
        public static KeyValuePair<bool, IAdditionalInfo> CreateAddition(OperationByActions action, IDictionary<string, object> infoForControl, ReportFormHintPanel hintPanel)
        {
            if (hintPanel != null)
                hintPanel.Clear();

            if (string.IsNullOrWhiteSpace(action.AdditionalInfo))
                return new KeyValuePair<bool, IAdditionalInfo>(false, null);
            var creator = new InstanceCreator();
            try
            {
                var add = creator.CreateInstance(action.AdditionalInfo, action.AdditionalInfoParameters) as IAdditionalInfo;
                return new KeyValuePair<bool, IAdditionalInfo>(false, add == null ? null : add.InitControlInfo(infoForControl));
            }
            catch (Exception ex)
            {
                ErrorsHelper.Errors.Add(new KeyValuePair<int, string>(-100, ex.InnerException == null ? ex.Message : ex.InnerException.Message));
                ShowMessages(hintPanel);
                return new KeyValuePair<bool, IAdditionalInfo>(true, null);
            }
        }

        /// <summary> Редагувати документ </summary>
        /// <param name="fS">Фільтер</param>
        /// <param name="row">Запис документу</param>
        /// <param name="owner">Овнер</param>
        /// <param name="hintPanel">Хинт панель для сообщений</param>
        public static KeyValuePair<DialogResult, KeyValuePair<ReportFormHintPanel.HintState, string>> DocumentEdit(FilterSet fS, DataRow row = null, Control owner = null, ReportFormHintPanel hintPanel = null)
        {
            if(fS == null)
            {
                if(row == null)
                    return new KeyValuePair<DialogResult, KeyValuePair<ReportFormHintPanel.HintState, string>>(DialogResult.Cancel, new KeyValuePair<ReportFormHintPanel.HintState, string>(ReportFormHintPanel.HintState.error, "Нет записи"));
                fS = GetStandartFilterSet(row);
            }
            var editForm = CreateEditorForm(fS, row, hintPanel);
            var form = editForm.Key as Form;
            if(editForm.Key != null)
            {
                return form == null
                    ? new KeyValuePair<DialogResult, KeyValuePair<ReportFormHintPanel.HintState, string>>(editForm.Key.Run(), editForm.Key.AllMessageInOne)
                    : new KeyValuePair<DialogResult, KeyValuePair<ReportFormHintPanel.HintState, string>>(FormsActivator.Activate(form, owner, () => editForm.Key.Run(), isModal: SaHelper.EditFormProperties[EditorProperties.UseModalForm]==1), editForm.Key.AllMessageInOne);
            }
            var err = !string.IsNullOrWhiteSpace(editForm.Value) ? editForm.Value : string.Format("Не задан класс для редактирования операции '{0}'", fS[OperationId].Value);
            UMessage.Show(null, err);
            return new KeyValuePair<DialogResult, KeyValuePair<ReportFormHintPanel.HintState, string>>(DialogResult.Cancel, new KeyValuePair<ReportFormHintPanel.HintState, string>(ReportFormHintPanel.HintState.error, err));
        }

        /// <summary> Редагувати документ </summary>
        /// <param name="gridView">Грид</param>
        /// <param name="rowHandle">Ід. документа</param>
        /// <param name="owner">Овнер</param>
        /// <param name="hintPanel">Хинт панель для сообщений</param>
        public static KeyValuePair<DialogResult, KeyValuePair<ReportFormHintPanel.HintState, string>> DocumentEdit(GridView gridView, int rowHandle, Control owner = null, ReportFormHintPanel hintPanel = null)
        {
            return gridView.IsGroupRow(rowHandle)
                ? new KeyValuePair<DialogResult, KeyValuePair<ReportFormHintPanel.HintState, string>>(DialogResult.Cancel, new KeyValuePair<ReportFormHintPanel.HintState, string>(ReportFormHintPanel.HintState.error, "Редактирование группы невозможно"))
                : DocumentEdit(null, gridView.GetDataRow(rowHandle), owner, hintPanel);
        }

        /// <summary> Створення форми редагування по операції </summary>
        /// <param name="row">Звідки взяти операцію</param>
        /// <param name="filter">Фільтр</param>
        /// <param name="hintPanel">Хинт панель для сообщений</param>
        /// <returns>Форма</returns>
        public static KeyValuePair<IEditorForm, string> CreateEditorForm(FilterSet filter, DataRow row, ReportFormHintPanel hintPanel = null)
        {
            if (filter[OperationId] == null)
                return new KeyValuePair<IEditorForm, string>(null, "Не задана операция");

            var creator = new InstanceCreator();
            var operationEditor = SaHelper.HashOperationEditor.FirstOrDefault(p => ComparerHelper.CompareObjects(p.OperationId, filter[OperationId].Value) == 0);
            if (operationEditor == null)
                return new KeyValuePair<IEditorForm, string>(null, string.Format("Не задан класс для редактирования операции '{0}'", filter[OperationId].Value));

            var loadedForm = creator.CreateInstance(operationEditor.EditorClass);
            if (loadedForm == null)
                return new KeyValuePair<IEditorForm, string>(null, string.Format("Не создан экземпляр класса '{0}' для редактирования операции '{1}'", operationEditor.EditorClass, filter[OperationId].Value));

            var form = loadedForm as Form;
            if (form == null)
                return new KeyValuePair<IEditorForm, string>(null, string.Format("Класс '{0}' для редактирования операции '{1}' не является формой", operationEditor.EditorClass, filter[OperationId].Value));

            var editForm = form as IEditorForm;
            if (editForm == null)
                return new KeyValuePair<IEditorForm, string>(null, string.Format("Класс '{0}' для редактирования операции '{1}' не является формой редактирования", operationEditor.EditorClass, filter[OperationId].Value));

            editForm.Filter = filter;
            editForm.SourceRow = row;
            editForm.XmlPath = operationEditor.XmlPath;
            if (hintPanel != null)
                editForm.Parameters = new Dictionary<string, object> { { "ReportFormHintPanel", hintPanel } };
            return new KeyValuePair<IEditorForm, string>(editForm, string.Empty);
        }

        /// <summary> Створення хідерконтролу </summary>
        /// <param name="operationId">Операція</param>
        /// <param name="create">Контрол який створюється і це можливо відмінити</param>
        public static void OnHeaderControlCreated(object operationId, ItemEvent<IHeaderControl> create)
        {
            var operationEditor = SaHelper.HashOperationEditor.FirstOrDefault(p => ComparerHelper.CompareObjects(p.OperationId, operationId) == 0);
            if (operationEditor == null)
                return;
            var item = operationEditor.InvisibleControls[create.Item.Name, create.Item.GetType().ToString()];
            if (item != null)
                create.Cancel = true;
        }

        /// <summary> Створення датаконтролу </summary>
        /// <param name="operationId">Операція</param>
        /// <param name="create">Контрол який створюється і це можливо відмінити</param>
        public static void OnDataControlCreated(object operationId, ItemEvent<IDataControl> create)
        {
            var operationEditor = SaHelper.HashOperationEditor.FirstOrDefault(p => ComparerHelper.CompareObjects(p.OperationId, operationId) == 0);
            if (operationEditor == null)
                return;
            var item = operationEditor.InvisibleControls[create.Item.Name, create.Item.GetType().ToString()];
            if (item != null)
                create.Cancel = true;
        }

        /// <summary> Створення додаткового ресурсу </summary>
        /// <param name="operationId">Операція</param>
        /// <param name="create">Ресурс який створюється і це можливо відмінити</param>
        public static void OnResourceLoaded(object operationId, ItemEvent<FormResource> create)
        {
            var operationEditor = SaHelper.HashOperationEditor.FirstOrDefault(p => ComparerHelper.CompareObjects(p.OperationId, operationId) == 0);
            if (operationEditor == null)
                return;
            var item = operationEditor.LoadResources[create.Item.resourceID, create.Item.resourceType.ToString()];
            if (item != null)
                create.Cancel = true;
        }

        /// <summary> Після збереження даних редактором  </summary>
        /// <param name="o">Непотріб</param>
        /// <param name="result">Тут усі дані запаковано, помилки якщо влізуть то і вилізуть</param>
        public static void OnSaveEditResult(object o, SaveResult result)
        {
            if (!result.Data.ContainsKey(MessageTableName))
                return;

            foreach (var row in result.Data[MessageTableName].Rows.Cast<DataRow>().Where(r => !r.IsNull("Type") && !r.IsNull("Message")))
                result.Add(row["Message"].ToString(),
                    Convert.ToInt32(row["Type"]) < 0 ? ReportFormHintPanel.HintState.error
                        : Convert.ToInt32(row["Type"]) > 0
                            ? ReportFormHintPanel.HintState.warning
                            : ReportFormHintPanel.HintState.message);
        }

        /// <summary> Сворити редактор </summary>
        /// <returns>Редактор</returns>
        public static RepositoryItem CreateFieldDicitionary(GridColumn column, ICollection mapDictionary, string valueMember = "Key", string displayMemeber = "Value", bool useTagAsFormat = true, bool reCreateEditor = false, bool validateOnEnter = false)
        {
            if (column == null || mapDictionary == null || mapDictionary.Count==0)
                return null;

            if (column.ColumnEdit != null && !reCreateEditor)
                return column.ColumnEdit;

            var lookupEdit = new RepositoryItemLookUpEdit
            {
                // В умову саме воно попадає
                ValueMember = valueMember,
                DisplayMember = displayMemeber,

                DataSource = mapDictionary,
                TextEditStyle = TextEditStyles.DisableTextEditor,
                SearchMode = SearchMode.OnlyInPopup,
                BestFitMode = BestFitMode.BestFitResizePopup,
                NullText = string.Empty,
                ValidateOnEnterKey = validateOnEnter
            };
            lookupEdit.Columns.Add(new LookUpColumnInfo(valueMember, "Код"));
            lookupEdit.Columns.Add(new LookUpColumnInfo(displayMemeber, "Значение"));
            column.ColumnEdit = lookupEdit;
            return column.ColumnEdit;
        }

        /// <summary> Сворити редактор </summary>
        /// <returns>Редактор</returns>
        public static RepositoryItem CreateFieldEditor(GridColumn column, bool useTagAsFormat = true, bool reCreateEditor = false, bool useValueEx = false, bool useValueExAsKey = false, string mapField = null, bool validateOnEnter = false, Type type = null)
        {
            if (column == null)
                return null;
            if (column.ColumnEdit != null && !reCreateEditor)
                return column.ColumnEdit;

            var realType = column.ColumnType;
            if (column.ColumnType.In(typeof(object)) && type != null)
                realType = type;

            // Есть ли такое в загруженных словарях
            var pa = SaHelper.EntryAttributes.EntryAttribute.Where(e => e.EntryValues != null).FirstOrDefault(e => e.attributeName.Contains(mapField.IsNull(column.FieldName)));
            if (pa != null && pa.attributeEditorType == EntryAttributeAttributeEditorType.lookupEdit)
            {
                var dict = useValueEx
                    ? useValueExAsKey
                        ? pa.EntryValues.Select(p => new
                        {
                            Key = p.ValueEx,
                            Value = string.IsNullOrWhiteSpace(p.ValueEx) ? p.Value : p.ValueEx
                        }).ToList()
                        : pa.EntryValues.Select(p => new
                        {
                            Key = p.Key,
                            Value = string.IsNullOrWhiteSpace(p.ValueEx) ? p.Value : p.ValueEx
                        }).ToList()
                    : useValueExAsKey
                        ? pa.EntryValues.Select(p => new
                        {
                            Key = p.ValueEx,
                            Value = p.Value
                        }).ToList()
                        : pa.EntryValues.Select(p => new
                        {
                            Key = p.Key,
                            Value = p.Value
                        }).ToList();

                var lookupEdit = new RepositoryItemLookUpEdit
                {
                    // В умову саме воно попадає
                    ValueMember = "Key",
                    DisplayMember = "Value",

                    DataSource = dict,
                    TextEditStyle = TextEditStyles.DisableTextEditor,
                    SearchMode = SearchMode.OnlyInPopup,
                    BestFitMode = BestFitMode.BestFitResizePopup,
                    NullText = string.Empty,
                    ValidateOnEnterKey = validateOnEnter
                };
                lookupEdit.Columns.Add(new LookUpColumnInfo("Key", "Код"));
                lookupEdit.Columns.Add(new LookUpColumnInfo("Value", "Значение"));
                column.ColumnEdit = lookupEdit;
                return column.ColumnEdit;
            }

            // Чи є в закешованих
            if (SaHelper.DictMappings.ContainsKey(mapField.IsNull(column.FieldName)))
            {
                var hashedDict = SaHelper.DictMappings[mapField.IsNull(column.FieldName)];
                var editor = CreateFieldDicitionary(column, hashedDict.Dictionary, hashedDict.ValueMember, hashedDict.DisplayMember, useTagAsFormat, reCreateEditor, validateOnEnter);
                if (editor != null)
                    return editor;
            }

            var columnTag = useTagAsFormat ? string.Format("{0}", column.Tag) : null;
            if (realType.In(typeof(decimal), typeof(float), typeof(double), typeof(sbyte), typeof(byte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong)))
            {
                var ct = new RepositoryItemTextEdit
                {
                    ValidateOnEnterKey = validateOnEnter
                };
                ct.DisplayFormat.FormatType = FormatType.Numeric;
                ct.Mask.MaskType = MaskType.Numeric;
                ct.DisplayFormat.FormatString = column.DisplayFormat.FormatString;
                ct.Mask.EditMask = string.IsNullOrWhiteSpace(columnTag) ? column.DisplayFormat.FormatString : columnTag;
                column.ColumnEdit = ct;
                return column.ColumnEdit;
            }

            if (realType.In(typeof(DateTime)))
            {
                var ct = new RepositoryItemDateEdit
                {
                    ValidateOnEnterKey = validateOnEnter
                };
                ct.DisplayFormat.FormatString = column.DisplayFormat.FormatString;
                ct.Mask.MaskType = MaskType.DateTime;
                //@"G"-  Дата время полное
                ct.Mask.EditMask = string.IsNullOrWhiteSpace(columnTag) ? @"G" : columnTag;
                column.ColumnEdit = ct;
                return column.ColumnEdit;
            }

            if (realType.In(typeof(bool)))
            {
                column.ColumnEdit = new RepositoryItemCheckEdit
                {
                    AllowGrayed = !string.IsNullOrWhiteSpace(columnTag)
                };
                return column.ColumnEdit;
            }

            var defaultEditor = column.AppearanceCell.TextOptions.WordWrap == WordWrap.Wrap ? new RepositoryItemMemoEdit() : new RepositoryItemTextEdit();

            defaultEditor.ValidateOnEnterKey = validateOnEnter;
            defaultEditor.DisplayFormat.FormatType = column.DisplayFormat.FormatType;
            defaultEditor.DisplayFormat.FormatString = column.DisplayFormat.FormatString;
            if (string.IsNullOrWhiteSpace(columnTag))
            {
                defaultEditor.Mask.MaskType = MaskType.None;
                defaultEditor.Mask.EditMask = null;
            }
            else
            {
                defaultEditor.Mask.MaskType = MaskType.RegEx;
                defaultEditor.Mask.UseMaskAsDisplayFormat = true;
                defaultEditor.Mask.ShowPlaceHolders = false;
                defaultEditor.Mask.EditMask = columnTag;
            }
            column.ColumnEdit = defaultEditor;
            return column.ColumnEdit;
        }

        /// <summary> Створити довідник із вже завантажених даних  </summary>
        /// <param name="lookUpEdit">Контрол довідника</param>
        /// <param name="predicate">Додаткова умова</param>
        /// <param name="mapField">Яке поле</param>
        /// <param name="useValueEx">Використовувати додаткове значення як значення</param>
        /// <param name="useValueExAsKey">Використовувати додаткове значення як ключ</param>
        public static void CreateLookUpDictionary(LookUpEdit lookUpEdit, string mapField, Func<EntryAttributeValue, bool> predicate = null, bool useValueEx = false, bool useValueExAsKey = false)
        {
            var pa = SaHelper.EntryAttributes.EntryAttribute.Where(e => e.EntryValues != null).FirstOrDefault(a => a.attributeName.Contains(mapField));
            if (pa == null || pa.attributeEditorType != EntryAttributeAttributeEditorType.lookupEdit)
                return;
            lookUpEdit.Properties.ValueMember = "Key";
            lookUpEdit.Properties.DisplayMember = "Value";

            lookUpEdit.Properties.DataSource = predicate == null
                ? useValueEx
                    ? useValueExAsKey
                        ? pa.EntryValues.Select(p => new
                        {
                            Key = p.ValueEx,
                            Value = string.IsNullOrWhiteSpace(p.ValueEx) ? p.Value : p.ValueEx
                        }).ToList()
                        : pa.EntryValues.Select(p => new
                        {
                            Key = p.Key,
                            Value = string.IsNullOrWhiteSpace(p.ValueEx) ? p.Value : p.ValueEx
                        }).ToList()
                    : useValueExAsKey
                        ? pa.EntryValues.Select(p => new
                        {
                            Key = p.ValueEx,
                            Value = p.Value
                        }).ToList()
                        : pa.EntryValues.Select(p => new
                        {
                            Key = p.Key,
                            Value = p.Value
                        }).ToList()

                : useValueEx
                    ? useValueExAsKey
                        ? pa.EntryValues.Where(predicate).Select(p => new
                        {
                            Key = p.ValueEx,
                            Value = string.IsNullOrWhiteSpace(p.ValueEx) ? p.Value : p.ValueEx
                        }).ToList()
                        : pa.EntryValues.Where(predicate).Select(p => new
                        {
                            Key = p.Key,
                            Value = string.IsNullOrWhiteSpace(p.ValueEx) ? p.Value : p.ValueEx
                        }).ToList()
                    : useValueExAsKey
                        ? pa.EntryValues.Where(predicate).Select(p => new
                        {
                            Key = p.ValueEx,
                            Value = p.Value
                        }).ToList()
                        : pa.EntryValues.Where(predicate).Select(p => new
                        {
                            Key = p.Key,
                            Value = p.Value
                        }).ToList();


            lookUpEdit.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            lookUpEdit.Properties.SearchMode = SearchMode.OnlyInPopup;
            lookUpEdit.Properties.BestFitMode = BestFitMode.BestFit;

            if (lookUpEdit.Properties.Columns["Key"] == null)
                lookUpEdit.Properties.Columns.Add(new LookUpColumnInfo("Key", "Код"));

            if (lookUpEdit.Properties.Columns["Value"] == null)
                lookUpEdit.Properties.Columns.Add(new LookUpColumnInfo("Value", "Значение"));
        }

        /// <summary> Створити довідник із вже завантажених даних  </summary>
        /// <param name="control">Контрол довідника</param>
        /// <param name="predicate">Додаткова умова</param>
        /// <param name="useValueEx">Використовувати додаткове значення як значення</param>
        /// <returns>Кількість записів</returns>
        public static int CreateIHeaderControlDictionary(IHeaderControl control, Func<EntryAttributeValue, bool> predicate = null, bool useValueEx = false)
        {
            var pa = SaHelper.EntryAttributes.EntryAttribute.Where(e => e.EntryValues != null).FirstOrDefault(e => e.attributeName.Contains(control.Name));
            if (pa == null || pa.attributeEditorType != EntryAttributeAttributeEditorType.lookupEdit)
                return 0;

            var dt = predicate == null
                ? useValueEx
                    ? pa.EntryValues.Select(p => new
                    {
                        Key = p.Key,
                        Value = string.IsNullOrWhiteSpace(p.ValueEx) ? p.Value : p.ValueEx
                    }).ToDataTable()
                    : pa.EntryValues.Select(p => new
                    {
                        Key = p.Key,
                        Value = p.Value
                    }).ToDataTable()
                : useValueEx
                    ? pa.EntryValues.Where(predicate).Select(p => new
                    {
                        Key = p.Key,
                        Value = string.IsNullOrWhiteSpace(p.ValueEx) ? p.Value : p.ValueEx
                    }).ToDataTable()
                    : pa.EntryValues.Where(predicate).Select(p => new
                    {
                        Key = p.Key,
                        Value = p.Value
                    }).ToDataTable();

            dt.TableName = "Data";
            control.ToSource(dt);
            return dt.Rows.Count;
        }

        #endregion

        /// <summary> Покажу сообщения </summary>
        public static KeyValuePair<DialogResult, KeyValuePair<ReportFormHintPanel.HintState, string>> ShowMessages(ReportFormHintPanel hintPanel = null)
        {
            var res = DialogResult.OK;
            var ret = ErrorsHelper.ToText(true);
            if (ret.Value.IsNull(string.Empty).Length == 0)
                return new KeyValuePair<DialogResult, KeyValuePair<ReportFormHintPanel.HintState, string>>(res, new KeyValuePair<ReportFormHintPanel.HintState, string>(ReportFormHintPanel.HintState.message, string.Empty));
            ErrorsHelper.Errors.Clear();
            // Є помилки
            if (ret.Key < 0)
                res = DialogResult.Cancel;
            // Є попередження
            if (ret.Key > 0)
                res = DialogResult.Ignore;
            if (hintPanel != null)
                hintPanel.AppendMessage(ret.Value, ret.Key < 0 ? ReportFormHintPanel.HintState.error : ret.Key > 0 ? ReportFormHintPanel.HintState.warning : ReportFormHintPanel.HintState.message);
            else
                UMessage.Show(null, ret.Value, size: LagreMessageSize, icon: ret.Key < 0 ? MessageBoxIcon.Error : ret.Key > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);

            return new KeyValuePair<DialogResult, KeyValuePair<ReportFormHintPanel.HintState, string>>(res, new KeyValuePair<ReportFormHintPanel.HintState, string>(ret.Key < 0 ? ReportFormHintPanel.HintState.error : ret.Key > 0 ? ReportFormHintPanel.HintState.warning : ReportFormHintPanel.HintState.message, ret.Value));
        }

        /// <summary> Покажу сообщения </summary>
        public static KeyValuePair<DialogResult, KeyValuePair<ReportFormHintPanel.HintState, string>> ShowMessages(KeyValuePair<DialogResult, KeyValuePair<ReportFormHintPanel.HintState, string>> message, ReportFormHintPanel hintPanel = null)
        {
            if (message.Value.Value.IsNull(string.Empty).Length == 0)
                return message;

            if (hintPanel != null)
                hintPanel.AppendMessage(message.Value.Value, message.Value.Key);
            else
                UMessage.Show(null, message.Value.Value, size: LagreMessageSize, icon: message.Value.Key == ReportFormHintPanel.HintState.error ? MessageBoxIcon.Error : message.Value.Key == ReportFormHintPanel.HintState.warning ? MessageBoxIcon.Warning : MessageBoxIcon.Information);

            return message;
        }

        /// <summary> ВИКОНАТИ ДІЮ НАД ОДНИМ КОНКРЕТНИМ ЕЛЕМЕНТОМ</summary>
        /// <param name="r">Документа</param>
        /// <param name="action">Що робити</param>
        /// <param name="owner">Контрол</param>
        /// <param name="hintPanel">Хинт панель для сообщений</param>
        public static KeyValuePair<DialogResult, KeyValuePair<ReportFormHintPanel.HintState, string>> SendOneItem(DataRow r, OperationByActions action, Control owner = null, ReportFormHintPanel hintPanel = null)
        {
            var res = new KeyValuePair<DialogResult, KeyValuePair<ReportFormHintPanel.HintState, string>>(DialogResult.OK, new KeyValuePair<ReportFormHintPanel.HintState, string>(ReportFormHintPanel.HintState.message, string.Empty));
            var add = CreateAddition(action, new Dictionary<string, object> { { "Operation", action }, { "Owner", owner }, { "DataRow", r }, { "ReportFormHintPanel", hintPanel } }, hintPanel);
            if (add.Key)
                return new KeyValuePair<DialogResult, KeyValuePair<ReportFormHintPanel.HintState, string>>(DialogResult.Cancel, new KeyValuePair<ReportFormHintPanel.HintState, string>(ReportFormHintPanel.HintState.message, string.Empty));

            UMessage.Show((result, dictionary, arg3) =>
            {
                if (result != DialogResult.Yes)
                    return;

                var showEntriesBefore = false;

                // Додам до фільтра все що прийшло з додаткового
                foreach (var pair in dictionary)
                {
                    action.AddFilterSet.Add(pair);
                    if (string.Compare(pair.Key, "SA.EntriesQueueProcessing", StringComparison.Ordinal) == 0)
                        showEntriesBefore = pair.Value.IsNull(false);
                }

                // Проводки до проведення
                if (showEntriesBefore)
                {
                    if (DocumentEntries(r, owner, action.ActionName) != int.MaxValue)
                        return;
                }

                res = SendOneItemNoQuestion(r, action, owner, hintPanel);
            }, string.Format("Действительно выполнить над текущим документом операцию{0}{0}'{1}'?", Environment.NewLine, action.ActionName),
                string.Format("Выполнение '{0}'", action.ActionName), MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2, MessageBoxIcon.Question, additionalInfo: add.Value);

            return res;
        }

        /// <summary> ВИКОНАТИ ДІЮ НАД ОДНИМ КОНКРЕТНИМ ЕЛЕМЕНТОМ БЕЗ ПИТАНЬ</summary>
        /// <param name="r">Документа</param>
        /// <param name="action">Що робити</param>
        /// <param name="owner">Контрол</param>
        /// <param name="hintPanel">Хинт панель для сообщений</param>
        /// <param name="sourceRow">Запис для замальовки</param>
        public static KeyValuePair<DialogResult, KeyValuePair<ReportFormHintPanel.HintState, string>> SendOneItemNoQuestion(DataRow r, OperationByActions action, Control owner = null, ReportFormHintPanel hintPanel = null, DataRow sourceRow = null)
        {
            var result = new KeyValuePair<DialogResult, KeyValuePair<ReportFormHintPanel.HintState, string>>(DialogResult.Cancel, new KeyValuePair<ReportFormHintPanel.HintState, string>(ReportFormHintPanel.HintState.message, string.Empty));

            // Якщо 4 та 5 то робим парентами
            var fS = GetStandartFilterSet(r, action.ModeGroupOperation.In(4, 5));

            // Додам до фільтра все що прийшло зверху
            foreach (var item in action.AddFilterSet)
                fS[item.Key] = new FilterSetItem(item.Key, FilterType.Static, item.Value);

            // Добавляю, ЧТО ДЕЛАТЬ С ДОКУМЕНТОМ
            fS[action.ActionType] = new FilterSetItem(action.ActionType, FilterType.Static, action.ActionId);

            Exception exception = null;
            switch (action.ModeGroupOperation)
            {
                //Результат только таблицы
                //0 На элементе 
                //1 На группе(поэлементно) и на элементе
                //12 На группе(все элементы) и на элементе 
                case 0:
                case 1:
                case 12:
                    DataRow newDocument = null;
                    using (var w = WaitControl.Show(owner, (s, a) =>
                    {
                        exception = new Exception(string.Format("Прервано пользователем выполнение операции '{0}'", action.ActionName));
                    }, int.MaxValue))
                    {
                        w.Title = "Работаем...";
                        w.Text = string.Format("Выполнение операции '{0}'", action.ActionName);
                        try
                        {
                            var data = UEditDataReader.GetData(fS, action.OperationRun);
                            foreach (var dt in data.Where(d => string.Compare(d.Key, HeaderTableName, StringComparison.OrdinalIgnoreCase) != 0))
                            {
                                if (string.Compare(dt.Key, MessageTableName, StringComparison.OrdinalIgnoreCase) == 0)
                                {
                                    ErrorsHelper.FillErrors(dt.Value);
                                    continue;
                                }
                                if (dt.Value.Rows.Count != 1)
                                    continue;
                                // Таблиця документу та режим відкрити редактор. збережу фільтр
                                if (action.ModeEditor.In(3) && string.Compare(dt.Key, DocumentHeader, StringComparison.OrdinalIgnoreCase) == 0)
                                {
                                    newDocument = dt.Value.Rows[0];
                                    fS = GetStandartFilterSet(newDocument);
                                }
                                RefreshData(r, dt.Value.Rows[0]);
                                RefreshData(sourceRow, dt.Value.Rows[0]);
                            }
                        }
                        catch (Exception ex)
                        {
                            exception = ex;
                        }
                        if (exception != null)
                            ErrorsHelper.Errors.Add(new KeyValuePair<int, string>(-100, exception.InnerException == null ? exception.Message : exception.InnerException.Message));

                        result = ShowMessages(hintPanel);
                        // Відкрити редактор
                        if (newDocument != null)
                            DocumentEdit(fS, newDocument,owner);
                    }
                    break;
                //Редактор(IEditorForm)
                //4 На элементе  
                //5 На группе(все элементы) и на элементе 

                //Форма(IEditorForm или нет)
                //8  На элементе
                //9  На группе(все элементы) и на элементе 
                case 4:
                case 5:
                case 8:
                case 9:
                    if (action.ModeGroupOperation.In(4, 5))
                    {
                        fS[OperationId] = new FilterSetItem(OperationId, FilterType.Static, action.OperationRun);
                        // Переношу усі які треба поля з оригіналу
                        foreach (var column in r.Table.Columns.Cast<DataColumn>().Where(c => c.ColumnName.In(SaHelper.HashIncludedColumn.ToArray()) && !r.IsNull(c.ColumnName)))
                            fS[column.ColumnName] = new FilterSetItem(column.ColumnName, FilterType.Static, MicroSerializer.CreateValue(r[column.ColumnName]));
                        result = DocumentEdit(fS, r, owner);
                    }
                    else
                    {
                        result = ShowMessages(OperationForm(action, fS, r, owner, hintPanel), hintPanel);
                    }
                    break;
            }
            return result;
        }

        /// <summary> ВЫПОЛНЯЕТСЯ НА ГРУППЕ </summary>
        /// <param name="gridView">Грид</param>
        /// <param name="groupRowHandle">Ід. групи</param>
        /// <param name="action">Що робити</param>
        /// <param name="owner">Контрол</param>
        /// <param name="hintPanel">Хинт панель для сообщений</param>
        public static void SendMarkedItems(GridView gridView, int groupRowHandle, OperationByActions action, Control owner = null, ReportFormHintPanel hintPanel = null)
        {
            var handledRows = gridView.FindItems(groupRowHandle, (row, handle) =>
            {
                DataRow r = null;
                var t = row.GetType();
                if (t == typeof(DataRowView) || t.IsSubclassOf(typeof(DataRowView)))
                {
                    var gridR = row as DataRowView;
                    if (gridR != null)
                        r = gridR.Row;
                }
                if (t == typeof(DataRow) || t.IsSubclassOf(typeof(DataRow)))
                    r = row as DataRow;

                if (r == null)
                    return false;

                // Запис відмічений та для неї дана дія передбачена?
                return r[IsChecked].ConvertTo<bool>() && IsActionEnabled(r, action);
            }, false);

            // Не створює новий документ и нема записів - допобачення
            if (!action.ModeGroupOperation.In(7, 11) && handledRows.Count == 0)
            {
                UMessage.Show(null, string.Format("Не отмечены ПОДХОДЯЩИЕ документы для выполнения операции{0}{0}'{1}'.", Environment.NewLine, action.ActionName), string.Format("Выполнение '{0}'", action.ActionName), MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);
                return;
            }

            var add = CreateAddition(action, new Dictionary<string, object> { { "Operation", action }, { "Owner", owner }, { "DataRowCollection", handledRows.Values }, { "ReportFormHintPanel", hintPanel } }, hintPanel);
            if (add.Key)
                return;

            Exception exception = null;
            FilterSet fS = null;

            switch (action.ModeGroupOperation)
            {
                //Результат только таблицы
                //1 На группе и на элементе выполняется поэлементно
                //2 На группе               выполняется поэлементно
                case 1:
                case 2:
                    UMessage.Show((result, dictionary, arg3) =>
                    {
                        if (result != DialogResult.Yes)
                            return;

                        using (var w = WaitControl.Show(owner, (s, a) =>
                        {
                            exception = new Exception(string.Format("Прервано пользователем выполнение операции '{0}'", action.ActionName));
                        }, int.MaxValue))
                        {
                            w.Title = "Работаем...";
                            w.Text = string.Format("Выполнение операции '{0}'", action.ActionName);

                            gridView.BeginDataUpdate();
                            foreach (var r in handledRows.Values.Cast<DataRowView>().Where(r => r != null && r.Row != null))
                            {
                                Application.DoEvents();

                                fS = GetStandartFilterSet(r.Row);

                                // Додам до фільтра все що прийшло зверху
                                foreach (var item in action.AddFilterSet)
                                    fS[item.Key] = new FilterSetItem(item.Key, FilterType.Static, item.Value);

                                // Додам до фільтра все що прийшло з додаткового
                                foreach (var pair in dictionary)
                                    fS[pair.Key] = new FilterSetItem(pair.Key, FilterType.Static, pair.Value);

                                // Добавляю, ЧТО ДЕЛАТЬ С ДОКУМЕНТОМ
                                fS[action.ActionType] = new FilterSetItem(action.ActionType, FilterType.Static, action.ActionId);

                                try
                                {
                                    // Документ у чергу на повну обробку
                                    var data = UEditDataReader.GetData(fS, action.OperationRun);
                                    // Была постановка в очередь
                                    foreach (var dt in data.Where(d => string.Compare(d.Key, HeaderTableName, StringComparison.OrdinalIgnoreCase) != 0))
                                    {
                                        if (string.Compare(dt.Key, MessageTableName, StringComparison.OrdinalIgnoreCase) == 0)
                                        {
                                            ErrorsHelper.FillErrors(dt.Value);
                                            continue;
                                        }

                                        if (dt.Value.Rows.Count == 1)
                                            RefreshData(r.Row, dt.Value.Rows[0]);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    exception = ex;
                                }
                                if (exception == null)
                                    continue;
                                ErrorsHelper.Errors.Add(new KeyValuePair<int, string>(-100, exception.InnerException == null ? exception.Message : exception.InnerException.Message));
                                break;
                            }
                            gridView.EndDataUpdate();
                            ShowMessages(hintPanel);
                        }
                    }, string.Format("Действительно выполнить над ними операцию{0}{0}'{1}'?", Environment.NewLine, action.ActionName),
                        string.Format("Выполнение '{0}'", action.ActionName), MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2, MessageBoxIcon.Question, additionalInfo: new AdditionalInfoContainer(add.Value, string.Format("В группе отмечено ПОДХОДЯЩИХ документов : {0}", handledRows.Count)));
                    break;
                //Результат только таблицы
                //3 На группе               выполняется над всей группой

                //Редактор(IEditorForm)
                //5 На группе(все элементы) и на элементе 
                //6 На группе(все элементы)

                //Форма(IEditorForm или нет)
                //9  На группе(все элементы) и на элементе 
                //10 На группе(все элементы)
                case 3:
                case 5:
                case 6:
                case 9:
                case 10:
                case 12:
                    foreach (var row in handledRows.Values.Cast<DataRowView>().Where(r => r != null && r.Row != null))
                        fS = fS == null ? GetStandartFilterSet(row.Row) : AddStandartValue(fS, row.Row);

                    if (fS == null)
                        return;

                    // Додам до фільтра все що прийшло зверху
                    foreach (var item in action.AddFilterSet)
                        fS[item.Key] = new FilterSetItem(item.Key, FilterType.Static, item.Value);

                    // Добавляю, ЧТО ДЕЛАТЬ С ДОКУМЕНТОМ
                    fS[action.ActionType] = new FilterSetItem(action.ActionType, FilterType.Static, action.ActionId);

                    UMessage.Show((result, dictionary, arg3) =>
                    {
                        if (result != DialogResult.Yes)
                            return;

                        // Додам до фільтра все що прийшло з додаткового
                        foreach (var pair in dictionary)
                            fS[pair.Key] = new FilterSetItem(pair.Key, FilterType.Static, pair.Value);

                        switch (action.ModeGroupOperation)
                        {
                            case 3:
                            case 12:
                                DataRow newDocument = null;
                                using (var w = WaitControl.Show(owner, (s, a) =>
                                {
                                    exception = new Exception(string.Format("Прервано пользователем выполнение операции '{0}'", action.ActionName));
                                }, int.MaxValue))
                                {
                                    w.Title = "Работаем...";
                                    w.Text = string.Format("Выполнение операции '{0}'", action.ActionName);

                                    gridView.BeginDataUpdate();
                                    try
                                    {
                                        var data = UEditDataReader.GetData(fS, action.OperationRun);
                                        foreach (var dt in data.Where(d => string.Compare(d.Key, HeaderTableName, StringComparison.OrdinalIgnoreCase) != 0))
                                        {
                                            if (string.Compare(dt.Key, MessageTableName, StringComparison.OrdinalIgnoreCase) == 0)
                                            {
                                                ErrorsHelper.FillErrors(dt.Value);
                                                continue;
                                            }
                                            // Якщо немає колонки DocumentId - допобачення
                                            if (!dt.Value.Columns.Contains(DocumentId))
                                                continue;

                                            // Таблиця документу та режим відкрити редактор. збережу фільтр
                                            if (action.ModeEditor.In(3) && string.Compare(dt.Key, DocumentHeader, StringComparison.OrdinalIgnoreCase) == 0)
                                            {
                                                newDocument = dt.Value.Rows[0];
                                                fS = GetStandartFilterSet(newDocument);
                                            }

                                            foreach (var r in handledRows.Values.Cast<DataRowView>().Where(r => r != null && r.Row != null))
                                            {
                                                var r1 = r.Row;
                                                dt.Value.Rows.Cast<DataRow>().Where(rr => ComparerHelper.CompareObjects(rr[DocumentId], r1[DocumentId]) == 0).ForEach(rr => RefreshData(r1, rr));
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        exception = ex;
                                    }

                                    if (exception != null)
                                        ErrorsHelper.Errors.Add(new KeyValuePair<int, string>(-100, exception.InnerException == null ? exception.Message : exception.InnerException.Message));

                                    gridView.EndDataUpdate();
                                    ShowMessages(hintPanel);
                                    // Відкрити редактор
                                    if (newDocument != null)
                                        DocumentEdit(fS, newDocument, owner);
                                }
                                break;
                            case 5:
                            case 6:
                                // ТРЕБА ОПЕРАЦІЮ
                                fS[OperationId] = new FilterSetItem(OperationId, FilterType.Static, action.OperationRun);
                                DocumentEdit(fS, null, owner);
                                break;
                            case 9:
                            case 10:
                                if (ShowMessages(OperationForm(action, fS, null, owner, hintPanel), hintPanel).Key == DialogResult.OK)
                                {
                                    var sourceTable = GetDocumentHeader(fS, owner);
                                    if (sourceTable == null)
                                        break;
                                    // Якщо немає колонки DocumentId - допобачення
                                    if (!sourceTable.Columns.Contains(DocumentId))
                                        break;

                                    gridView.BeginDataUpdate();
                                    foreach (var r in handledRows.Values.Cast<DataRowView>().Where(r => r != null && r.Row != null))
                                    {
                                        var r1 = r.Row;
                                        sourceTable.Rows.Cast<DataRow>().Where(rr => ComparerHelper.CompareObjects(rr[DocumentId], r1[DocumentId]) == 0).ForEach(rr => RefreshData(r1, rr));
                                    }
                                    gridView.EndDataUpdate();
                                }
                                break;
                        }
                    }, string.Format("Действительно выполнить над ними операцию{0}{0}'{1}'?", Environment.NewLine, action.ActionName),
                        string.Format("Выполнение '{0}'", action.ActionName), MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2, MessageBoxIcon.Question, additionalInfo: new AdditionalInfoContainer(add.Value, string.Format("В группе отмечено ПОДХОДЯЩИХ документов : {0}", handledRows.Count)));
                    break;
                //Редактор(IEditorForm)
                //7 На группе(просто новый)

                //Форма(IEditorForm или нет)
                //11 На группе(просто новый)
                case 7:
                case 11:
                    fS = GetNewFilterSet(null);
                    // Додам до фільтра все що прийшло зверху
                    foreach (var item in action.AddFilterSet)
                        fS[item.Key] = new FilterSetItem(item.Key, FilterType.Static, item.Value);
                    // Добавляю, ЧТО ДЕЛАТЬ С ДОКУМЕНТОМ
                    fS[action.ActionType] = new FilterSetItem(action.ActionType, FilterType.Static, action.ActionId);
                    // ТРЕБА ОПЕРАЦІЮ
                    fS[OperationId] = new FilterSetItem(OperationId, FilterType.Static, action.OperationRun);

                    if(action.ModeGroupOperation.In(7))
                        DocumentEdit(fS, null, owner);
                    else
                        ShowMessages(OperationForm(action, fS, null, owner, hintPanel), hintPanel);
                    break;
            }
        }

        #region Контекстне меню

        /// <summary> Створення випадаючого меню конкретного запису </summary>
        /// <param name="gridView">Грід</param>
        /// <param name="rowHandle">Запис</param>
        /// <param name="m">Меню, до якого додати</param>
        /// <param name="owner">Передати кудись далі</param>
        /// <param name="someItems">Це теж додати</param>
        /// <returns>Оновлене меню</returns>
        public static ContextMenuStrip AddMainItemMenu(ContextMenuStrip m, GridView gridView, int rowHandle, Control owner = null, ToolStripItem[] someItems = null)
        {
            // Для группы не катит
            if(gridView.IsGroupRow(rowHandle) || !gridView.IsValidRowHandle(rowHandle))
                return m;

            var row = gridView.GetDataRow(rowHandle);
            if(row == null)
                return m;

            if(m == null)
                m = new ContextMenuStrip
                {
                    ShowImageMargin = false
                };

            m.Items.AddRange(
                new ToolStripItem[]
                {
                    new ToolStripMenuItem("Редактировать", null, (s, ev) => DocumentEdit(null, (DataRow)((ToolStripItem)s).Tag, owner)) {Tag = row},
                    new ToolStripMenuItem("Правила и проводки", null, (s, ev) => DocumentEntries((DataRow)((ToolStripItem)s).Tag, owner)) {Tag = row, Enabled = FZCoreProxy.Session.IsOperationAvailable("SA.EntriesQueueProcessing")},
                    new ToolStripMenuItem("Состояние", null, (s, ev) => DocumentState((DataRow)((ToolStripItem)s).Tag, owner)) {Tag = row, Enabled = FZCoreProxy.Session.IsOperationAvailable("SA.DocumentsRevision") && FZCoreProxy.Session.IsOperationAvailable("SA.Forms.GetDocumentHeaders")},
                    new ToolStripMenuItem("Связанные документы", null, (s, ev) => DocumentLink((DataRow)((ToolStripItem)s).Tag, owner)) {Tag = row, Enabled = FZCoreProxy.Session.IsOperationAvailable("SA.Forms.DocumentHierarchy")},
                });
            if(someItems == null || someItems.Length == 0)
                return m;
            m.Items.Add(new ToolStripSeparator());
            m.Items.AddRange(someItems);
            return m;
        }

        /// <summary> Створення меню дій конкретного запису або групи</summary>
        /// <param name="gridView">Грід</param>
        /// <param name="rowHandle">Запис</param>
        /// <param name="addFilter">Те що додати до фвльтерсету операції</param>
        /// <param name="m">Меню, до якого додати</param>
        /// <param name="owner">Передати кудись далі</param>
        /// <param name="usedOperations">Операції доступні у статусі(щоб непотріб не тягти) </param>
        /// <param name="isUsed">Включать или исключать usedOperation</param>
        /// <param name="hintPanel">Хинт панель для сообщений</param>
        /// <param name="sameStatusId">Задавати обережно якщо впевнені, що всі документи в одному статусі(тобто статус найвища група)</param>
        /// <returns>Оновлене меню</returns>
        public static ContextMenuStrip AddActionMenu(ContextMenuStrip m, GridView gridView, int rowHandle, IDictionary<string, string> addFilter = null, Control owner = null, IList<object> usedOperations = null, bool isUsed = true, ReportFormHintPanel hintPanel = null, bool sameStatusId = false)
        {
            var row = gridView.GetDataRow(rowHandle);
            if (row == null || !gridView.IsValidRowHandle(rowHandle))
                return m;

            if (m == null)
                m = new ContextMenuStrip
                {
                    ShowImageMargin = false
                };

            var statusId = row[DocumentStatus];
            var operationId = row[OperationId];
            // Чи группа
            var isGroup = gridView.IsGroupRow(rowHandle);
            // Якщо группа то взнати яка
            if (isGroup)
            {
                operationId = null;
                var grStatusLevel = int.MaxValue;
                var grOperationLevel = int.MaxValue;
                var grOperationNameLevel = int.MaxValue;
                var grLevel = gridView.GetRowLevel(rowHandle);
                var grName = string.Empty;
                // Визначив порядок груп, якщо вони є
                for (var i = 0; i < gridView.GroupedColumns.Count; i++)
                {
                    var gr = gridView.GroupedColumns[i];
                    if (string.Compare(gr.FieldName, DocumentStatus, StringComparison.OrdinalIgnoreCase) == 0)
                        grStatusLevel = i;
                    if (string.Compare(gr.FieldName, OperationName, StringComparison.OrdinalIgnoreCase) == 0)
                        grOperationNameLevel = i;
                    if (string.Compare(gr.FieldName, OperationId, StringComparison.OrdinalIgnoreCase) == 0)
                        grOperationLevel = i;

                    if (i == grLevel)
                        grName = gr.FieldName;
                }

                // Якщо всі документи в одному статусі то група СТАТУС сама головна
                if (sameStatusId)
                    grStatusLevel = int.MinValue;

                // Перевіримо по якому полю група на якій натиснули
                switch (grName)
                {
                    case DocumentStatus:// Група по статусу
                        //Якщо вище(номер меньше) є група по операції то додаю до меню дію саме по операції
                        if (grOperationLevel < grStatusLevel || grOperationNameLevel < grStatusLevel)
                            operationId = row[OperationId];
                        break;
                    case OperationName:
                        //Якщо вище(номер меньше) є група по статусу то додаю до меню дію саме по операції
                        if (grStatusLevel >= grOperationNameLevel)
                            return m;
                        operationId = row[OperationId];
                        break;
                    case OperationId:
                        //Якщо вище(номер меньше) є група по статусу то додаю до меню дію саме по операції
                        if (grStatusLevel >= grOperationLevel)
                            return m;
                        operationId = row[OperationId];
                        break;
                    default:// Група по чому завгодно
                        // Перевіряю,чи вище є група по статусу
                        // Якщо група статусу нижче групи поточної то дії неможливі
                        if (grStatusLevel >= grLevel)
                            return m;
                        // Тут група статусу ВИЩЕ групи поточної і перевіримо наявність групи по операції вище поточної групи
                        if (grOperationLevel < grLevel || grOperationNameLevel < grLevel)
                            operationId = row[OperationId];
                        break;
                }
            }
            var actions = isGroup ? GetGroupAction(statusId, operationId, usedOperations, isUsed).ToList() : GetSingleAction(row).ToList();
            if (!actions.Any())
                return m;

            if (addFilter == null)
                addFilter = new Dictionary<string, string>();

            var drop = new ToolStripMenuItem(string.Format("{0}", isGroup ? "Проводки по отмеченным в группе" : "Проводки по текущему документу"));
            var countAction = actions.Count(a => string.CompareOrdinal(a.ActionType, OperationByActions.DefaultActionType) == 0);
            var cAction = countAction;

            if (m.Items.Count > 0 && cAction > 0)
                m.Items.Add(new ToolStripSeparator());

            foreach (var item in actions.Where(a => string.CompareOrdinal(a.ActionType, OperationByActions.DefaultActionType) == 0).Select(r => CreateMenuItem(r, gridView, rowHandle, isGroup, cAction > 1, owner, addFilter, hintPanel)))
            {
                if (cAction > 1)
                    drop.DropDownItems.Add(item);
                else
                    m.Items.Add(item);
            }
            if (cAction > 1)
                m.Items.Add(drop);

            countAction = actions.Count(a => string.CompareOrdinal(a.ActionType, OperationByActions.DefaultActionType) != 0);
            if (countAction > 0)
                m.Items.Add(new ToolStripSeparator());

            drop = new ToolStripMenuItem(string.Format("{0}", isGroup ? "Операции над отмеченными в группе" : "Операции над текущим документом"));
            foreach (var item in actions.Where(a => string.CompareOrdinal(a.ActionType, OperationByActions.DefaultActionType) != 0).Select(r => CreateMenuItem(r, gridView, rowHandle, isGroup, countAction > 1, owner, addFilter, hintPanel)))
            {
                if (countAction > 1)
                    drop.DropDownItems.Add(item);
                else
                    m.Items.Add(item);
            }
            if (countAction > 1)
                m.Items.Add(drop);

            return m;
        }

        /// <summary> Створення конкретної дії запису або групи</summary>
        /// <param name="r">Операція</param>
        /// <param name="gridView">Грід</param>
        /// <param name="rowHandle">Запис</param>
        /// <param name="addFilter">Те що додати до фвльтерсету операції</param>
        /// <param name="isGroup">Чи группова</param>
        /// <param name="isDropDown">Чи пункт попав у випадаюче</param>
        /// <param name="owner">Передати кудись далі</param>
        /// <param name="hintPanel">Хинт панель для сообщений</param>
        /// <returns>Оновлене меню</returns>
        private static ToolStripMenuItem CreateMenuItem(OperationByActions r, GridView gridView, int rowHandle, bool isGroup, bool isDropDown, Control owner, IEnumerable<KeyValuePair<string, string>> addFilter, ReportFormHintPanel hintPanel = null)
        {
            foreach (var item in addFilter)
                r.AddFilterSet.Add(item.Key, item.Value);

            var captionTemplate = string.Format("{0}", r.ActionName);
            if (!isDropDown)
                captionTemplate = string.Format(isGroup ? "{0} : по отмеченным в группе" : "{0}", r.ActionName);

            var menuPoint = new ToolStripMenuItem(captionTemplate, r.ActionImage, (s, ev) =>
            {
                if (isGroup)
                {
                    SendMarkedItems(gridView, rowHandle, (OperationByActions)((ToolStripItem)s).Tag, owner, hintPanel);
                }
                else
                {
                    if (r.ModeGroupOperation.In(0, 1) || !string.IsNullOrWhiteSpace(r.AdditionalInfo))
                        SendOneItem(gridView.GetDataRow(rowHandle), (OperationByActions)((ToolStripItem)s).Tag, owner, hintPanel);
                    else
                        SendOneItemNoQuestion(gridView.GetDataRow(rowHandle), (OperationByActions)((ToolStripItem)s).Tag, owner, hintPanel);
                }
            })
            {
                Tag = r
            };
            return menuPoint;
        }

        #endregion

        #region Суми

        /// <summary> Процентна ставка по групі </summary>
        /// <param name="taxGroup">Група</param>
        /// <param name="taxType">Входящий/Исходящий (0/1)</param>
        /// <returns>Ставка або 0 якщо групи немає</returns>
        public static decimal TaxRate(object taxGroup, string taxType = "0")
        {
            var gr = string.Format("{0}", taxGroup).Trim();
            return SaHelper.TaxRateGroups.TaxPercent(gr, taxType);
        }

        /// <summary> Пыдготувати до конвертації у decimal </summary>
        /// <param name="sum">Строка</param>
        /// <returns>Підготовлена строка</returns>
        public static string PrepareToDecimal(string sum)
        {
            if (string.IsNullOrWhiteSpace(sum))
                return sum;
            var sb = new StringBuilder(string.Empty);
            sum = sum.Trim();
            var isSeparatorExists = false;
            for (var i = 0; i < sum.Length; i++)
            {
                var c = sum[i];
                if (i == 0 && c == '-')
                {
                    sb.Append(c);
                    continue;
                }
                if (char.IsDigit(c))
                {
                    sb.Append(c);
                    continue;
                }
                if (isSeparatorExists)
                    break;

                // Тільки один роздільник
                sb.Append(RegexHelper.SystemDecimalSeparator);
                isSeparatorExists = true;
            }
            return sb.ToString();
        }

        /// <summary> Встановити суму </summary>
        /// <param name="sum">Строка</param>
        /// <param name="digits">Після коми</param>
        /// <returns>Сумма</returns>
        public static decimal Sum(string sum, int digits = -1)
        {
            decimal s;
            if (!decimal.TryParse(PrepareToDecimal(sum), out s))
                s = new decimal(0.0);
            return Math.Round(s, digits < 0 ? SumPoint : digits);
        }

        /// <summary> Розрахунок курс по двом валютам </summary>
        /// <param name="sumCur">Валютна сума</param>
        /// <param name="sumBase">Сума в базовій валюті</param>
        /// <param name="digits">Після коми</param>
        /// <returns>Курс</returns>
        public static decimal CalculateRate(string sumCur, string sumBase, int digits = -1)
        {
            decimal sCur;
            if (!decimal.TryParse(PrepareToDecimal(sumCur), out sCur))
                sCur = new decimal(0.0);

            decimal sBase;
            if (!decimal.TryParse(PrepareToDecimal(sumBase), out sBase))
                sBase = new decimal(0.0);

            return sCur == 0 ? new decimal(0.0) : Math.Round(sBase / sCur, digits < 0 ? RatePoint : digits);
        }

        /// <summary> Розрахунок базової суми  </summary>
        /// <param name="sumCur">Сума у валюті</param>
        /// <param name="rate">Курс</param>
        /// <param name="digits">Після коми</param>
        /// <returns>Сума у базовій валюті</returns>
        public static decimal CalculateSumBase(string sumCur, string rate, int digits = -1)
        {
            decimal sCur;
            if (!decimal.TryParse(PrepareToDecimal(sumCur), out sCur))
                sCur = new decimal(0.0);

            decimal sRate;
            if (!decimal.TryParse(PrepareToDecimal(rate), out sRate))
                sRate = new decimal(0.0);

            return Math.Round(sCur * sRate, digits < 0 ? SumPoint : digits);
        }

        /// <summary> Розрахунок проценту НДС  </summary>
        /// <param name="sumWithNds">Сумма з НДС</param>
        /// <param name="sumWithoutNds">Сумма без НДС</param>
        /// <param name="digits">Після коми</param>
        /// <returns>Ставка НДС</returns>
        public static decimal CalculatePercent(decimal sumWithNds, decimal sumWithoutNds, int digits = -1)
        {
            return sumWithoutNds == 0 ? new decimal(0.0) : Math.Round(((100 * sumWithNds) / sumWithoutNds) - 100, digits < 0 ? SumPoint : digits);
        }

        /// <summary> Розрахунок суми НДС від суми </summary>
        /// <param name="sum">Сумма</param>
        /// <param name="percent">Процент</param>
        /// <param name="digits">Після коми</param>
        /// <returns>Сумма НДС</returns>
        public static decimal CalculateSumNds(decimal sum, decimal percent, int digits = -1)
        {
            return Math.Round((sum * percent) / 100, digits < 0 ? SumPoint : digits);
        }

        /// <summary> Розрахунок суми без НДС від суми з НДС</summary>
        /// <param name="sumWithNds">Сумма з НДС</param>
        /// <param name="percent">Процент НДС</param>
        /// <param name="digits">Після коми</param>
        /// <returns>Сумма без НДС</returns>
        public static decimal CalculateSumWithoutNds(decimal sumWithNds, decimal percent, int digits = -1)
        {
            var divider = (100 + percent);
            if (divider == 0)
                return new decimal(0.0);

            var sumNds = (sumWithNds * percent) / divider;
            return Math.Round((sumWithNds - sumNds), digits < 0 ? SumPoint : digits);
        }

        /// <summary> Розрахунок суми з НДС по сумі без НДС</summary>
        /// <param name="sumWithoutNds">Сумма без НДС</param>
        /// <param name="percent">Процент НДС</param>
        /// <param name="digits">Після коми</param>
        /// <returns>Сумма з НДС</returns>
        public static decimal CalculateSumWithNds(decimal sumWithoutNds, decimal percent, int digits = -1)
        {
            return Math.Round(sumWithoutNds + ((sumWithoutNds / 100) * percent), digits < 0 ? SumPoint : digits);
        }

        /// <summary> Розрахунок суми НДС по сумі з НДС</summary>
        /// <param name="sumWithNds">Сумма з НДС</param>
        /// <param name="percent">Процент НДС</param>
        /// <param name="digits">Після коми</param>
        /// <returns>Сумма НДС</returns>
        public static decimal CalculateNdsFromSumWithNds(decimal sumWithNds, decimal percent, int digits = -1)
        {
            var divider = (100 + percent);
            return divider == 0 ? new decimal(0.0) : Math.Round((sumWithNds * percent) / divider, digits < 0 ? SumPoint : digits);
        }

        /// <summary> Отримати курс за встановлену дату </summary>
        /// <param name="baseCurrency">Базова валюта</param>
        /// <param name="currencyFrom">Валюта</param>
        /// <param name="currencyRateDate">Дата курса</param>
        /// <returns>Курс</returns>
        public static decimal CurrentRate(string baseCurrency, string currencyFrom, DateTime currencyRateDate)
        {
            // Валюти співпали
            if (string.Compare(currencyFrom, baseCurrency, StringComparison.OrdinalIgnoreCase) == 0)
                return new decimal(1.0);

            try
            {
                var row = SaHelper.HashRates[currencyRateDate].Rows.Cast<DataRow>()
                    .FirstOrDefault(r => string.Format("{0}", r["currencyBase"]).In(currencyFrom) &&
                                         string.Format("{0}", r["currencyTarget"]).In(baseCurrency) &&
                                         Sum(string.Format("{0}", r["exchangeValue"])) > 0) ??

                          SaHelper.HashRates[currencyRateDate].Rows.Cast<DataRow>()
                              .FirstOrDefault(r => string.Format("{0}", r["currencyBase"]).In(currencyFrom, baseCurrency) &&
                                                   string.Format("{0}", r["currencyTarget"]).In(currencyFrom, baseCurrency) &&
                                                   Sum(string.Format("{0}", r["exchangeValue"])) > 0);
                if (row != null)
                    return Math.Round(Convert.ToDecimal(row["exchangeValue"]) * Convert.ToInt32(row["exchangeNominalTarget"]) / Convert.ToInt32(row["exchangeNominalBase"]), RatePoint);
            }
            catch (Exception)
            {
                return new decimal(0.0);
            }
            return new decimal(0.0);
        }

        /// <summary> Дата курсу валют </summary>
        public static KeyValuePair<string, DateTime> CurrencyRate(object date, DateTime defaultDate, Control owner = null)
        {
            var d = defaultDate.Date;

            var value = date as DateTime?;
            if (value.HasValue)
                d = value.Value.Date;

            if (SaHelper.HashRates.ContainsKey(d.Date))
                return new KeyValuePair<string, DateTime>(string.Empty, d.Date);

            var filter = new FilterSet();
            filter["rateDate"] = new FilterSetItem("rateDate", FilterType.Static, d.Date);

            IDictionary<string, DataTable> rate;
            var message = string.Empty;
            try
            {
                rate = UEditDataReader.GetData(filter, "SA.GetExchangeRate", owner);
            }
            catch (Exception ex)
            {
                message = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                rate = new Dictionary<string, DataTable>();
            }
            if (!rate.ContainsKey("Data"))
                return new KeyValuePair<string, DateTime>(string.Format("Ошибка получения курса валют за {0}{1}", d.Date, message), d.Date);

            SaHelper.HashRates.Add(d.Date, rate["Data"]);
            return new KeyValuePair<string, DateTime>(string.Empty, d.Date);
        }

        #endregion

        /// <summary> Встановити колонкам дозвіл редагування та ридонли </summary>
        /// <param name="gridView">Грід</param>
        /// <param name="allowEdit">Редагування</param>
        /// <param name="readOnly">Тільки для читання</param>
        /// <param name="excludedNames">Колонки виключення</param>
        public static void SetColumnAllowEdit(GridView gridView, bool allowEdit, bool readOnly, ICollection<string> excludedNames = null)
        {
            if (excludedNames == null)
                excludedNames = new Collection<string>();

            gridView.Columns.Cast<GridColumn>().Where(c => !excludedNames.Contains(c.FieldName)).ForEach(c =>
            {
                c.OptionsColumn.AllowEdit = allowEdit;
                c.OptionsColumn.ReadOnly = readOnly;
            });
        }

        /// <summary> Значення колонки </summary>
        /// <param name="row">Запис</param>
        /// <param name="columnName">Колонка</param>
        /// <param name="value">Нове значення</param>
        /// <param name="editor">Редактор</param>
        public static void CellValueFixed(DataRow row, string columnName, object value, RepositoryItem editor)
        {
            var filterSet = GetStandartFilterSet(row);
            row[columnName] = value;
            filterSet[columnName] = new FilterSetItem(columnName, FilterType.Static, string.Format("{0}", value));
            UEditDataReader.GetData(filterSet, "SA.Document@QuickSave");

            RefreshHistory(columnName, value, editor);
        }

        /// <summary> Оновити історію </summary>
        /// <param name="columnName">Колонка</param>
        /// <param name="value">Нове значення</param>
        /// <param name="editor">Редактор</param>
        public static void RefreshHistory(string columnName, object value, RepositoryItem editor)
        {
            if (!columnName.In(Note))
                return;
            SaHelper.NotesHistory = UpdateHistory(string.Format("{0}", value), SaHelper.NotesHistory);
            ((RepositoryItemComboBox)(editor)).Items.Clear();
            ((RepositoryItemComboBox)(editor)).Items.AddRange(SaHelper.NotesHistory.ToList());
        }

        /// <summary> Оновлення історії </summary>
        /// <param name="value">Значення</param>
        /// <param name="list">Список</param>
        /// <returns>Оновленний список</returns>
        public static IList<string> UpdateHistory(string value, IList<string> list)
        {
            if (string.IsNullOrWhiteSpace(value))
                return list;
            value = value.Trim();

            if (list.Contains(value))
                return list;

            var newValues = new string[MaxQueryHistory];
            newValues[0] = value;
            for (var i = 0; i < Math.Min(list.Count, MaxQueryHistory - 1); i++)
                newValues[i + 1] = list[i];
            return newValues.Where(v => !string.IsNullOrWhiteSpace(v)).ToList();
        }

        /// <summary> Натиснули прогалик </summary>
        public static void HistoryKeyDown(object sender, KeyEventArgs e)
        {
            var history = sender as ComboBoxEdit;
            if (history == null)
                return;
            if (history.SelectionStart != 0 || e.KeyCode != Keys.Space)
                return;
            history.SelectionStart = 0;
            history.ShowPopup();
        }

        /// <summary> Встановити довідник </summary>
        /// <param name="control">Контрол</param>
        /// <param name="source">Соурс</param>
        /// <param name="sourceName">Ім'я</param>
        public static void SetControlSource<T>(IHeaderControl control, IEnumerable<T> source, string sourceName = "Data")
        {
            DataTable dt;
            var type = typeof(T);
            if (type.IsPrimitive || type == typeof(string))
                dt = source.Select(s => new
                {
                    Key = s
                }).ToDataTable();
            else
                dt = source.ToDataTable();

            dt.TableName = sourceName;
            control.ToSource(dt);
        }
    }
}
