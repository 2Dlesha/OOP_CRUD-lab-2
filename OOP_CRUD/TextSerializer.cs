using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace OOP_CRUD
{
    class TextSerializer : ISerializer
    {

        public string FilePath { get; set; }

        public TextSerializer()
        {
            FilePath = "CRUD.data";
        }


        public void Serialize(Object itemList)
        { 


            string objectInfo = String.Empty;
            //MessageBox.Show(objectInfo);
            using (StreamWriter streamWriter= new StreamWriter(FilePath))
            {
                streamWriter.Write(objectInfo);
            }
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
