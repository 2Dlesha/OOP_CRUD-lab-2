using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace OOP_CRUD
{
    public class JojoSerializer : ISerializer
    {
    
        public string FileExtension { get;} = ".jojo";
        public JojoSerializer()
        {
        }
        
        public void Serialize(Object itemList, string fileName)
        {
            string objectInfo = String.Empty;
            JojoFormatter textFormatter = new JojoFormatter();
            objectInfo = textFormatter.GetObjectInfo(itemList);
            using (StreamWriter streamWriter = new StreamWriter(fileName))
            {
                streamWriter.Write(objectInfo);
            }

        }

        public Object Deserialize(string fileName)
        {
            string objectInfo = String.Empty;
            using (StreamReader streamReader = new StreamReader(fileName))
            {
                objectInfo = streamReader.ReadToEnd();
            }
            JojoParser jojoParser = new JojoParser();
            object deserializedObject = jojoParser.ParseJojoObject(objectInfo);
            return deserializedObject;
        }
    }





}
