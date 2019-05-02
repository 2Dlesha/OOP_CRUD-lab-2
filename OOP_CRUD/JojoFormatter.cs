using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OOP_CRUD
{
    class JojoFormatter
    {
        class Container
        {
            public Container(object obj, int id)
            {
                this.container = obj;
                this.id = id;
            }

            public int id = 0;
            public object container = null;
        }

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
            StringBuilder textSerializerBuilder = new StringBuilder();

            textSerializerBuilder.AppendLine(tabs  +'"' + _arrayAttribute + '"' + ":" + tab + _arrayOpenSymbol);

            bool firstProp = true;
            foreach (object obj in (List<Object>)objectList)
            { 
                if (firstProp)
                    firstProp = false;
                else
                    textSerializerBuilder.AppendLine(_objectDelimeter.ToString());
                
                textSerializerBuilder.Append(tabs + tab);
                textSerializerBuilder.Append(FormatObject(obj, tabs + tab));
            }

            textSerializerBuilder.AppendLine();
            textSerializerBuilder.AppendLine(tabs + _arrayCloseSymbol);

            return textSerializerBuilder.ToString();
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
            StringBuilder textSerializerBuilder = new StringBuilder();

            textSerializerBuilder.AppendLine(tabs + _objectOpenSymbol);
            textSerializerBuilder.AppendLine(tabs + tab + _referenceAttribute + ':' + tab + FindID(obj));
            textSerializerBuilder.Append(tabs + _objectCloseSymbol);

            return textSerializerBuilder.ToString();
        }

        private string FormatObject(Object obj, string tabs = "")
        {
            StringBuilder textSerializerBuilder = new StringBuilder();

            if (IsAlreadyExist(obj))
            {
                return FormatReference(obj, tabs);
            }
            else
            {
                _referenceList.Add(new Container(obj, _idCounter++));
            }

            textSerializerBuilder.AppendLine(/*tabs +*/ "" + _objectOpenSymbol);
            textSerializerBuilder.AppendLine(tabs + tab + '"' + _typeAttribute + '"' + ":" + tab + '"' + obj.GetType().ToString() + '"' + _objectDelimeter);
            textSerializerBuilder.AppendLine(tabs + tab + '"' + _idAttribute + '"' + ":" + tab + _idCounter.ToString() + _objectDelimeter);

            if (obj.GetType().GetInterface("ICollection") != null || (obj.GetType().GetInterface("IEnumerable`1") != null))
            {
                textSerializerBuilder.Append(FormatList(obj, tabs + tab));
            }
            else
            {
                bool firstProp = true;
                foreach (var property in obj.GetType().GetProperties())
                {
                    if (firstProp)
                        firstProp = false;
                    else
                        textSerializerBuilder.AppendLine(_objectDelimeter.ToString());

                    textSerializerBuilder.Append(FormatProperty(property, obj, tabs));
                }
                textSerializerBuilder.AppendLine();
            }

            textSerializerBuilder.Append(tabs + _objectCloseSymbol);

            return textSerializerBuilder.ToString();
        }

        private string FormatProperty(PropertyInfo property, Object obj, string tabs)
        {
            StringBuilder textSerializerBuilder = new StringBuilder();
            textSerializerBuilder.Append(tabs + tab + '"' + property.Name + '"' + ':' + tab);

            if ((property.PropertyType.IsPrimitive) || (property.PropertyType.IsEnum) || (property.PropertyType == typeof(string)) || (property.PropertyType.IsValueType))
            {
                if (property.PropertyType == typeof(string))
                {
                    textSerializerBuilder.Append('"' + property.GetValue(obj).ToString() + '"');
                }
                else if (property.PropertyType.IsEnum)
                {
                    textSerializerBuilder.Append(((int)property.GetValue(obj)).ToString());
                }
                else
                    textSerializerBuilder.Append(property.GetValue(obj).ToString());
            }
            else if (property.PropertyType.IsClass)
            {
                object nestedObject = property.GetValue(obj);
                if (nestedObject != null)
                    textSerializerBuilder.Append(FormatObject(nestedObject, tabs + tab));
                else
                    textSerializerBuilder.Append("null");
            }

            return textSerializerBuilder.ToString();
        }


        public string GetObjectInfo(Object item)
        {
            _idCounter = 1;
            return FormatObject(item);
        }
    }
}
