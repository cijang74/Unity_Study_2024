using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour, IEnemy
{
    // 탄막 발사 스크립트
    [SerializeField] private GameObject bulletPrefab;

    public void Attack()
    {
        Vector2 targetDirection = PlayerController.Instance.transform.position - transform.position;
        // 타겟 방향은 플레이어와 탄막발사위치간의 방향벡터방향

        GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        // 총알의 인스턴스화
        newBullet.transform.right = targetDirection; // 인스턴스화한 총알의 정면방향은 타겟방향

    }
}
