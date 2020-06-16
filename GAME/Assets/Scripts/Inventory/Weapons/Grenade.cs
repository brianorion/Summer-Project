using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Grenade : Projectile
{
    private bool haveUpdatedGrenadeInformation = false;
    Dictionary<string, WeaponData> grenadeData = new Dictionary<string, WeaponData>();
    private Rigidbody rb;


    private void Start()
    {
        haveUpdatedGrenadeInformation = true;
        WeaponData weaponData = new WeaponData(defenseWeapon, attackDamage, armor, magicResist);
        grenadeData.Add(name, weaponData);
        rb = GetComponent<Rigidbody>();
    }

    public Dictionary<string, WeaponData> GetGrenadeData()
    {
        return grenadeData;
    }
}