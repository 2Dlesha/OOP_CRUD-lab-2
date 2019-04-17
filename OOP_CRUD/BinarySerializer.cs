using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace OOP_CRUD
{

    class BinarySerializer : ISerializer
    {
        public void Serialize(Object itemList)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream("CRUD.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, itemList);
            }
        }

        public Object Deserialize(Object itemList)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            // десериализация из файла people.dat
            List<Object> buf = new List<object>();
            using (FileStream fs = new FileStream("CRUD.dat", FileMode.OpenOrCreate))
            {
                buf = (List<Object>)formatter.Deserialize(fs);
            }

            return buf;
        }
    }

    class JSONSerializer : ISerializer
    {

        public void Serialize(Object itemList)
        {

            string jsonObject = JsonConvert.SerializeObject(itemList, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            });

            MessageBox.Show(jsonObject);

            using (StreamWriter fs = new StreamWriter("CRUD.json"))
            {
                fs.Write(jsonObject);
            }

        }

        public Object Deserialize(Object itemList)
        {
            string jsonObject = String.Empty;
            using (StreamReader fs = new StreamReader("CRUD.json"))
            {
                jsonObject = fs.ReadToEnd();
            }

            object deserializedObject = JsonConvert.DeserializeObject<Object>(jsonObject, new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                TypeNameHandling = TypeNameHandling.All
            });

            return (List<Object>)deserializedObject; 
        }
    }

    class TextSerializer : ISerializer
    {

        public void Serialize(Object itemList)
        {
            string jsonObject = JsonConvert.SerializeObject(itemList, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            });

            MessageBox.Show(jsonObject);

            using (StreamWriter fs = new StreamWriter("CRUD.json"))
            {
                fs.Write(jsonObject);
            }
        }

        public Object Deserialize(Object itemList)
        {
            string jsonObject = String.Empty;
            using (StreamReader fs = new StreamReader("CRUD.json"))
            {
                jsonObject = fs.ReadToEnd();
            }



            
            object deserializedObject = new List<Object>();
            return (List<Object>)deserializedObject;
        }
    }





}
