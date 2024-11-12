using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스크립터블 오브젝트를 만들어주는 스크립트
[CreateAssetMenu(menuName = "New Weapon")]

public class WeaponInfo : ScriptableObject
{
    // 해당 오브젝트의 속성값(인스펙터에 뜰 요소들)
    public GameObject weaponPrefab;
    public float weaponCooldown;
}
