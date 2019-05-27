using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using EncoderPluginInterface;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Configuration;

namespace OOP_CRUD
{
    public partial class CRUDForm : Form
    {
        private List<ISerializer> _serializers = new List<ISerializer>()
        {
            new BinarySerializer(),
            new JSONSerializer(),
            new JojoSerializer()
        };

        public ICRUDHelper _CRUDAssistant = null;
        public Form _editForm = null;
        public List<object> _itemList = new List<object>();
        public List<object> _activeItemList = new List<object>();
        public List<Type> _itemCreator = new List<Type>();

        public string _cipherKey { get; set; }

        public List<IEncoder> _pluginList = new List<IEncoder>();

        public CRUDForm(List<Type> availibleTypes, ICRUDHelper CRUDHelper)
        {
            InitializeComponent();
            _CRUDAssistant = CRUDHelper;
            //CRUDAssistant.ItemsInit(itemList);
            var settings = ConfigurationManager.AppSettings;
            _activeItemList = _itemList;
            _itemCreator = availibleTypes;
            _cipherKey = ConfigurationManager.AppSettings["СipherKey"];

           // _cipherKey = "OOP_1234567890_OOP";
        }




        private void CRUDForm_Load(object sender, EventArgs e)
        {
            foreach (var item in _itemCreator)
            {
                string typeString = item.Name; 

                if (item.GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault() is DisplayNameAttribute displayNameAttribute)
                    typeString = displayNameAttribute.DisplayName;

                comboBoxTypes.Items.Add(typeString);
            }

            comboBoxTypes.SelectedIndex = 0;
            comboBoxTypes.DropDownStyle = ComboBoxStyle.DropDownList;
            itemsListView.View = View.Details;

            _CRUDAssistant.ListRedraw(itemsListView, _activeItemList);

            foreach (var item in _serializers)
            {
                string typeString = item.GetType().Name;

                if (item.GetType().GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault() is DisplayNameAttribute displayNameAttribute)
                    typeString = displayNameAttribute.DisplayName;

                comboBoxChooseSerializer.Items.Add(typeString);
            }

            if(comboBoxChooseSerializer.Items.Count != 0)
                comboBoxChooseSerializer.SelectedIndex = 0;

            comboBoxChooseSerializer.DropDownStyle = ComboBoxStyle.DropDownList;


           /* AppDomainSetup setup = new AppDomainSetup();
            setup.ApplicationBase = @"D:\Study\Univesity\4 term\OOP\CRUD\OOP_CRUD-lab-2\OOP_CRUD\bin\Debug";

            AppDomain pluginDomain = AppDomain.CreateDomain("Plugins", null, setup);
            pluginDomain.DoCallBack(delegate() { LoadPlugins(_pluginList); });*/

            LoadPlugins(_pluginList);

            foreach (var item in _pluginList)
            {
                string typeString = item.GetType().Name;

                if (item.GetType().GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault() is DisplayNameAttribute displayNameAttribute)
                    typeString = displayNameAttribute.DisplayName;

                comboBoxPlugin.Items.Add(typeString);
            }

            if (comboBoxPlugin.Items.Count != 0)
                comboBoxPlugin.SelectedIndex = 0;

            comboBoxPlugin.DropDownStyle = ComboBoxStyle.DropDownList;
        }


        private void CreateItemEditForm(Object item, List<Object> itemList)
        {
            if (item != null)
            {
                _editForm = _CRUDAssistant.CreateForm(item, itemList);
                _editForm.ShowDialog();
                _editForm.Dispose();
            }
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            ConstructorInfo itemConstructor = _itemCreator[comboBoxTypes.SelectedIndex].GetConstructor(new Type[] { });
            object newitem = itemConstructor.Invoke(new object[] { });
            _itemList.Add(newitem);

            _activeItemList = GetActiveList(_itemList, checkBoxFilter.Checked, _itemCreator[comboBoxTypes.SelectedIndex]);

            CreateItemEditForm(newitem, _itemList);

            _CRUDAssistant.ListRedraw(itemsListView, _activeItemList);
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            object item = GetFocusItem();
            if (item != null)
            {
                _CRUDAssistant.DeleteItem(item, _itemList);
                _activeItemList = GetActiveList(_itemList, checkBoxFilter.Checked, _itemCreator[comboBoxTypes.SelectedIndex]);
                _CRUDAssistant.ListRedraw(itemsListView, _activeItemList);
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            object item = GetFocusItem();
            if (item != null)
            {
                CreateItemEditForm(item, _itemList);
                _CRUDAssistant.ListRedraw(itemsListView, _activeItemList);
            }
        }

        public object GetFocusItem()
        {
            object item = null;
            _activeItemList = GetActiveList(_itemList, checkBoxFilter.Checked, _itemCreator[comboBoxTypes.SelectedIndex]);

            if (itemsListView.SelectedIndices.Count != 0)
                item = _activeItemList[itemsListView.SelectedIndices[0]];

            return item;
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _activeItemList = GetActiveList(_itemList, checkBoxFilter.Checked, _itemCreator[comboBoxTypes.SelectedIndex]);
            _CRUDAssistant.ListRedraw(itemsListView, _activeItemList);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            _activeItemList = GetActiveList(_itemList, checkBoxFilter.Checked, _itemCreator[comboBoxTypes.SelectedIndex]);
            _CRUDAssistant.ListRedraw(itemsListView, _activeItemList);
        }

        public  List<object> GetActiveList(List<object> items, bool specificClass, Type classType)
        {
            return (specificClass && (classType != null)) 
                ?  items.Where(item => (item.GetType() == classType)).ToList() 
                :  items;
        }

        private string ChooseFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = @"G:\Учеба\4 сем\ООП\Лабы\lab1\OOP_CRUD lab 2\OOP_CRUD\bin\Debug";
            openFileDialog.ShowDialog();
            return openFileDialog.FileName;
        }

