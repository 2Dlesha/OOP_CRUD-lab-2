using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace OOP_CRUD
{
    class TextSerializer : ISerializer
    {

        public string FilePath { get; set; }

        public TextSerializer()
        {
            FilePath = "CRUD.data";
        }


        private string GetObjectInfo(Object item, string tabs)
        {
            string objectInfo = string.Empty;
            objectInfo += "{\n";
            objectInfo +=tabs + "$type: " + "\"" + item.GetType().ToString() + "\",\n";


            if (item.GetType().GetInterface("ICollection") != null || (item.GetType().GetInterface("IEnumerable`1") != null))
            {
                objectInfo += "[\n";
                foreach (object itm in (List<Object>)item)
                {
                    objectInfo +=  GetObjectInfo(itm,tabs + "    ");
                }
                objectInfo += "]\n";
            }


            foreach (var property in item.GetType().GetProperties())
            {
                objectInfo += '"' + property.Name + '\n';

                if (((property.PropertyType.IsPrimitive) || (property.PropertyType.IsEnum)) || (property.PropertyType == typeof(string))|| (property.PropertyType.IsValueType))
                {
                    objectInfo += "=\"" + property.GetValue(item) + "\n";
                }
                else if (property.PropertyType.IsClass)
                {
                    try
                    {
                        object classItem = property.GetValue(item);
                        if (classItem != null)
                        {
                            objectInfo += GetObjectInfo(classItem, tabs + "        ");
                        }
                    }
                    catch
                    {

                    }
                }
                    

                objectInfo += "    </" + property.Name + ">\n";
            }
            objectInfo +=  "</" + item.GetType().ToString() + ">\n";
            return objectInfo;
        }

        public void Serialize(Object itemList)
        {
            string objectInfo = String.Empty;
            objectInfo = GetObjectInfo(itemList, "");
            MessageBox.Show(objectInfo);
        }

        public Object Deserialize(Object itemList)
        {
            string objectInfo = String.Empty;
            using (StreamReader streamReader = new StreamReader(FilePath))
            {
                objectInfo = streamReader.ReadToEnd();
            }
            object deserializedObject = new List<Object>();
            return (List<Object>)deserializedObject;
        }
    }





}
