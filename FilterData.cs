using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using FozzySystems.Types;
using Utils.Assist;

namespace SA.Classes
{
    /// <summary> Основні дані для маніпуляцій </summary>
    public class FilterData
    {
        /// <summary> Імя колонки </summary>
        public string Name{ get; set; }
        /// <summary> Імя у фільтесеті </summary>
        public string FilterName{get;set;}
        /// <summary> Значення </summary>
        public object Value{ get; set; }
        /// <summary> Конструктор </summary>
        /// <param name="name">Імя колонки</param>
        /// <param name="filterName">Імя у фільтер сеті</param>
        /// <param name="value">Значення</param>
        public FilterData(string name, string filterName, object value)
        {
            Name = name;
            FilterName = filterName;
            Value = value;
        }
        /// <summary> Копія </summary>
        /// <returns>Копія</returns>
        public FilterData Copy()
        {
            return new FilterData(Name,FilterName,Value);
        }

        /// <summary> Serves as a hash function for a particular type.  </summary>
        /// <returns> A hash code for the current <see cref="T:System.Object"/>. </returns>
        public override int GetHashCode()
        {
            return string.Format("{0}:{1}", Name, Value).GetHashCode();
        }
    }

    /// <summary> Колекція основних даних для маніпуляцій </summary>
    public class FilterDataCollection : Collection<FilterData>
    {
        /// <summary> Індексер для отримання-встановлення значення</summary>
        /// <param name="name">Імя</param>
        /// <returns>Значення</returns>
        public object this[string name]
        {
            get
            {
                var item=this.FirstOrDefault(i => string.Compare(name, i.Name, StringComparison.Ordinal) == 0);
                return item == null ? null : item.Value;
            }
            set
            {
                var item = this.FirstOrDefault(i => string.Compare(name, i.Name, StringComparison.Ordinal) == 0);
                if(item == null)
                    return;
                item.Value = value;
            }
        }

        /// <summary> Зробити фільтер сет </summary>
        /// <returns>Фільтер сет</returns>
        public FilterSet ToFilterSet()
        {
            var filter = new FilterSet();
            var prevName = string.Empty;
            var value = new StringBuilder(string.Empty);

            foreach(var item in this.Where(i=>!string.IsNullOrWhiteSpace(i.FilterName)&&!i.Value.IsNull()).OrderBy(i=>i.FilterName).ThenBy(i=>i.Name))
            {
                // Нове імя
                if(string.Compare(prevName, item.FilterName, StringComparison.Ordinal) != 0)
                {
                    if(!string.IsNullOrWhiteSpace(prevName))
                        filter.SetStaticFilter(prevName, value.ToString());

                    value = new StringBuilder(string.Empty);
                    value.AppendFormat("{0}",
                        !item.Value.IsNull() && item.Value.GetType().In(typeof(DateTime))
                            ? ((DateTime)item.Value).ToString("D", MicroSerializer.SerializationCulture)
                            : !item.Value.IsNull() && item.Value.GetType().In(typeof(double), typeof(float), typeof(decimal))
                                ? ((decimal)item.Value).ToString(MicroSerializer.SerializationCulture)
                                : item.Value);
                }
                else
                {
                    value.AppendFormat(",{0}",
                        !item.Value.IsNull() && item.Value.GetType().In(typeof(DateTime))
                            ? ((DateTime)item.Value).ToString("D", MicroSerializer.SerializationCulture)
                            : !item.Value.IsNull() && item.Value.GetType().In(typeof(double), typeof(float), typeof(decimal))
                                ? ((decimal)item.Value).ToString(MicroSerializer.SerializationCulture)
                                : item.Value);
                }
                prevName = item.FilterName;
            }
            if (!string.IsNullOrWhiteSpace(prevName))
                filter.SetStaticFilter(prevName, value.ToString());

            return filter;
        }
        /// <summary> Копія </summary>
        /// <returns>Копія</returns>
        public FilterDataCollection Copy()
        {
            var tbc = new FilterDataCollection();
            this.ForEach(i=>tbc.Add(i.Copy()));
            return tbc;
        }

        /// <summary> Serves as a hash function for a particular type.  </summary>
        /// <returns>A hash code for the current <see cref="T:System.Object"/>. </returns>
        public override int GetHashCode()
        {
            var res = new decimal(0.0);
            this.ForEach(f =>{res+=f.GetHashCode();});
            return res.GetHashCode();
        }

        /// <summary> Те саме, що GetHashCode тільки не по всім даним </summary>
        /// <param name="included">Колонки які включати</param>
        /// <returns>Код</returns>
        public int GetHashCode(ICollection<string> included)
        {
            if(included == null)
                return 0;
            var res = new decimal(0.0);
            this.Where(f => included.Contains(f.Name)).ForEach(f =>
            {
                res += f.GetHashCode();
            });
            return res.GetHashCode();
        }

    }
}
