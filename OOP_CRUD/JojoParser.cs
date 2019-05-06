using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OOP_CRUD
{
    class JojoParser
    {
        private List<Container> _referenceList = new List<Container>();

        public bool CutNextToken(ref string svList, string separator, out string token)
        {

            token = "";
            if (svList == "")
                return false;

            List<char> openBrakets = new List<char>(){ '{', '[' };
            List<char> closeBrakets = new List<char>() { '}', ']' };

            int i = 0;
            int j = 0;

            Stack<char> brStack = new Stack<char>();
            bool quotes = false;

            while (i < svList.Length)
            {
                if (svList[i] == '"')
                    quotes = !quotes;

                if (openBrakets.Contains(svList[i]))
                {
                    brStack.Push(svList[i]);
                }
                else if (closeBrakets.Contains(svList[i]))
                {
                    brStack.Pop();
                }

                if (svList[i] == separator[j])
                {
                    if ((j == (separator.Length - 1)) && (!quotes)&&(brStack.Count == 0))
                    {
                        token = svList.Substring(0, i - (separator.Length - 1)).Trim();
                        svList = svList.Substring(i + 1).Trim();
                        return true;
                    }
                    else
                    {
                        if ((j == (separator.Length - 1)) && ((quotes)||(brStack.Count != 0)))
                            j = 0;
                        else
                            j++;
                    }
                }
                else
                    j = 0;
                i++;
            }

            token = svList.Trim();
            svList = "";
            return true;
        }

        public string CutTillMatchinPare(char openSymbol, ref string svList, char closeSymbol)
        {
            if (svList == null || svList.Length <= 1)
                return "";

            Stack<char> brStack = new Stack<char>();
            if (svList[0] == openSymbol)
            {
                int i = 0;
                while (i < svList.Length)
                {
                    if (svList[i] == openSymbol)
                        brStack.Push(openSymbol);

                    if (svList[i] == closeSymbol)
                        brStack.Pop();

                    if ((svList[i] == closeSymbol) && (brStack.Count == 0))
                        break;

                    i++;
                }

                if (i < svList.Length)
                {
                    string result = svList.Substring(1, i - 1).Trim();
                    svList = svList.Substring(i + 1).Trim();
                    return result;
                }
            }

            return "";
        }

        private Object CreateClassInstance(string className)
        {
            return Activator.CreateInstance(Type.GetType(className));
        }

        private void SetPropertyValue(Object obj, string propertyName, object propertyValue)
        {
            if (obj == null)
                return;

            PropertyInfo propertyInfo = obj.GetType().GetProperty(propertyName);
            try
            {
                propertyInfo.SetValue(obj, Convert.ChangeType(propertyValue, propertyInfo.PropertyType));
            }
            catch
            {
                Debug.WriteLine("Bad value: " + propertyValue + " for property " + propertyName);
            }
        }

        private void FillObjectList(ref object list, string values)
        {
            var collection = (IList)list;
            if (collection != null)
            {
                var objectsString = CutTillMatchinPare('[', ref values, ']');
                while (CutNextToken(ref objectsString, ",", out string token))
                {
                    collection.Add(ParseJojoObject(token));
                }
            }
            list = collection;

        }

        private Container FindReferenceByID(int id)
        {
            if (_referenceList.Count > 0)
            {
                for (int i = 0; i < _referenceList.Count; i++)
                {
                    if (_referenceList[i].id == id)
                        return _referenceList[i];
                }
            }
            return null;
        }

        private void SetUpObject(ref Container objectContainer, string key, string value)
        {
            switch (key)
            {
                case "$ref":
                    objectContainer = FindReferenceByID(Convert.ToInt32(value));
                    break;
                case "$id":
                    objectContainer.id = Convert.ToInt32(value);
                    break;
                case "$type":
                    objectContainer.container = CreateClassInstance(value);
                    break;
                case "$values":
                    FillObjectList(ref objectContainer.container, value);
                    break;
                default:
                    if ((value.Length > 0) && (value[0] == '{'))
                        SetPropertyValue(objectContainer.container, key, ParseJojoObject(value));
                    else
                        SetPropertyValue(objectContainer.container, key, value);
                    break;
            }
        }

        public Object ParseJojoObject(string obj)
        {
            string objectForParse = obj;
            string currentObjectString = CutTillMatchinPare('{', ref objectForParse, '}');

            Container container = new Container();
            while (CutNextToken(ref currentObjectString, ",", out string token))
            {
                string key, value;
                CutNextToken(ref token, ":", out key);
                CutNextToken(ref token, ":", out value);
                key = key.Trim('"');
                value = value.Trim('"');
                SetUpObject(ref container, key, value);
                //Console.WriteLine(key + ':' + value);
            }

            _referenceList.Add(container);

            return container.container;
        }
    }
}
