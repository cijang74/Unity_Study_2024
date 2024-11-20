using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    // 픽업 아이템을 인스턴스화하는 오브젝트에 부착시킬 스크립트
    [SerializeField] private GameObject goldCoinPrefab;

    public void DropItems()
    {
        Instantiate(goldCoinPrefab, transform.position, Quaternion.identity);
    }
}
