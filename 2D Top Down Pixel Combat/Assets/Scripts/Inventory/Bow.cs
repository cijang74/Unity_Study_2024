using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }
    
    public void Attack()
    {
        Debug.Log("Bow Attack");
    }
}
