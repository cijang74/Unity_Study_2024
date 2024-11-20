using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
// 풀숲과 같은 파괴 가능한 환경오브젝트들을 파괴할 때 애니메이션을 발생시키도록 하는 스크립트
{
    [SerializeField] private GameObject destroyVFX; // 삭제 애니메이션

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<DamageSource>() || other.gameObject.GetComponent<Projectile>())
        // 만약 닿은것이 검의 공격콜라이더거나 투사체라면
        {
            GetComponent<PickUpSpawner>().DropItems();
            Instantiate(destroyVFX, transform.position, Quaternion.identity);
            // 삭제 애니메이션을 해당 스크립트가 적용된 오브젝트의 위치에 생성
            Destroy(gameObject);
        }
    }
}
