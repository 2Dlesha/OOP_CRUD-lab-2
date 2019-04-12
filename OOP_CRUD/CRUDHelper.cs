using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP_CRUD
{
    class CRUDHelper
    {
        public static void ItemsInit(List<Object> items)
        {
            items.AddRange(new List<object>
            {
                new AutomaticRifle()
                {modelName = "AK- 47",weight = 5,butt = true,catrigeCapacity = 30,caliber = 7.62f , numberOfBarrels = 1, distance = 500 , shotsPerMinute = 120 },
                new Crossbow()
                {modelName = "Арбалет - 90", shotsPerMinute = 3 , distance = 300 , weight = 2.3f ,  mechanismType = MechanismType.Collapsible },
                new Bow()
                {modelName = "Лук - 3000", mechanismType = MechanismType.Folding , weight = 1 , distance = 200 , material = "wood" , arcLenght = 50 , shotsPerMinute = 2 , rechargeTime = 10},
                new BladedWeapon()
                {modelName = "Кинжал", material = "" , weight = 0.5f, bladeLenght = 20 , damageType = DamageType.Cutting, handleLenght = 10, numberOfBlades = 1 , range = 1 },
                new Gunsight()
                { zoom = 4, aimType = AimType.Optic},
                new Bullet()
                { caliber = 7.62f, penetratingAbility = 1, weight = 0.05f },
                new Arrow()
                {name = "Деревянная стрела" , numberOfFeathers = 4, weight = 0.02f , shaftLenght = 80}
            }
            );
        }

        public static void SaveControlsToItems(Object item, List<Object> items, Form form)
        {
            if ((form == null) || (item == null) || (items == null))
                return;

            FieldInfo[] fields = item.GetType().GetFields();
            //из текста в значение
            foreach (var control in form.Controls.OfType<TextBox>().ToList())
            {
                if (fields.ToList().Where(field => field.Name == control.Name).Count() != 0)
                {
                    FieldInfo fi = fields.ToList().Where(field => field.Name == control.Name).First();
                    var buf = fi.GetValue(item);
                    try
                    {
                        fi.SetValue(item, Convert.ChangeType(control.Text, fi.FieldType));
                    }
                    catch
                    {
                        fi.SetValue(item, buf);
                        MessageBox.Show(fi.Name + ": Incorrect field value");
                    }
                }
            }

            //Из выпадающих списков в enum
            foreach (var control in form.Controls.OfType<ComboBox>().ToList())
            {
                if (fields.ToList().Where(field => field.Name == control.Name).Count() != 0)
                {
                    FieldInfo fi = fields.ToList().Where(field => field.Name == control.Name).First();
                    var buf = fi.GetValue(item);

                    if (control.SelectedIndex == -1)
                        continue;

                    if (fi.FieldType.IsEnum)
                    {
                        try
                        {
                            fi.SetValue(item, control.SelectedIndex);
                        }
                        catch
                        {
                            fi.SetValue(item, buf);
                            MessageBox.Show(fi.Name + ": Incorrect field value");
                        }
                    }
                    else
                    {
                        List<object> suitableItems = items.Where(sitem => ((sitem.GetType() == fi.FieldType) || (sitem.GetType().BaseType == fi.FieldType))).ToList();
                        try
                        {
                            fi.SetValue(item, suitableItems[control.SelectedIndex]);
                        }
                        catch
                        {
                            fi.SetValue(item, buf);
                            MessageBox.Show(fi.Name + ": Incorrect field value");
                        }
                    }
                }
            }
        }

        public static Form CreateForm(Object item, List<Object> items, EventHandler saveEvent)
        {
            //список всех полей объекта
            FieldInfo[] fields = item.GetType().GetFields(); 

            //создание формы для редактирования полей
            Form form = new Form
            {
                Text = item.GetType().ToString(),
                Size = new System.Drawing.Size(350, 60 + 25 * (fields.Length + 2))
            };

            for (int i = 0; i < fields.Length; i++)
            {
                //надпись содержащая тип и имя поля
                Label label = new Label
                {
                    Location = new Point(15, 25 * (i + 1)),
                    Width = string.Concat(fields[i].FieldType.Name, " ", fields[i].Name).Length * 7,
                    Text = string.Concat(fields[i].FieldType.Name, " ", fields[i].Name)
                };

                form.Controls.Add(label);

                //Создание для типов значений текстовых полей ввода и их заполнение
                if ( ((fields[i].FieldType.IsPrimitive) && (!fields[i].FieldType.IsEnum))  ||  (fields[i].FieldType == typeof(string)) )
                {
                    TextBox text = new TextBox
                    {
                        Name = fields[i].Name,
                        Location = new Point(15 + label.Width, 25 * (i + 1)),
                        Width = form.Width - (label.Location.X + label.Width + 30),
                        Text = fields[i].GetValue(item).ToString()
                    };
                    form.Controls.Add(text);

                }//Создание выпадающих списков для перечислимых типов
                else if (fields[i].FieldType.IsEnum)
                {
                    ComboBox combobox = new ComboBox
                    {
                        Name = fields[i].Name,
                        SelectionStart = 0,
                        DropDownStyle = ComboBoxStyle.DropDownList,
                        Location = new Point(15 + label.Width, 25 * (i + 1)),
                        Width = form.Width - (label.Location.X + label.Width + 30)
                    };
                    combobox.Items.AddRange(fields[i].FieldType.GetEnumNames());
                    combobox.SelectedIndex = (int)(fields[i].GetValue(item));
                    form.Controls.Add(combobox);

                }//Создание выпадающих списков для вложенных членов
                //else if ((!fields[i].FieldType.IsPrimitive) && (!fields[i].FieldType.IsEnum) && (!(fields[i].FieldType == typeof(string))))
                else if ((fields[i].FieldType.IsClass))
                {
                    ComboBox combobox = new ComboBox
                    {
                        Name = fields[i].Name,
                        SelectionStart = 0,
                        DropDownStyle = ComboBoxStyle.DropDownList,
                        Location = new Point(15 + label.Width, 25 * (i + 1)),
                        Width = form.Width - (label.Location.X + label.Width + 30)
                    };

                    //список объектов удовлетворяющих типу поля
                    List<object> suitableItems = items.Where(s_item => ((s_item.GetType() == fields[i].FieldType) || (s_item.GetType().BaseType == fields[i].FieldType))).ToList();

                    for (int j = 0; j < suitableItems.Count; j++)
                    {
                        //var nameField = suitableItems[0].GetType().GetField("name");
                        //if (nameField != null)
                        //combobox.Items.Add(nameField.GetValue(suitableItems[j]));
                        combobox.Items.Add(suitableItems[j].ToString());
                    }

                    var buf = fields[i].GetValue(item);
                    int index = -1;

                    if (buf != null)
                    {
                        for (int j = 0; j < suitableItems.Count; j++)
                        {
                            if (buf.Equals(suitableItems[j]))
                            {
                                index = j; break;
                            }
                        }
                        combobox.SelectedIndex = index;
                    }

                    form.Controls.Add(combobox);
                }
            }

            //кнопка сохранения
            Button btn = new Button
            {
                Name = "btnSave",
                Text = "Save",
                Location = new Point(form.Width / 2 - (form.Width / 8), (fields.Length + 1) * 25),
                Width = form.Width / 4,
                DialogResult = DialogResult.OK,
            };

            btn.Click += saveEvent;
            form.Controls.Add(btn);

            return form;
        }


        public static void ListRedraw(ListView listView, List<Object> items ,string fieldWithName)
        {
            listView.Clear();
            listView.Columns.Add("Тип",145);
            listView.Columns.Add("Название", 145);

            for (int i = 0; i < items.Count; i++)
            {
                Type itemType = items[i].GetType();
                object name = String.Empty;

                var nameField = items[i].GetType().GetField(fieldWithName);
                if (nameField != null)
                {
                    name = nameField.GetValue(items[i]);
                }

                name = items[i].ToString();


                string typeString = ""; 
                if (itemType.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() is DescriptionAttribute descriptionAttribute)
                {
                    typeString = descriptionAttribute.Description;
                }
                else
                {
                    typeString = itemType.Name;
                }

                var listItem = new ListViewItem(new string[]{ typeString , name.ToString() });

                listView.Items.Add(listItem);
            }
        }

        public static void DeleteItem(Object item, List<Object> items)
        {
            //список объектов из общего списка
            //у которых среди полей
            //есть поле с типом удаляемого объекта или его родительским типом
            var ownerList = items.Where(itm => (itm.GetType()
            .GetFields()
            .Where(fld => ((fld.FieldType == item.GetType() || fld.FieldType.BaseType == item.GetType())))).ToList().Count > 0);

            foreach (var owner in ownerList)
            {
                foreach (var fld in owner.GetType().GetFields().Where(fld => (fld.FieldType == item.GetType())).ToList())
                {

                    if ((fld.GetValue(owner) != null) && (fld.GetValue(owner).Equals(item)))
                    {
                        fld.SetValue(owner, null);
                    }
                }
            }

            items.Remove(item);
        }
    }
}
