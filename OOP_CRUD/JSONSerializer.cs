using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace OOP_CRUD
{
    class JSONSerializer : ISerializer
    {
        public string FilePath { get; set; }

        public JSONSerializer()
        {
            FilePath = "CRUD.json";
        }

        public void Serialize(Object itemList)
        {

            string jsonObject = JsonConvert.SerializeObject(itemList, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            });

            MessageBox.Show(jsonObject);

            using (StreamWriter fs = new StreamWriter(FilePath))
            {
                fs.Write(jsonObject);
            }

        }

        public Object Deserialize(Object itemList)
        {
            string jsonObject = String.Empty;

            using (StreamReader fs = new StreamReader(FilePath))
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
}
