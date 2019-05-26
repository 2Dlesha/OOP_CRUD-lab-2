using System;
using System.IO;

namespace OOP_CRUD
{
    public interface ISerializer
    {
        void Serialize(Object item, Stream streamName);
        Object Deserialize(Stream streamName);
        //string FilePath { get; set; }
        string FileExtension{ get;}
    }
}
