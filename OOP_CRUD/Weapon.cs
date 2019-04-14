using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_CRUD
{
    [DisplayName("Оружие")]
    public class Weapon
    {
        public string modelName;
        public float weight;
        public string material;
        public int durability;
        public int damage;

        public Weapon()
        {
            modelName = "undefined";
            weight = 0;
            material = "test";
            durability = 0;
            damage = 0;
        }

        public override string ToString()
        {
            return modelName;
            //return base.ToString();
        }
    }

    [DisplayName("Боеприпасы")]
    public class Ammunition
    {
        public string name;
        public float weight;

        public Ammunition()
        {
            name = "undefined";
            weight = 0;
        }

        public override string ToString()
        {
            return name;
        }
    }

    [DisplayName("Стрела")]
    public class Arrow : Ammunition
    {
        public int shaftLenght;
        public int numberOfFeathers;

        public Arrow()
        {
            shaftLenght = 0;
            numberOfFeathers = 0;
        }
    }

    [DisplayName("Пуля")]
    public class Bullet : Ammunition
    {
        public float caliber;
        public int penetratingAbility;

        public Bullet()
        {
            caliber = 0;
            penetratingAbility = 0;
        }

        public override string ToString()
        {
            return caliber.ToString();
        }
    }

    [DisplayName("Оружие дальнего боя")]
    public class RangedWeapon : Weapon
    {
        public int distance;
        [Description("Aggregation")]
        public Ammunition ammunition = null;
        public int rechargeTime;
        public int shotsPerMinute;
        public int accuracy;

        public RangedWeapon()
        {
            distance = 0;
            rechargeTime = 0;
            shotsPerMinute = 0;
            accuracy = 0;
        }
    }

    public enum DamageType {None =  0, Slashing = 1, Cutting, Piercing, Crushing};

    [DisplayName("Оружие ближнего боя")]
    public class MelleeWeapon : Weapon
    {
        public int handleLenght;
        public int range;
        public DamageType damageType;

        public MelleeWeapon()
        {
            handleLenght = 0;
            range = 0;
            damageType = DamageType.None;
        }
    }

    [DisplayName("Клинок")]
    public class BladedWeapon : MelleeWeapon
    {
        public int numberOfBlades;
        public int sharpness;
        public int bladeLenght;

        public BladedWeapon()
        {
            numberOfBlades = 0;
            sharpness = 0;
            bladeLenght = 0;
        }
    }

    [DisplayName("Огенстрельное оружие")]
    public class Firearm : RangedWeapon
    {
        public int catrigeCapacity;
        public bool isAutomatic;
        public float caliber;
        public int numberOfBarrels;

        public Firearm()
        {
            catrigeCapacity = 0;
            isAutomatic = false;
            caliber = 0;
            numberOfBarrels = 0;
        }

    }

    public enum AimType { None = 0, Laser, Collimator, Optic, Holographic };

    [DisplayName("Прицел")]
    public class Gunsight
    {
        public AimType aimType;
        public int zoom;

        public Gunsight()
        {
            aimType = AimType.None;
            zoom = 0;
        }

        public override string ToString()
        {
            return aimType.ToString() + " Gunsight";
        }
    }

    [DisplayName("Автоматическая винтовка")]
    public class AutomaticRifle : Firearm
    {

        public BladedWeapon bayonet = null;
        public bool butt;
        [Description("Aggregation")]
        public Gunsight gunsight = null;

        public AutomaticRifle()
        {
            butt = false;
        }
    }

    public enum MechanismType{None = 0,NotСollapsible = 1, Collapsible, Folding};

    [DisplayName("Метательное оружие")]
    public class ThrowingWeapon : RangedWeapon
    {
        public MechanismType mechanismType;

        public ThrowingWeapon()
        {
            mechanismType = 0;
        }
    }

    [DisplayName("Лук")]
    public class Bow : ThrowingWeapon
    {
        public string bowstringType;
        public int arcLenght;

        public Bow()
        {
            bowstringType = " ";
            arcLenght = 0;
        }
    }

    [DisplayName("Арбалет")]
    public class Crossbow : ThrowingWeapon
    {
        public string bowstringType;
        [Description("Aggregation")]
        public Gunsight gunsight = null;
        public string triggerType;

        public Crossbow()
        {
            bowstringType = " ";
            triggerType = " ";
        }
    }

}