        private ISerializer ChooseSerializer(string ext)
        {
            foreach (var serializer in _serializers)
            {
                if (ext == serializer.FileExtension)
                    return serializer;
            }
            return null;
        }

        private IEncoder ChooseDecoder(string ext)
        {
            foreach (var encoder in _pluginList)
            {
                if (ext == encoder.Expansion)
                    return encoder;
            }
            return null;
        }


        private void buttonSave_Click(object sender, EventArgs e)
        {
            string destinationFileName = ChooseFile();
            

            IEncoder encoder = (comboBoxPlugin.SelectedIndex != -1) ? _pluginList[comboBoxPlugin.SelectedIndex] : null;

            if ((encoder != null) && MessageBox.Show("Шифровать файл?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                encoder = _pluginList[comboBoxPlugin.SelectedIndex];
            else
                encoder = null;
            //

            ISerializer serializer = _serializers[comboBoxChooseSerializer.SelectedIndex]; 
            if (destinationFileName.Length != 0)
            {
                if (serializer.FileExtension != Path.GetExtension(destinationFileName))
                { 
                    destinationFileName += serializer.FileExtension;
                }
                if ( (encoder != null) && encoder.Expansion != Path.GetExtension(destinationFileName))
                {
                    destinationFileName += encoder.Expansion;
                }
            }
            else
            {
                MessageBox.Show("Choose File!");
                return;
            }

            if ((encoder != null))
            {
                using (FileStream ms = new FileStream("buf.txt", FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite))
                {
                    ms.SetLength(0);
                    serializer.Serialize(_itemList, ms);
                }
                using (FileStream ms = new FileStream("buf.txt", FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite))
                using (FileStream fs = new FileStream(destinationFileName, FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite))
                {
                    fs.SetLength(0);
                    encoder.EncryptStream(ms, fs, _cipherKey);
                }

            }
            else
            {
                using (FileStream file = new FileStream(destinationFileName, FileMode.OpenOrCreate))
                    serializer.Serialize(_itemList, file);
            }
            _activeItemList = GetActiveList(_itemList, checkBoxFilter.Checked, _itemCreator[comboBoxTypes.SelectedIndex]);
            _CRUDAssistant.ListRedraw(itemsListView, _activeItemList);
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {

            string destinationFileName = ChooseFile();
            ISerializer serializer;
            IEncoder encoder = null;

            if (destinationFileName.Length != 0)
            {
                encoder = ChooseDecoder(Path.GetExtension(destinationFileName));
                serializer = ChooseSerializer(Path.GetExtension(destinationFileName));

                if (encoder != null)
                {
                    serializer = ChooseSerializer(Path.GetExtension(destinationFileName.Replace(encoder.Expansion, "")));
                }

                if (serializer == null)
                { 
                    MessageBox.Show("Bad file extension");
                    return;
                }
            }
            else
            {
                MessageBox.Show("ChooseFile File!");
                return;
            }

            if (encoder != null)
            {
                using (FileStream ms = new FileStream("buf.txt", FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite))
                using (FileStream fs = new FileStream(destinationFileName, FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite))
                {
                    ms.SetLength(0);
                    encoder.DecryptStream(fs, ms, _cipherKey);
                }
                using (FileStream ms = new FileStream("buf.txt", FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite))
                {
                    _itemList = (List<Object>)serializer.Deserialize(ms);
                }
            }
            else
            {
                using (FileStream file = new FileStream(destinationFileName, FileMode.OpenOrCreate))
                    _itemList = (List<Object>)serializer.Deserialize(file);
            }

            _activeItemList = GetActiveList(_itemList, checkBoxFilter.Checked, _itemCreator[comboBoxTypes.SelectedIndex]);
            _CRUDAssistant.ListRedraw(itemsListView, _activeItemList);
        }

        public void LoadPlugins(List<IEncoder> pluginList)
        {
            var pluginsPath = "Plugins";
            var files = Directory.GetFiles(pluginsPath, "*.dll");

            foreach (var file in files)
            {
                //Console.WriteLine($"Trying load: {file}");
                Assembly assembly;

                try { assembly = Assembly.LoadFrom(file); }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    continue;
                }

                var types = assembly.GetTypes()
                    .Where(type => type.IsClass && type.GetInterface(nameof(IEncoder)) != null);
                    //.Select(t => Activator.CreateInstance(t))
                    //.Cast<IEncoder>()
                    //.Where(x => x != null).ToList();

                foreach (var type in types)
                {
                    var plugin = Activator.CreateInstance(type) as IEncoder;

                    if (plugin == null)
                    {
                        Console.WriteLine("Null");
                        continue;
                    }


                    pluginList.Add(plugin);
                }
            }

        }
    }
}
