using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapeProjectile : MonoBehaviour
{
    // z축으로 낙하하는 포도 탄막 구현 함수
    [SerializeField] private float duration = 1f; // 포도 탄막이 공중에 떠있을 시간
    [SerializeField] private AnimationCurve animCurve; // 부드러운 곡션 애니메이션 구현용 컴포넌트
    [SerializeField] private float heightY = 3f;
    [SerializeField] private GameObject grapeProjectileShadow; 
    [SerializeField] private GameObject splatterPrefab; 

    void Start()
    {
        GameObject grapeShadow = Instantiate(grapeProjectileShadow, transform.position + new Vector3(0, -0.3f, 0), Quaternion.identity);
        
        Vector3 playerPos = PlayerController.Instance.transform.position;
        Vector3 grapeShadowStartPosition = grapeShadow.transform.position;

        StartCoroutine(ProjectileCurveRoutine(transform.position, playerPos));
        StartCoroutine(MoveGrapeShadowRoutine(grapeShadow, grapeShadowStartPosition, playerPos));
    }

    private IEnumerator ProjectileCurveRoutine(Vector3 startPosition, Vector3 endPosition)
    {
        float timePassed = 0f;

        while(timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / duration; // 선형 시간
            float heightT = animCurve.Evaluate(linearT); // linearT시간동안의 선형 시간에 커브효과 주기
            float height = Mathf.Lerp(0f, heightY, heightT);

            // Mathf.Lerp: ~에서 ~까지 ~초동안 수렴
            transform.position = Vector2.Lerp(startPosition, endPosition, linearT) + new Vector2(0f, height);

            yield return null;
        }

        Instantiate(splatterPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private IEnumerator MoveGrapeShadowRoutine(GameObject grapeShadow, Vector3 startPosition, Vector3 endPosition)
    {
        float timePassed = 0f;

        while(timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / duration; // 선형 시간
            // Mathf.Lerp: ~에서 ~까지 ~초동안 수렴
            grapeShadow.transform.position = Vector2.Lerp(startPosition, endPosition, linearT);
            
            yield return null;
        }

        Destroy(grapeShadow);
    }
}
