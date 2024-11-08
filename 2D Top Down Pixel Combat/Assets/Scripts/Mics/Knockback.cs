using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    [SerializeField] private float knockbackTime = .2f;

    private Rigidbody2D rb;
    public bool GettingKnockedBack {get; private set; }
    // 위 생략방식은 아래 함수들과 같은 역할을 한다. 단, 참조 변수는 무조건 private이어야만 한다.
    // public bool GetgettingKnockedBack()
    // {
    //     return facingLeft;
    // }

    // private void SetgettingKnockedBack(bool value)
    // {
    //     facingLeft = value;
    // }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void GetKnockedBack(Transform damageSource, float KnockBackThrust)
    {
        GettingKnockedBack = true;
        // 데미지가 가해지는 방향(왼쪽에서 맞았으면 오른쪽으로 밀려나가야 함), 
        // 넉백 추력(무기에 따라 다를 수 있음.)을 매개변수로 받아서 넉벡을 시켜주는 함수.
        Vector2 difference = (transform.position - damageSource.position).normalized * KnockBackThrust * rb.mass;
        // x값과 y값이 같은 2차원 벡터 생성. (정규화된 방향벡터 * 넉벡량 * 몬스터 질량)
        rb.AddForce(difference, ForceMode2D.Impulse); // Impulse: 특정 방향으로 매우 빠르게 힘을 가한다.
        StartCoroutine(KnockRoutine());
    }

    // 코루틴은 시간의 경과에 따른 절차적 단계를 수행하는데에 사용되는 함수
    // 1번째 호출 끝나고 다시 코루틴 호출하면 1번째 호출 리턴 라인 이후부터 시작.
    // 마지막번째 호출이 끝나고 다시 코루틴 호출하면 마지번째의 전 호출 리턴 라인부터
    private IEnumerator KnockRoutine()
    {
        yield return new WaitForSeconds(knockbackTime); // 넉벡당하는 시간까지 기다렸다가 리턴
        rb.velocity = Vector2.zero; // rb에 가해지고 있는 속도를 0으로 설정
        GettingKnockedBack = false;
    }
}
