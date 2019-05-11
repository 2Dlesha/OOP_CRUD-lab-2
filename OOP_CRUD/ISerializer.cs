using System;

namespace OOP_CRUD
{
    public interface ISerializer
    {
        void Serialize(Object item, string fileName);
        Object Deserialize(string fileName);
        //string FilePath { get; set; }
        string FileExtension{ get;}
    }
}
