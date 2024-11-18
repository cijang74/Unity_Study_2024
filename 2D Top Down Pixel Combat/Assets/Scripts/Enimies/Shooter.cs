using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour, IEnemy
{
    // 탄막 발사 스크립트
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletMoveSpeed; // 투사체속도
    [SerializeField] private int burstCount; // 탄막발사횟수
    [SerializeField] private int projectilesPerBurst; // 탄막 라인수
    [SerializeField][Range(0, 359)] private float angleSpread; // 발사 각도
    [SerializeField] private float startDistance = 0.1f; // 탄막 사이의 거리
    [SerializeField] private float timeBetweenBrusts; // 공격속도
    [SerializeField] private float restTime = 1f;
    [SerializeField] private bool stagger;
    [SerializeField] private bool oscillage;

    private bool isShooting = false;

    public void Attack()
    {
        if(!isShooting)
        {
            StartCoroutine(ShootRoutine());
        }
    }

    private IEnumerator ShootRoutine()
    {
        isShooting = true;

        float startAngle, curruentAngle, angleStep;
        TargetConeOfInfluence(out startAngle, out curruentAngle, out angleStep);

        for (int i = 0; i < burstCount; i++) // 탄막발사횟수 i번만큼 반복
        {
            for (int j = 0; j < projectilesPerBurst; j++) // 탄막라인수 j번만큼 반복
            {
                Vector2 pos = FindBulletSpawnPos(curruentAngle);

                GameObject newBullet = Instantiate(bulletPrefab, pos, Quaternion.identity); // 총알의 인스턴스화
                newBullet.transform.right = newBullet.transform.position - transform.position;
                // 인스턴스화한 총알의 정면 방향은 인스턴스화한 총알의 위치 - 해당 스크립트가 적용된 오브젝트의 위치


                if (newBullet.TryGetComponent(out Projectile projectile))
                // 만약 Projectile컴포넌트를 불러오려고 시도했을경우 아래 코드 실행
                {
                    projectile.UpdateMoveSpeed(bulletMoveSpeed); // 투사체속도 업데이트
                }

                curruentAngle += angleStep;
            }
            curruentAngle = startAngle;
            yield return new WaitForSeconds(timeBetweenBrusts);
            TargetConeOfInfluence(out startAngle, out curruentAngle, out angleStep);
        }

        yield return new WaitForSeconds(restTime);
        isShooting = false;
    }

    private void TargetConeOfInfluence(out float startAngle, out float curruentAngle, out float angleStep)
    {
        // 탄막 각도를 설정하는 함수 

        Vector2 targetDirection = PlayerController.Instance.transform.position - transform.position;
        // 타겟 방향은 플레이어와 탄막발사위치간의 방향벡터방향

        // 탄막 각도 계산 관련
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        startAngle = targetAngle;
        curruentAngle = targetAngle;
        float endAngle = targetAngle;
        float halfAngleSpread = 0f;
        angleStep = 0;
        if (angleSpread != 0)
        // anlgeSpread가 0이라면 직선발사
        {
            angleStep = angleSpread / (projectilesPerBurst - 1);
            // 특정 범위내에서 발사되는 투사체 라인 수
            halfAngleSpread = angleSpread / 2f;
            startAngle = targetAngle - halfAngleSpread;
            endAngle = targetAngle + halfAngleSpread;
            curruentAngle = startAngle;
        }
    }

    private Vector2 FindBulletSpawnPos(float curruentAngle)
    // 다음 총알 생성 포인트를 찾는 매서드
    {
        float x = transform.position.x + startDistance * Mathf.Cos(curruentAngle * Mathf.Deg2Rad);
        float y = transform.position.y + startDistance * Mathf.Sin(curruentAngle * Mathf.Deg2Rad);
        
        Vector2 pos = new Vector2(x, y);
        return pos;
    }
}
