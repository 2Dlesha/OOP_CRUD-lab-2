using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP_CRUD
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            List<Type> types = new List<Type>(){
                typeof(AutomaticRifle),
                typeof(Crossbow),
                typeof(Bow),
                typeof(BladedWeapon),
                typeof(Gunsight),
                typeof(Bullet),
                typeof(Arrow),
            }; 

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CRUDForm(types, new CRUDHelper()));
        }
    }
}
