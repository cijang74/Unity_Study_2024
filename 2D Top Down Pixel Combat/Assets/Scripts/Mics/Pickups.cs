using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    // 픽업아이템에 부착할 스크립트
    
    // 열거자
    private enum PickUpType
    {
        GoldCoin,
        StaminaGlobe,
        healthGlobe
    }

    [SerializeField] private PickUpType pickUpType;
    [SerializeField] private float pickUpDistance = 5f; // 자력 사거리
    [SerializeField] private float acclartionRate = 0.2f; // 자력 가속도
    [SerializeField] private float moveSpeed = 3f; // 자력 강도
    [SerializeField] private AnimationCurve animCurve; // 부드러운 곡션 애니메이션 구현용 컴포넌트
    [SerializeField] private float heightY = 1.5f;
    [SerializeField] private float popDuration = 1f; //코인이 공중에 떠있을 시간

    private Vector3 moveDir;
    private Rigidbody2D rb;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start() 
    {
        StartCoroutine(AnimCurveSpawnRoutine());
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if(other.gameObject.GetComponent<PlayerController>())
        // 만약 닿은게 플레이어라면
        {
            DetectPickUpType();
            Destroy(gameObject);
        }
    }

    private IEnumerator AnimCurveSpawnRoutine()
    {
        float randomX = transform.position.x + Random.Range(-2f, 2f);
        float randomY = transform.position.y + Random.Range(-1f, 1f);

        Vector2 startPoint = transform.position; 
        Vector2 endPoint = new Vector2(randomX, randomY);

        float timePassed = 0f;

        while(timePassed < popDuration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / popDuration; // 선형 시간
            float heightT = animCurve.Evaluate(linearT); // 선형시간에 커브효과 주기

            float height = Mathf.Lerp(0f, heightY, heightT);

            // Mathf.Lerp: ~에서 ~까지 ~초동안 수렴
            transform.position = Vector2.Lerp(startPoint, endPoint, linearT) + new Vector2(0f, height);
            
            yield return null;
        }
    }

    private void DetectPickUpType()
    {
        switch(pickUpType)
        {
            case PickUpType.GoldCoin:
                // 1골드를 추가하고 UI를 업데이트하는 메서드
                EconomyManager.Instance.UpdateCurrentGold();
                break; 

            case PickUpType.healthGlobe:
                PlayerHealth.Instance.HealPlayer();
                break; 

            case PickUpType.StaminaGlobe:
                Stamina.Instance.RefreshStamina();
                break; 

            default:
                break;
        }
    }

    private void Update() 
    {
        Vector3 playerPos = PlayerController.Instance.transform.position;

        if(Vector3.Distance(transform.position, playerPos) < pickUpDistance)
        // 픽업아이템과 플레이어간의 거리가 자력 사거리 내에 있으면 코드 실행
        {
            moveDir = (playerPos - transform.position).normalized; // 방향벡터
            moveSpeed += acclartionRate;
        }

        else
        {
        // 자력 사거리를 벗어나면 가해지던 힘 X
            moveDir = Vector3.zero;
            moveSpeed = 0f;
        }
    }
    
    private void FixedUpdate() 
    {
        rb.velocity = moveDir * moveSpeed * Time.deltaTime;
        // rb에 속력 가해주기
    }
}
