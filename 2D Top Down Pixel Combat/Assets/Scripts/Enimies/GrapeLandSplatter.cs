using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapeLandSplatter : MonoBehaviour
{
    // 포도 탄막 관련 스크립트

    private SpriteFade spriteFade;

    private void Awake()
    {
        spriteFade = GetComponent<SpriteFade>();
    }

    private void Start() 
    {
        // 투명해지다가 투명도가 최대에 달하면 오브젝트 삭제
        StartCoroutine(spriteFade.SlowFadeRoutine());

        // DisableCollider()메서드가 0.2초뒤 실행됨
        Invoke("DisableCollider", 0.2f);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        // 포도탄막과 플레이어가 닿으면 플레이어 정보 읽어와서 HP 감소시키기
        PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
        playerHealth?.TakeDamage(1, transform);
    }

    private void DisableCollider()
    {
        GetComponent<CapsuleCollider2D>().enabled = false;
    }
}
