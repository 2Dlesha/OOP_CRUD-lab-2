using System;

namespace OOP_CRUD
{
    public interface ISerializer
    {
        void Serialize(Object item);
        Object Deserialize(Object item);

    }





}
