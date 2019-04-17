using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_CRUD
{
    [DisplayName("Оружие")]
    [Serializable]
    public class Weapon
    {
        public string ModelName { get; set;}
        public float Weight { get; set; }
        public string Material { get; set; }
        public int Durability { get; set; }
        public int Damage { get; set; }

        public Weapon()
        {
            ModelName = "undefined";
            Weight = 0;
            Material = "test";
            Durability = 0;
            Damage = 0;
        }

        public override string ToString()
        {
            return ModelName;
            //return base.ToString();
        }
    }

    [DisplayName("Боеприпасы")]
    [Serializable]
    public class Ammunition
    {
        public float Weight { get; set; }

        public Ammunition()
        {
            Weight = 0;
        }

        public override string ToString()
        {
            return Weight.ToString();
        }
    }

    [DisplayName("Стрела")]
    [Serializable]
    public class Arrow : Ammunition
    {
        public string Name { get; set; }
        public int ShaftLenght { get; set; }
        public int NumberOfFeathers { get; set; }

        public Arrow()
        {
            Name = "undefined";
            ShaftLenght = 0;
            NumberOfFeathers = 0;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    [DisplayName("Пуля")]
    [Serializable]
    public class Bullet : Ammunition
    {
        public float Caliber { get; set; }
        public int PenetratingAbility { get; set; }

        public Bullet()
        {
            Caliber = 0;
            PenetratingAbility = 0;
        }

        public override string ToString()
        {
            return Caliber.ToString();
        }
    }

    [DisplayName("Оружие дальнего боя")]
    [Serializable]
    public class RangedWeapon : Weapon
    {
        public int Distance { get; set; }
        [Description("Aggregation")]
        public Ammunition Ammunition { get; set; }
        public int RechargeTime { get; set; }
        public int ShotsPerMinute { get; set; }
        public int Accuracy { get; set; }

        public RangedWeapon()
        {
            Distance = 0;
            RechargeTime = 0;
            ShotsPerMinute = 0;
            Accuracy = 0;
        }
    }

    public enum DamageType {None =  0, Slashing = 1, Cutting, Piercing, Crushing};

    [DisplayName("Оружие ближнего боя")]
    [Serializable]
    public class MelleeWeapon : Weapon
    {
        public int HandleLenght { get; set; }
        public int Range { get; set; }
        public DamageType DamageType { get; set; }

        public MelleeWeapon()
        {
            HandleLenght = 0;
            Range = 0;
            DamageType = DamageType.None;
        }
    }

    [DisplayName("Клинок")]
    [Serializable]
    public class BladedWeapon : MelleeWeapon
    {
        public int NumberOfBlades { get; set; }
        public int Sharpness { get; set; }
        public int BladeLenght { get; set; }

        public BladedWeapon()
        {
            NumberOfBlades = 0;
            Sharpness = 0;
            BladeLenght = 0;
        }
    }

    [DisplayName("Огенстрельное оружие")]
    [Serializable]
    public class Firearm : RangedWeapon
    {
        public int CatrigeCapacity { get; set; }
        public bool IsAutomatic { get; set; }
        public float Caliber { get; set; }
        public int NumberOfBarrels { get; set; }

        public Firearm()
        {
            CatrigeCapacity = 0;
            IsAutomatic = false;
            Caliber = 0;
            NumberOfBarrels = 0;
        }

    }

    public enum AimType { None = 0, Laser, Collimator, Optic, Holographic };

    [DisplayName("Прицел")]
    [Serializable]
    public class Gunsight
    {
        public AimType AimType { get; set; }
        public int Zoom { get; set; }

        public Gunsight()
        {
            AimType = AimType.None;
            Zoom = 0;
        }

        public override string ToString()
        {
            return AimType.ToString() + " Gunsight";
        }
    }

    [DisplayName("Автоматическая винтовка")]
    [Serializable]
    public class AutomaticRifle : Firearm
    {

        public BladedWeapon Bayonet { get; set; }
        public bool Butt { get; set; }
        [Description("Aggregation")]
        public Gunsight Gunsight{ get; set; }

        public AutomaticRifle()
        {
            Gunsight = null;
            Bayonet = null;
            Butt = false;
        }
    }

    public enum MechanismType{None = 0,NotСollapsible = 1, Collapsible, Folding};

    [DisplayName("Метательное оружие")]
    [Serializable]
    public class ThrowingWeapon : RangedWeapon
    {
        public MechanismType MechanismType { get; set; }

        public ThrowingWeapon()
        {
            MechanismType = 0;
        }
    }

    [DisplayName("Лук")]
    [Serializable]
    public class Bow : ThrowingWeapon
    {
        public string BowstringType { get; set; }
        public int ArcLenght { get; set; }

        public Bow()
        {
            BowstringType = " ";
            ArcLenght = 0;
        }
    }

    [DisplayName("Арбалет")]
    [Serializable]
    public class Crossbow : ThrowingWeapon
    {
        public string BowstringType { get; set; }
        [Description("Aggregation")]
        public Gunsight Gunsight { get; set; }
        public string TriggerType { get; set; }

        public Crossbow()
        {
            Gunsight = null;
            BowstringType = " ";
            TriggerType = " ";
        }
    }

}

