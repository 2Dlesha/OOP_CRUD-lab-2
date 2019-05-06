using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace OOP_CRUD
{
    class JojoSerializer : ISerializer
    {

        public string FilePath { get; set; }

        public JojoSerializer()
        {
            FilePath = "CRUD.txtdata";
        }
        
        public void Serialize(Object itemList)
        {
            string objectInfo = String.Empty;
            JojoFormatter textFormatter = new JojoFormatter();
            objectInfo = textFormatter.GetObjectInfo(itemList);
            //MessageBox.Show(objectInfo);
            using (StreamWriter streamWriter = new StreamWriter(FilePath))
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
            JojoParser jojoParser = new JojoParser();
            object deserializedObject = jojoParser.ParseJojoObject(objectInfo);
            return (List<Object>)deserializedObject;
        }
    }





}
