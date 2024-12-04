using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MagicLaser : MonoBehaviour
{
    // 레이저의 스프라이트와 콜라이더를 늘려주는 스크립트
    
    [SerializeField] private float laserGrowTime = 0.22f; // 레이저 발사시간

    private bool isGrowing = true; // 지형 오브젝트에 닿으면 레이저 확장을 못하게 하는 변수
    private float laserRange; // 레이저 범위
    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D capsuleCollider2D;

    private void Awake() 
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    private void Start() 
    {
        LaserFaceMouse();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.GetComponent<Indestructible>() && !other.isTrigger)
        // 만약 닿은게 파괴불가능한 오브젝트이고 그것이 Trigger가 아니라면 (나무의 나뭇잎 때문에)
        {
            isGrowing = false;
        }
    }

    public void UpdateLaserRange(float laserRange)
    {
        // Staff 스크립트에서 laserRanger값을 전달받아 업데이트
        this.laserRange = laserRange;
        StartCoroutine(IncreaseLaserLengthRoutine());
    }
    
    private void LaserFaceMouse()
    // 레이저가 마우스방향으로 확장되도록 방향을 정해주는 메서드
    {
        Vector3 mousePosition = Input.mousePosition;
        // 실제 마우스 위치를 입력받음
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        // 게임 내 화면 마우스 위치로 변환
        Vector2 direction = transform.position - mousePosition;
        // 내 마우스 위치 - 캐릭터의 위치 => 마우스와 캐릭터에 대한 방향벡터

        transform.right = -direction;
    }

    private IEnumerator IncreaseLaserLengthRoutine()
    {
        //레이저의 스프라이트와 콜라이더를 늘려주는 메서드
        float timePassed = 0f;

        while(spriteRenderer.size.x < laserRange && isGrowing == true)
        // 스프라이트의 x축 크기가 레이저 범위까지 다다를때까지 반복
        {
            timePassed += Time.deltaTime; // 시간 세주기
            float linearT = timePassed / laserGrowTime;

            // 스프라이트 크기 늘리기
            // Mathf.Lerp: ~에서 ~까지 ~초동안 수렴
            spriteRenderer.size = new Vector2(Mathf.Lerp(1f, laserRange, linearT), 1f);

            // 콜라이더 크기와 포지션 늘리기
            capsuleCollider2D.size = new Vector2(Mathf.Lerp(1f, laserRange, linearT), capsuleCollider2D.size.y);
            capsuleCollider2D.offset = new Vector2(Mathf.Lerp(1f, laserRange, linearT) / 2, capsuleCollider2D.offset.y);
            // offset은 한쪽방향으로만 이동하는거니까 사실상 /2해주어야 함.

            yield return null;
        }
        StartCoroutine(GetComponent<SpriteFade>().SlowFadeRoutine());
    }
}
