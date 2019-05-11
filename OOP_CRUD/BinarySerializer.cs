using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OOP_CRUD
{

    public class BinarySerializer : ISerializer
    {
        public string FileExtension { get; } = ".dat";

        public BinarySerializer()
        {
        }

        public void Serialize(Object itemList, string fileName)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, itemList);
            }
        }

        public Object Deserialize(string fileName)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Object buf = null;
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                buf = formatter.Deserialize(fs);
            }

            return buf;
        }
    }
}
