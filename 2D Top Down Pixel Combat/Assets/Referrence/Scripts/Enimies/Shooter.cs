using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour, IEnemy
{
    // 탄막 발사 스크립트
    [SerializeField] private GameObject bulletPrefab;
    [Tooltip("투사체속도")]
    [SerializeField] private float bulletMoveSpeed; // 투사체속도
    [Tooltip("한 step마다 발사되는 횟수")]
    [SerializeField] private int burstCount; // 한 step마다 발사되는 횟수
    [Tooltip("한번 발사될때 얼만큼 발사")]
    [SerializeField] private int projectilesPerBurst; // 한번 발사될때 얼만큼 발사
    [Tooltip("발사 각도")]
    [SerializeField][Range(0, 359)] private float angleSpread; // 발사 각도
    [Tooltip("탄막 사이의 거리")]
    [SerializeField] private float startDistance = 0.1f; // 탄막 사이의 거리
    [Tooltip("발사 쿨타임")]
    [SerializeField] private float timeBetweenBrusts; // 발사 쿨타임
    [Tooltip("step쿨타임")]
    [SerializeField] private float restTime = 1f; // step쿨타임
    [Tooltip("스프레이 패턴으로 진동하는 효과는 stagger와 oscillate 둘다 체크해야 함.")]
    [SerializeField] private bool stagger;
    [Tooltip("스프레이 패턴으로 진동하는 효과는 stagger와 oscillate 둘다 체크해야 함.")]
    [SerializeField] private bool oscillate;

    private bool isShooting = false;

    private void OnValidate() 
    // SerializeField값중 하나가 변경될 때 마다 유효성 검사
    {
        if(oscillate) { stagger = true; }
        if(!oscillate) { stagger = false; }
        if(projectilesPerBurst < 1) { projectilesPerBurst = 1; }
        if(burstCount < 1) { burstCount = 1; }
        if(timeBetweenBrusts < 0.1f) { timeBetweenBrusts = 0.1f; }
        if(restTime < 0.1f) { restTime = 0.1f; }
        if(startDistance < 0.1f) { startDistance = 0.1f; }
        if(angleSpread == 0) { projectilesPerBurst = 1; }
        if(bulletMoveSpeed <= 0) { bulletMoveSpeed = 0.1f; }
    }

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

        float startAngle, curruentAngle, angleStep, endAngle;
        float timeBetweenProjectiles = 0f;

        if(stagger == true)
        // stagger옵션 켜져있으면 스프레이 패턴 만들어주기
        {
            timeBetweenProjectiles = timeBetweenBrusts / projectilesPerBurst;
        }

        TargetConeOfInfluence(out startAngle, out curruentAngle, out angleStep, out endAngle);

        for (int i = 0; i < burstCount; i++) // 탄막발사횟수 i번만큼 반복
        {
            if(!oscillate)
            // 옵션 꺼져있으면 원래대로.
            {
                TargetConeOfInfluence(out startAngle, out curruentAngle, out angleStep, out endAngle);
            }

            if(oscillate && i % 2 != 1)
            // 왼쪽-오른쪽 발사 끝나면 타겟의 각도 체크하기
            {
                TargetConeOfInfluence(out startAngle, out curruentAngle, out angleStep, out endAngle);

            }

            else if(oscillate)
            {
                // 옵션 켜져있으면 진동효과 만들어주기 (왼쪽에서 부터 쏘고 다음에는 오른쪽부터 쏘고..)
                // 반대방향으로 다시 진동
                curruentAngle = endAngle;
                endAngle = startAngle;
                startAngle = curruentAngle;
                angleStep *= -1;
            }

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

                if(stagger == true)
                // stagger옵션 켜져있으면 스프레이 패턴 만들어주기
                {
                    yield return new WaitForSeconds(timeBetweenProjectiles);
                }
            }
            curruentAngle = startAngle;

            if(!stagger)
            {
                yield return new WaitForSeconds(timeBetweenBrusts);
            }
        }

        yield return new WaitForSeconds(restTime);
        isShooting = false;
    }

    private void TargetConeOfInfluence(out float startAngle, out float curruentAngle, out float angleStep, out float endAngle)
    {
        // 탄막 각도를 설정하는 함수 

        Vector2 targetDirection = PlayerController.Instance.transform.position - transform.position;
        // 타겟 방향은 플레이어와 탄막발사위치간의 방향벡터방향

        // 탄막 각도 계산 관련
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        startAngle = targetAngle;
        curruentAngle = targetAngle;
        endAngle = targetAngle;
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

    public void Trace()
    {
        
    }

    public void Stop()
    {
        
    }
}
