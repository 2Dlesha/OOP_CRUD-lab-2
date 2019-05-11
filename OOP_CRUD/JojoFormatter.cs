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
            StringBuilder listFormatter = new StringBuilder();

            listFormatter.AppendLine(tabs  +'"' + _arrayAttribute + '"' + ":" + tab + _arrayOpenSymbol);
            bool firstFlag = true;
            foreach (object obj in (List<Object>)objectList)
            { 
                if (firstFlag)
                    firstFlag = false;
                else
                    listFormatter.AppendLine(_objectDelimeter.ToString());
                
                listFormatter.Append(tabs + tab);
                listFormatter.Append(FormatObject(obj, tabs + tab));
            }
            listFormatter.AppendLine();
            listFormatter.AppendLine(tabs + _arrayCloseSymbol);

            return listFormatter.ToString();
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
            StringBuilder referenceFormatter = new StringBuilder();

            referenceFormatter.AppendLine(_objectOpenSymbol.ToString());
            referenceFormatter.AppendLine(tabs + tab + '"' + _referenceAttribute + '"' + ':' + tab + FindID(obj));
            referenceFormatter.Append(tabs + _objectCloseSymbol);

            return referenceFormatter.ToString();
        }

        private string FormatObject(Object obj, string tabs = "")
        {
            StringBuilder objectFormatter = new StringBuilder();

            if (IsAlreadyExist(obj))
                return FormatReference(obj, tabs);
            else
                _referenceList.Add(new Container(obj, _idCounter));

            objectFormatter.AppendLine(_objectOpenSymbol.ToString());
            objectFormatter.AppendLine(tabs + tab + '"' + _typeAttribute + '"' + ":" + tab + '"' + obj.GetType().FullName + '"' + _objectDelimeter);
            objectFormatter.AppendLine(tabs + tab + '"' + _idAttribute + '"' + ":" + tab + (_idCounter++).ToString() + _objectDelimeter);

            if (obj.GetType().GetInterface("ICollection") != null || (obj.GetType().GetInterface("IEnumerable`1") != null))
                objectFormatter.Append(FormatList(obj, tabs + tab));
            else
            {
                bool firstFlag = true;
                foreach (var property in obj.GetType().GetProperties())
                {
                    if (firstFlag)
                        firstFlag = false;
                    else
                        objectFormatter.AppendLine(_objectDelimeter.ToString());

                    objectFormatter.Append(FormatProperty(property, obj, tabs));
                }
                objectFormatter.AppendLine();
            }
            objectFormatter.Append(tabs + _objectCloseSymbol);

            return objectFormatter.ToString();
        }

        private string FormatProperty(PropertyInfo property, Object obj, string tabs)
        {
            StringBuilder propertyFormatter = new StringBuilder();
            propertyFormatter.Append(tabs + tab + '"' + property.Name + '"' + ':' + tab);

            if ((property.PropertyType.IsPrimitive) || (property.PropertyType.IsEnum) || (property.PropertyType == typeof(string)) || (property.PropertyType.IsValueType))
            {
                if (property.PropertyType == typeof(string))
                {
                    propertyFormatter.Append('"' + property.GetValue(obj).ToString() + '"');
                }
                else if (property.PropertyType.IsEnum)
                {
                    propertyFormatter.Append(((int)property.GetValue(obj)).ToString());
                }
                else
                    propertyFormatter.Append(property.GetValue(obj).ToString());
            }
            else if (property.PropertyType.IsClass)
            {
                object nestedObject = property.GetValue(obj);
                if (nestedObject != null)
                    propertyFormatter.Append(FormatObject(nestedObject, tabs + tab));
                else
                    propertyFormatter.Append("null");
            }

            return propertyFormatter.ToString();
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
