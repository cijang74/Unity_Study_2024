using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grape : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject grapeProjectilePrefab;

    private Animator myAnimator;
    private SpriteRenderer spriteRenderer;

    readonly int ATTACK_HASH = Animator.StringToHash("Attack");
    // 읽기전용 상수 ATTACK_HASH는 "Attack"이라는 문자열을 해쉬형으로 바꾼 value값을 가진 해쉬임

    private void Awake() 
    {
        myAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Attack()
    {
        myAnimator.SetTrigger(ATTACK_HASH);

        // 공격할때 플레이어방향으로 뒤집기
        if(transform.position.x - PlayerController.Instance.transform.position.x < 0)
        {
            spriteRenderer.flipX = false;
        }

        else
        {
            spriteRenderer.flipX = true;
        }
    }

    public void SpawnProjectileAnimEvent()
    // 애니메이션에 이벤트 프레임으로 박을거임
    {
        Instantiate(grapeProjectilePrefab, transform.position, Quaternion.identity); // 총알의 인스턴스화
    }

    public void Trace()
    {

    }

    public void Stop()
    {
        
    }
}
