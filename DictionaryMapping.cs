using System.Collections;

namespace SA.Classes
{
    /// <summary> Привязка справочников к некоторым полям </summary>
    public class DictionaryMapping
    {
        /// <summary> Справочник </summary>
        public ICollection Dictionary{ get; private set; }
        /// <summary> Ключ </summary>
        public string ValueMember{ get; private set; }
        /// <summary> Значение </summary>
        public string DisplayMember{ get; private set; }

        /// <summary> Конструтор </summary>
        /// <param name="dictionary">Справочник</param>
        /// <param name="valueMemeber">Ключ</param>
        /// <param name="displayMemeber">Значение</param>
        public DictionaryMapping(ICollection dictionary,string valueMemeber="Key",string displayMemeber="Value")
        {
            Dictionary = dictionary;
            ValueMember = valueMemeber;
            DisplayMember = displayMemeber;
        }
    }
}
