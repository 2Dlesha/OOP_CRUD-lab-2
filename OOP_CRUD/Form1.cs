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
    public partial class Form1 : Form
    {
        public Form frm;

        public List<Weapon> weaponList = new List<Weapon>() {
           // new AutomaticRifle(),
           // new Bow()
        };

        public List<WeaponCreator> weaponCreatorList = new List<WeaponCreator>() {
            new AutomaticRifleCreator(),
            new BowCreator(),
            new CrossBowCreator(),
            new BladeCreator(),
        };

        public List<AmmunitionCreator> ammunitionCreatorList = new List<AmmunitionCreator>() {
            new BulletCreator(),
            new ArrowCreator()
        };

        public List<GunsightCreator> gunsightCreatorList = new List<GunsightCreator>() {
            new GunsightCreator()
        };



        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listView1.MultiSelect = false;        
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            weaponList.Add(weaponCreatorList[ random.Next(0, weaponCreatorList.Count-1)].Create());
            ListRedraw(listView1, weaponList);
        }

        public void ListRedraw(ListView listView, List<Weapon> listOfWeapon)
        {
            listView.Clear();
            for (int i = 0; i < listOfWeapon.Count; i++)
            {
                var listItem = new ListViewItem();
                Type itemType = listOfWeapon[i].GetType();
                listItem.Text = itemType.Name;
                listView.Items.Add(listItem);
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count != 0)
            {
                int itemNumber = listView1.SelectedIndices[0];
                if (itemNumber <weaponList.Count)
                    weaponList.Remove(weaponList[itemNumber]);
            }
            ListRedraw(listView1, weaponList);
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            int itemNumber = -1;
            if (listView1.SelectedIndices.Count != 0)
                 itemNumber = listView1.SelectedIndices[0];
            else
                return;

            //список всех полей объекта
            FieldInfo[] fields = weaponList[itemNumber].GetType().GetFields();

            //создание формы для редактирования полей
            frm = new Form
            {
                Size = new System.Drawing.Size(300, 30 * fields.Length),
                Text = "Form"
            };
             
            string fld = "";
  
            for (int i = 0 ; i < fields.Length ; i++)
            {
                fld += fields[i].FieldType.Name + " " + fields[i].Name + "\n";

                Label label = new Label
                {
                    Location = new Point(15, 25 * (i + 1)),
                    Width = string.Concat(fields[i].FieldType.Name, " ", fields[i].Name).Length * 6,
                    Text = string.Concat(fields[i].FieldType.Name ," " , fields[i].Name)
                };
                frm.Controls.Add(label);

                //Создание для типов значений текстовых полей ввода
                if (((fields[i].FieldType.IsPrimitive)&&(!fields[i].FieldType.IsEnum))||(fields[i].FieldType.Name == "String"))
                {
                    TextBox text = new TextBox
                    {
                        Name = fields[i].Name,
                        Location = new Point(15 + label.Width, 25 * (i + 1)),
                        Width = frm.Width - ( label.Location.X + label.Width + 30)
                    };

                    text.Text = fields[i].GetValue(weaponList[itemNumber]).ToString();
                    frm.Controls.Add(text);
                }


                //Создание выпадающих списков для перечислимых типов
                if (fields[i].FieldType.IsEnum)
                {
                    ComboBox combobox = new ComboBox
                    {
                        Name = fields[i].Name,
                        SelectionStart = 0,
                        DropDownStyle = ComboBoxStyle.DropDownList,
                        Location = new Point(15 + label.Width, 25 * (i + 1)),
                        Width = frm.Width - (label.Location.X + label.Width + 30)
                    };
                    combobox.Items.AddRange(fields[i].FieldType.GetEnumNames());
                    combobox.SelectedIndex = (int)(fields[i].GetValue(weaponList[itemNumber]));
                    frm.Controls.Add(combobox);
                }

                //Создание выпадающих списков для перечислимых типов
                if ((!fields[i].FieldType.IsPrimitive)&&(!fields[i].FieldType.IsEnum)&&(!(fields[i].FieldType.Name == "String")))
                {
                    ComboBox combobox = new ComboBox
                    {
                        Name = fields[i].Name,
                        SelectionStart = 0,
                        DropDownStyle = ComboBoxStyle.DropDownList,
                        Location = new Point(15 + label.Width, 25 * (i + 1)),
                        Width = frm.Width - (label.Location.X + label.Width + 30)
                    };
                    frm.Controls.Add(combobox);
                }


            }

            frm.FormClosing += SaveControlsToItems;
            frm.ShowDialog();
            frm.Dispose();
        }


        //сохранение значений контролов в объект (событие)
        public void SaveControlsToItems(object sender, EventArgs e)
        {
            int itemNumber = -1;
            if (listView1.SelectedIndices.Count != 0)
                itemNumber = listView1.SelectedIndices[0];
            else
                return;

            FieldInfo[] fields = weaponList[itemNumber].GetType().GetFields();

            string tst = "";
            foreach (var control in frm.Controls.OfType<TextBox>().Cast<Control>().ToList())  
            {
                if (fields.ToList().Where(field => field.Name == control.Name).Count() != 0)
                {
                    FieldInfo fi = fields.ToList().Where(field => field.Name == control.Name).First();
                    var buf = fi.GetValue(weaponList[itemNumber]);

                    try
                    {
                        fi.SetValue(weaponList[itemNumber], Convert.ChangeType(control.Text, fi.FieldType));
                    }
                    catch
                    {
                        fi.SetValue(weaponList[itemNumber], buf);
                        MessageBox.Show("Incorrect field value");
                    }
                }
            }


            foreach (var c in frm.Controls.OfType<ComboBox>().Cast<Control>().ToList())
            {

            }

        }

    }
}
