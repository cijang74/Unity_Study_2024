using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageSource : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) // other과 충돌하면 실행되는 메서드
    {
        PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();
        player?.TakeDamage(1, transform);
    }
}