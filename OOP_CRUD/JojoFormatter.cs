using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OOP_CRUD
{
    public class JojoFormatter
    {
        private List<Container> _referenceList = new List<Container>();
        private int _idCounter = 1;
        public char _arrayOpenSymbol = '[';
        public char _arrayCloseSymbol = ']';
        public char _objectOpenSymbol = '{';
        public char _objectCloseSymbol = '}';
        public char _objectDelimeter = ',';
        public string _arrayAttribute = "$values";
        public string _typeAttribute = "$type";
        public string _idAttribute = "$id";
        public string _referenceAttribute = "$ref";
        public string tab = "  ";

        private string FormatList(Object objectList, string tabs)
        {
            StringBuilder listBuilder = new StringBuilder();

            listBuilder.AppendLine(tabs  +'"' + _arrayAttribute + '"' + ":" + tab + _arrayOpenSymbol);
            bool firstFlag = true;
            foreach (object obj in (List<Object>)objectList)
            { 
                if (firstFlag)
                    firstFlag = false;
                else
                    listBuilder.AppendLine(_objectDelimeter.ToString());
                
                listBuilder.Append(tabs + tab);
                listBuilder.Append(FormatObject(obj, tabs + tab));
            }
            listBuilder.AppendLine();
            listBuilder.AppendLine(tabs + _arrayCloseSymbol);

            return listBuilder.ToString();
        }

        private int FindID(Object obj)
        {
            if (_referenceList.Count > 0)
            {
                for (int i = 0; i < _referenceList.Count; i++)
                {
                    if (_referenceList[i].container.Equals(obj))
                        return _referenceList[i].id;
                }
            }
            return -1;
        }

        private bool IsAlreadyExist(Object obj)
        {
            return FindID(obj) != -1;
        }

        private string FormatReference(Object obj, string tabs)
        {
            StringBuilder referenceBuilder = new StringBuilder();

            referenceBuilder.AppendLine(_objectOpenSymbol.ToString());
            referenceBuilder.AppendLine(tabs + tab + _referenceAttribute + ':' + tab + FindID(obj));
            referenceBuilder.Append(tabs + _objectCloseSymbol);

            return referenceBuilder.ToString();
        }

        private string FormatObject(Object obj, string tabs = "")
        {
            StringBuilder objectBuilder = new StringBuilder();

            if (IsAlreadyExist(obj))
                return FormatReference(obj, tabs);
            else
                _referenceList.Add(new Container(obj, _idCounter));

            objectBuilder.AppendLine(_objectOpenSymbol.ToString());
            objectBuilder.AppendLine(tabs + tab + '"' + _typeAttribute + '"' + ":" + tab + '"' + obj.GetType().FullName + '"' + _objectDelimeter);
            objectBuilder.AppendLine(tabs + tab + '"' + _idAttribute + '"' + ":" + tab + (_idCounter++).ToString() + _objectDelimeter);

            if (obj.GetType().GetInterface("ICollection") != null || (obj.GetType().GetInterface("IEnumerable`1") != null))
                objectBuilder.Append(FormatList(obj, tabs + tab));
            else
            {
                bool firstProp = true;
                foreach (var property in obj.GetType().GetProperties())
                {
                    if (firstProp)
                        firstProp = false;
                    else
                        objectBuilder.AppendLine(_objectDelimeter.ToString());

                    objectBuilder.Append(FormatProperty(property, obj, tabs));
                }
                objectBuilder.AppendLine();
            }
            objectBuilder.Append(tabs + _objectCloseSymbol);

            return objectBuilder.ToString();
        }

        private string FormatProperty(PropertyInfo property, Object obj, string tabs)
        {
            StringBuilder propertyBuilder = new StringBuilder();
            propertyBuilder.Append(tabs + tab + '"' + property.Name + '"' + ':' + tab);

            if ((property.PropertyType.IsPrimitive) || (property.PropertyType.IsEnum) || (property.PropertyType == typeof(string)) || (property.PropertyType.IsValueType))
            {
                if (property.PropertyType == typeof(string))
                {
                    propertyBuilder.Append('"' + property.GetValue(obj).ToString() + '"');
                }
                else if (property.PropertyType.IsEnum)
                {
                    propertyBuilder.Append(((int)property.GetValue(obj)).ToString());
                }
                else
                    propertyBuilder.Append(property.GetValue(obj).ToString());
            }
            else if (property.PropertyType.IsClass)
            {
                object nestedObject = property.GetValue(obj);
                if (nestedObject != null)
                    propertyBuilder.Append(FormatObject(nestedObject, tabs + tab));
                else
                    propertyBuilder.Append("null");
            }

            return propertyBuilder.ToString();
        }

        public string GetObjectInfo(Object item)
        {
            _idCounter = 1;
            return FormatObject(item);
        }
    }

    public class Container
    {
        public Container(object obj, int id)
        {
            this.container = obj;
            this.id = id;
        }
        public Container(){}

        public int id = 0;
        public object container = null;
    }
}
