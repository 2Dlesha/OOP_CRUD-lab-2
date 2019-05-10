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
        public string FilePath { get; set; }

        public BinarySerializer()
        {
            FilePath = "CRUD.dat";
        }

        public void Serialize(Object itemList)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, itemList);
            }
        }

        public Object Deserialize(Object itemList)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Object buf = null;
            using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate))
            {
                buf = formatter.Deserialize(fs);
            }

            return buf;
        }
    }
}
