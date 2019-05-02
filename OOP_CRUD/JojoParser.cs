using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_CRUD
{
    class JojoParser
    {


        private List<Container> _referenceList = new List<Container>();

        private string ParseList(string str)
        {
            return null;
        }

        public bool CutNextToken(ref string svList, string separator,out string token)
        {
            token = "";

            if (svList == "")
                return false;

            int i = 0;
            int j = 0;

            while (i < svList.Length)
            {
                if (svList[i] == separator[j])
                {
                    if (j == (separator.Length - 1))
                    {
                        token = svList.Substring(0, i - (separator.Length - 1));
                        svList = svList.Substring(i + 1);
                        return true;
                    }
                    else
                        j++;
                }
                else
                    j = 0;
                i++;
            }

            token = svList;
            svList = "";
            return true;
        }

        public string CutTillMatchinPare(char openSymbol,ref string svList, char closeSymbol)
        {
            Stack<char> brStack = new Stack<char>();

            if (svList == null || svList.Length <= 1)
                return "";

            if (svList[0] == openSymbol)
            {
                int i = 0;
                while ( (i == 0) && (svList[i] != closeSymbol) && (i < svList.Length) && (brStack.Count == 0))
                {
                    if (svList[i] == openSymbol)
                        brStack.Push(openSymbol);
                    if (svList[i] == closeSymbol)
                        brStack.Pop();

                    i++;
                }

                if (i < svList.Length)
                {
                    string result = svList.Substring(1, i - 1);
                    svList = svList.Substring(i + 1);
                    return result ;
                }
            }

            return "";
        }

        public Object ParseJojoObject(string obj)
        {
            string objectForParse = obj;
            string currentObjectString = CutTillMatchinPare('{',ref objectForParse, '}');

            Container container = new Container();
            string token;
            while (CutNextToken(ref currentObjectString, "," , out token))
            {
                string key;
                string value;
                CutNextToken(ref currentObjectString, ":", out key);
                value = currentObjectString;
            }


            return null;
        }
    }
}
