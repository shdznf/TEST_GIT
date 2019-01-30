using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Utils.Helpful;

namespace SA.Classes
{
    /// <summary> Розмальовка jufhdkj редактора </summary>
    public partial class EditDocumentCfg : UserControl, IAdditionalInfo
    {
        /// <summary> Розмальовка </summary>
        public IDictionary<EditorProperties, Color> EditFormColors = new Dictionary<EditorProperties, Color>
        {
            {EditorProperties.Empty,SaHelper.EditFormColors[EditorProperties.Empty]},
            {EditorProperties.EditHeaderColumnColor, SaHelper.EditFormColors[EditorProperties.EditHeaderColumnColor]},
            {EditorProperties.EditColumnColor, SaHelper.EditFormColors[EditorProperties.EditColumnColor]},
            {EditorProperties.EditHeaderMandatoryColumnColor, SaHelper.EditFormColors[EditorProperties.EditHeaderMandatoryColumnColor]}
        };

        /// <summary> Відображення форми редактора </summary>
        public int ShowEditForm
        {
            get
            {
                var v = lookUpEdit1.EditValue.IsNull(0);
                return _showEditForm.ContainsKey(v) ? v : 0;
            }
        }

        /// <summary> Відображення   eff  форм редагування </summary>
        private readonly IDictionary<int,string> _showEditForm=new Dictionary<int, string>
        {
            {0,"Отображать без блокировки других форм"},
            {1,"Отображать внутри формы реестра"},
            {2,"Блокировать все остальные формы"},
        };


        /// <summary> Конструкторр </summary>
        public EditDocumentCfg()
        {
            InitializeComponent();
            colorEdit1.EditValue = EditFormColors[EditorProperties.EditHeaderColumnColor];
            colorEdit2.EditValue = EditFormColors[EditorProperties.EditColumnColor];
            colorEdit3.EditValue = EditFormColors[EditorProperties.EditHeaderMandatoryColumnColor];
            lookUpEdit1.Properties.DataSource = _showEditForm;
            lookUpEdit1.EditValue = SaHelper.EditFormProperties[EditorProperties.UseModalForm];
        }

        /// <summary> Фільтер сет який треба додати до передачі у операцію</summary>
        public IDictionary<string, string> AddFilterSet
        {
            get { return new Dictionary<string, string>(); }
        }

        /// <summary> Чи введені дані валідні </summary>
        public bool IsValid
        {
            get { return true; }
        }

        /// <summary> Чи треба модальне вікно</summary>
        public bool IsModal
        {
            get { return false; }
        }

        /// <summary> Додаткова інформація </summary>
        public string Info
        {
            get { return "Установите цвета для форм редактирования документов"; }
        }

        /// <summary> Контрол який додати </summary>
        public Control AdditionControl
        {
            get { return this; }
        }

        /// <summary> Викликається після конструктору для ініціалізації чогось </summary>
        /// <param name="informationForControl">Додаткова інформація</param>
        public IAdditionalInfo InitControlInfo(IDictionary<string, object> informationForControl = null)
        {
            return this;
        }

        /// <summary> Зміна значення </summary>
        private void ColorEditEditValueChanged(object sender, EventArgs e)
        {
            var colorEdit = sender as ColorEdit;
            if(colorEdit == null)
                return;
            if(colorEdit == colorEdit1)
                EditFormColors[EditorProperties.EditHeaderColumnColor] = (Color)colorEdit.EditValue;

            if(colorEdit == colorEdit2)
                EditFormColors[EditorProperties.EditColumnColor] = (Color)colorEdit.EditValue;

            if (colorEdit == colorEdit3)
                EditFormColors[EditorProperties.EditHeaderMandatoryColumnColor] = (Color)colorEdit.EditValue;
        }
    }
}
