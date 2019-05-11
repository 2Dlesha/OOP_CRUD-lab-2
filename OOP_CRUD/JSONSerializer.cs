﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace OOP_CRUD
{
    public class JSONSerializer : ISerializer
    {
        public string FileExtension { get; }  = ".json";

        public JSONSerializer()
        {
        }

        public void Serialize(Object itemList, string fileName)
        {
            string jsonObject = JsonConvert.SerializeObject(itemList, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            });

            using (StreamWriter fs = new StreamWriter(fileName))
            {
                fs.Write(jsonObject);
            }
        }

        public Object Deserialize(string fileName)
        {
            string jsonObject = String.Empty;

            using (StreamReader fs = new StreamReader(fileName))
            {
                jsonObject = fs.ReadToEnd();
            }

            object deserializedObject = JsonConvert.DeserializeObject<Object>(jsonObject, new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                TypeNameHandling = TypeNameHandling.All
            });

            return deserializedObject; 
        }
    }
}
