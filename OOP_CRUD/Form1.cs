﻿using System;
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
    public partial class Form1 : Form
    {
        public Form frm;
        public List<object> itemList = new List<object>();
        public List<object> activeItemList = new List<object>();

        public List<Type> itemCreator = new List<Type>() {
                typeof(AutomaticRifle),
                typeof(Crossbow),
                typeof(Bow),
                typeof(BladedWeapon),
                typeof(Gunsight),
                typeof(Bullet),
                typeof(Arrow),
        };

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CRUDHelper.ItemsInit(itemList);
            activeItemList = itemList;

            foreach (var item in itemCreator)
            {
                string typeString = "";
                if (item.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() is DescriptionAttribute descriptionAttribute)
                    typeString = descriptionAttribute.Description;
                else
                    typeString = item.Name;

                comboBox1.Items.Add(typeString);
            }

            comboBox1.SelectedIndex = 0;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

            CRUDHelper.ListRedraw(listView1, activeItemList, "name");
            listView1.View = View.Details;
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            ListView listview = listView1;

            ConstructorInfo itemConstructor = itemCreator[comboBox1.SelectedIndex].GetConstructor(new Type[] { });
            object newitem = itemConstructor.Invoke(new object[] { });
            itemList.Add(newitem);

            activeItemList = GetActiveList(itemList, checkBox1.Checked, itemCreator[comboBox1.SelectedIndex]);
            CRUDHelper.ListRedraw(listView1, activeItemList, "name");

            ListViewItem lvi = listview.Items[(activeItemList.Count - 1)];
            if (lvi != null)
            {
                listview.Focus();
                lvi.Selected = true;
                buttonEdit.PerformClick();
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            ListView listview = GetActiveList();
            if (listview == null)
                return;

            object item = GetFocusItem();
            if (item != null)
            {
                CRUDHelper.DeleteItem(item, itemList);
                activeItemList = GetActiveList(itemList, checkBox1.Checked, itemCreator[comboBox1.SelectedIndex]);
                CRUDHelper.ListRedraw(listView1, activeItemList, "name");
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            ListView listview = GetActiveList();
            if (listview == null)
                return ;

            object item = GetFocusItem();
            if (item != null)
            {
                FieldInfo[] fields = item.GetType().GetFields(); 

                frm = CRUDHelper.CreateForm(item, itemList,SaveControls);
                frm.ShowDialog();
                frm.Dispose();

                CRUDHelper.ListRedraw(listView1, activeItemList, "name");
            }
        }

        //сохранение значений контролов (event)
        public void SaveControls(object sender, EventArgs e)
        {
            object item = GetFocusItem();
            if (item == null)
            {
                return;
            }
            CRUDHelper.SaveControlsToItems(item, itemList, frm);     
        }

        public object GetFocusItem()
        {
            object item = null;
            activeItemList = GetActiveList(itemList, checkBox1.Checked, itemCreator[comboBox1.SelectedIndex]);

            if ((listView1!=null) &&(listView1.SelectedIndices.Count != 0))
            {
                item = activeItemList[listView1.SelectedIndices[0]];
            }

            return item;
        }

        public ListView GetActiveList()
        {
            foreach (ListView listview in this.Controls.OfType<ListView>())
            {
                if (listview.SelectedIndices.Count != 0)
                {
                    return listview;
                }
            }
            return null;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            activeItemList = GetActiveList(itemList, checkBox1.Checked, itemCreator[comboBox1.SelectedIndex]);
            CRUDHelper.ListRedraw(listView1, activeItemList, "name");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            activeItemList = GetActiveList(itemList, checkBox1.Checked, itemCreator[comboBox1.SelectedIndex]);
            CRUDHelper.ListRedraw(listView1, activeItemList, "name");
        }

        public  List<object> GetActiveList(List<object> items, bool specificClass, Type classType)
        {
            if ( specificClass && (classType != null) )
            {
                return items.Where(item => (item.GetType() == classType)).ToList();
            }
            else
            {
                return items;
            }
        }
    }
}
