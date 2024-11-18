using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // 플레이어의 HP와 피격 관련 스크립트

    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockBackTrustAmount = 10f;
    [SerializeField] private float DamageRecoveryTime = 1f;

    private int currentHealth;

    private Knockback knockback;
    private Flash flash;
    private bool canTakeDamage = true;
    
    private void Awake() 
    {
        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
    }

    private void Start() 
    {
        currentHealth = maxHealth;
    }

    private void OnCollisionStay2D(Collision2D other)
    // 플레이어와 other이 계속 닿고있다면 (OncollisionEnter2D쓰면 부비부비해도 1번만 피격)
    {
        EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();

        if(enemy)
        {
            TakeDamage(1, other.transform);
        }
    }

    public void TakeDamage(int damageAmount, Transform hitTransform)
    {
        if(!canTakeDamage == true)
        // 만약 무적시간이면 넉백과 데미지 받지않도록 아래 함수들 무시
        {
            return;
        }

        // 플레이어에게 넉백과 플래시
        knockback.GetKnockedBack(hitTransform, knockBackTrustAmount);
        StartCoroutine(flash.FlashRoutine());

        // 플레이어에게 데미지를 주는 함수
        canTakeDamage = false;
        currentHealth -= damageAmount;
        StartCoroutine(DamageRecoveryRoutine());
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        // 한프레임에 연속 피격받는것을 방지하기 위한 코루틴
        yield return new WaitForSeconds(DamageRecoveryTime);
        canTakeDamage = true;
    }
}
