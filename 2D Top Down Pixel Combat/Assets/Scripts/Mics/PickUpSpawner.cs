using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    // 픽업 아이템을 인스턴스화하는 오브젝트에 부착시킬 스크립트
    [SerializeField] private GameObject goldCoin, healthGlobe, staminaGlobe;

    public void DropItems()
    {
        // 1은 포함, 5는 제외
        int randimNum = Random.Range(1, 5);

        if(randimNum == 1)
        {
            Instantiate(healthGlobe, transform.position, Quaternion.identity);
        }
        
        if(randimNum == 2)
        {
            Instantiate(staminaGlobe, transform.position, Quaternion.identity);
        }

        if(randimNum == 3)
        {
            int randomAmountOfGold = Random.Range(1,4);

            for(int i = 0; i < randomAmountOfGold; i++)
            // 1개에서 2개사이의 골드가 생성될 수 있음.
            {
                Instantiate(goldCoin, transform.position, Quaternion.identity);
            }
        }
    }
}
