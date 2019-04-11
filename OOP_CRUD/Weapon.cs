using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_CRUD
{
    [Description("Оружие")]
    public class Weapon
    {
        public string name;
        public float weight;
        public string material;
        public int durability;
        public int damage;

        public Weapon()
        {
            name = "undefined";
            weight = 0;
            material = "test";
            durability = 0;
            damage = 0;
        }
    }

    [Description("Боеприпасы")]
    public class Ammunition
    {
        public string name;
        public float weight;

        public Ammunition()
        {
            name = "undefined";
            weight = 0;
        }
    }

    [Description("Стрела")]
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

    [Description("Пуля")]
    public class Bullet : Ammunition
    {
        public float caliber;
        public int penetratingAbility;

        public Bullet()
        {
            caliber = 0;
            penetratingAbility = 0;
        }
    }

    [Description("Оружие дальнего боя")]
    public class RangedWeapon : Weapon
    {
        public int distance;
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

    [Description("Оружие ближнего боя")]
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

    [Description("Клинок")]
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

    [Description("Огенстрельное оружие")]
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

    [Description("Прицел")]
    public class Gunsight
    {
        public string name;
        public AimType aimType;
        public int zoom;

        public Gunsight()
        {
            name = "undefined";
            aimType = AimType.None;
            zoom = 0;
        }
    }

    [Description("Автоматическая винтовка")]
    public class AutomaticRifle : Firearm
    {
        public BladedWeapon bayonet = null;
        public bool butt;
        public Gunsight gunsight = null;

        public AutomaticRifle()
        {
            butt = false;
        }
    }

    public enum MechanismType{None = 0,NotСollapsible = 1, Collapsible, Folding};

    [Description("Метательное оружие")]
    public class ThrowingWeapon : RangedWeapon
    {
        public MechanismType mechanismType;

        public ThrowingWeapon()
        {
            mechanismType = 0;
        }
    }

    [Description("Лук")]
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

    [Description("Арбалет")]
    public class Crossbow : ThrowingWeapon
    {
        public string bowstringType;
        public Gunsight gunsight = null;
        public string triggerType;

        public Crossbow()
        {
            bowstringType = " ";
            triggerType = " ";
        }
    }



    public interface ICreator
    {
        object Create();
    }

    public class ArrowCreator : ICreator
    {
        public object Create()
        {
            return new Arrow();
        }
    }

    public class BulletCreator : ICreator
    {
        public  object Create()
        {
            return new Bullet();
        }
    }


    public class AutomaticRifleCreator : ICreator
    {
        public object Create()
        {
            return new AutomaticRifle();
        }
    }

    public class BladeCreator : ICreator
    {
        public object Create()
        {
            return new BladedWeapon();
        }
    }

    public class BowCreator : ICreator
    {
        public object Create()
        {
            return new Bow();
        }
    }

    public class CrossBowCreator : ICreator
    {
        public object Create()
        {
            return new Crossbow();
        }
    }

    public class GunsightCreator : ICreator
    {
        public object Create()
        {
            return new Gunsight();
        }
    }
}

