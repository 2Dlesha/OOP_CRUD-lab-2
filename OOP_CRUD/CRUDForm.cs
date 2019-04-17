using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace OOP_CRUD
{
    public partial class CRUDForm : Form
    {
        public ICRUDHelper CRUDAssistant = null;
        public Form editForm = null;
        public List<object> itemList = new List<object>();
        public List<object> activeItemList = new List<object>();
        public List<Type> itemCreator = new List<Type>();

        public CRUDForm(List<Type> availibleTypes, ICRUDHelper CRUDHelper)
        {
            InitializeComponent();
            CRUDAssistant = CRUDHelper;
            CRUDAssistant.ItemsInit(itemList);
            activeItemList = itemList;
            itemCreator = availibleTypes;
        }

        private void CRUDForm_Load(object sender, EventArgs e)
        {
            foreach (var item in itemCreator)
            {
                string typeString = item.Name; 

                if (item.GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault() is DisplayNameAttribute displayNameAttribute)
                    typeString = displayNameAttribute.DisplayName;

                comboBoxTypes.Items.Add(typeString);
            }

            comboBoxTypes.SelectedIndex = 0;
            comboBoxTypes.DropDownStyle = ComboBoxStyle.DropDownList;
            itemsListView.View = View.Details;

            CRUDAssistant.ListRedraw(itemsListView, activeItemList);
        }


        private void CreateItemEditForm(Object item, List<Object> itemList)
        {
            if (item != null)
            {
                editForm = CRUDAssistant.CreateForm(item, itemList);
                editForm.ShowDialog();
                editForm.Dispose();
            }
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            ConstructorInfo itemConstructor = itemCreator[comboBoxTypes.SelectedIndex].GetConstructor(new Type[] { });
            object newitem = itemConstructor.Invoke(new object[] { });
            itemList.Add(newitem);

            activeItemList = GetActiveList(itemList, checkBoxFilter.Checked, itemCreator[comboBoxTypes.SelectedIndex]);

            CreateItemEditForm(newitem, itemList);

            CRUDAssistant.ListRedraw(itemsListView, activeItemList);
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            object item = GetFocusItem();
            if (item != null)
            {
                CRUDAssistant.DeleteItem(item, itemList);
                activeItemList = GetActiveList(itemList, checkBoxFilter.Checked, itemCreator[comboBoxTypes.SelectedIndex]);
                CRUDAssistant.ListRedraw(itemsListView, activeItemList);
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            object item = GetFocusItem();
            if (item != null)
            {
                CreateItemEditForm(item, itemList);
                CRUDAssistant.ListRedraw(itemsListView, activeItemList);
            }
        }

        public object GetFocusItem()
        {
            object item = null;
            activeItemList = GetActiveList(itemList, checkBoxFilter.Checked, itemCreator[comboBoxTypes.SelectedIndex]);

            if (itemsListView.SelectedIndices.Count != 0)
                item = activeItemList[itemsListView.SelectedIndices[0]];

            return item;
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            activeItemList = GetActiveList(itemList, checkBoxFilter.Checked, itemCreator[comboBoxTypes.SelectedIndex]);
            CRUDAssistant.ListRedraw(itemsListView, activeItemList);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            activeItemList = GetActiveList(itemList, checkBoxFilter.Checked, itemCreator[comboBoxTypes.SelectedIndex]);
            CRUDAssistant.ListRedraw(itemsListView, activeItemList);
        }

        public  List<object> GetActiveList(List<object> items, bool specificClass, Type classType)
        {
            return (specificClass && (classType != null)) 
                ?  items.Where(item => (item.GetType() == classType)).ToList() 
                :  items;
        }
    }
}
