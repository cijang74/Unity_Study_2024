using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    // 스크립터블 오브젝트 WeaponInfo의 정보를 다른 스크립트에서 접근할 수 있도록 하는 클래스
    [SerializeField] private WeaponInfo weaponInfo;

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }
}
