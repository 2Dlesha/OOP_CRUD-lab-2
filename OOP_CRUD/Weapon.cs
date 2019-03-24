using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_CRUD
{
    /// <summary>
    /// Оружие
    /// </summary>
    public class Weapon
    {
        public string name;
        public int weight;
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

    public class Ammunition
    {
        public string name;
        public int weight;

        public Ammunition()
        {
            name = "undefined";
            weight = 0;
        }
    }

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

    public class Bullet : Ammunition
    {
        public int caliber;
        public int penetratingAbility;

        public Bullet()
        {
            caliber = 0;
            penetratingAbility = 0;
        }
    }

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

    public class Firearm : RangedWeapon
    {
        public int catrigeCapacity;
        public bool isAutomatic;
        public int caliber;
        public int numberOfBarrels;

        public Firearm()
        {
            catrigeCapacity = 0;
            isAutomatic = false;
            caliber = 0;
            numberOfBarrels = 0;
        }

    }

    public class Gunsight
    {
        public string name;
        public string aimType;
        public int zoom;

        public Gunsight()
        {
            name = "undefined";
            aimType = " ";
            zoom = 0;
        }
    }

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

    public class ThrowingWeapon : RangedWeapon
    {
        public MechanismType mechanismType;

        public ThrowingWeapon()
        {
            mechanismType = 0;
        }
    }

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




    public abstract class AmmunitionCreator
    {
        public abstract Ammunition Create();
    }

    public class ArrowCreator : AmmunitionCreator
    {
        public override Ammunition Create()
        {
            return new Arrow();
        }
    }

    public class BulletCreator : AmmunitionCreator
    {
        public override Ammunition Create()
        {
            return new Bullet();
        }
    }



    public abstract class WeaponCreator
    {
        public abstract Weapon Create();
    }

    public class AutomaticRifleCreator : WeaponCreator
    {
        public override Weapon Create()
        {
            return new AutomaticRifle();
        }
    }

    public class BladeCreator : WeaponCreator
    {
        public override Weapon Create()
        {
            return new BladedWeapon();
        }
    }

    public class BowCreator : WeaponCreator
    {
        public override Weapon Create()
        {
            return new Bow();
        }
    }

    public class CrossBowCreator : WeaponCreator
    {
        public override Weapon Create()
        {
            return new Crossbow();
        }
    }


    public class GunsightCreator 
    {
        public virtual Gunsight Create()
        {
            return new Gunsight();
        }
    }
}

