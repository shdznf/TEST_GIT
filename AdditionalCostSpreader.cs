using System;
using System.Collections.Generic;
using System.Data;

namespace SA.Classes
{
    /// <summary> Розподільник </summary>
    public class AdditionalCostSpreader
    {
        /// <summary> Заголовок </summary>
        public string Caption{ get; private set; }
        /// <summary> Імя колонки </summary>
        public string ColumnName{ get; private set; }
        /// <summary> Використати  всі записи або тільки відмічені</summary>
        public bool UseAllRows {get;private set;}
        /// <summary> Що виконувати </summary>
        public Action<IDictionary<DataRow, bool>, decimal, decimal, string> Spreader{ get; private set; }

        /// <summary> Конструктор </summary>
        /// <param name="caption">Заголовок</param>
        /// <param name="columnName">Імя колонки</param>
        /// <param name="spreader">Що виконувати</param>
        /// <param name="useAllRows">Використати всі записи або відмічені(за-замовчуванням тільки відмічені)</param>
        public AdditionalCostSpreader(string caption, string columnName, Action<IDictionary<DataRow,bool>, decimal, decimal, string> spreader, bool useAllRows = false)
        {
            Caption = caption;
            ColumnName = columnName;
            Spreader = spreader;
            UseAllRows = useAllRows;
        }

        /// <summary> Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>. </summary>
        /// <returns> A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>. </returns>
        public override string ToString()
        {
            return Caption.IsNull(ColumnName);
        }
    }
}
