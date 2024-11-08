using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;

    // private void OnTriggerEnter2D(Collider2D other) // other과 충돌하면 실행되는 메서드
    // {
    //     if(other.gameObject.GetComponent<EnemyHealth>()) // EnemyHealth를 가진 게임 오브젝트라면
    //     {
    //         EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
    //         enemyHealth.TakeDamage(damageAmount); // 공격력을 1이라고 가정, TakeDamage 함수에 1을 전달
    //     }
    // }

    private void OnTriggerEnter2D(Collider2D other) // other과 충돌하면 실행되는 메서드
    {
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        // enemy.Health가 존재한다면 공격력을 1이라고 가정, TakeDamage 함수에 1을 전달
        enemyHealth?.TakeDamage(damageAmount);
    }
}
