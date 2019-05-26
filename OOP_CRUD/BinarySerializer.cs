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

        public void Serialize(Object itemList, Stream streamName)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(streamName, itemList);
        }

        public Object Deserialize(Stream streamName)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Object buf = null;
            buf = formatter.Deserialize(streamName);

            return buf;
        }
    }
}
