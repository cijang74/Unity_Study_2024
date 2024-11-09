using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TransparentDetection : MonoBehaviour
// 나무 등 배경 뒤로 플레이어가 이동하면 배경을 약간 투명하게 만들어 플레이어를 보이게 하는 스크립트
{
    [Range(0, 1)] // 0~1만큼의 스크를바를 만들어줌
    [SerializeField] private float transparencyAmount = 0.8f;
    [SerializeField] private float fadeTime = .4f;

    private SpriteRenderer spriteRenderer;
    private Tilemap tilemap;
    
    private void Awake() 
    {
        // GetComponent했을때 안 찾아지면 해당 코드는 무효화됨 (NULL값)
        spriteRenderer = GetComponent<SpriteRenderer>();
        tilemap = GetComponent<Tilemap>();
    }

    private void OnTriggerEnter2D(Collider2D other) // 충돌 감지 
    {
        if(other.gameObject.GetComponent<PlayerController>())
        // 만약 충돌한놈이 PlayerController 컴포넌트를 가진놈이면
        {
            if(spriteRenderer) // 만약 스프라이트렌더러가 존재하면
            {
                StartCoroutine(FadeRoutine(spriteRenderer, fadeTime, spriteRenderer.color.a, transparencyAmount));        
            }

            else if (tilemap)
            {
                StartCoroutine(FadeRoutine(tilemap, fadeTime, tilemap.color.a, transparencyAmount));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.gameObject.GetComponent<PlayerController>())
        // 만약 충돌한놈이 PlayerController 컴포넌트를 가진놈이면
        {
            if(spriteRenderer) // 만약 스프라이트렌더러가 존재하면
            {
                StartCoroutine(FadeRoutine(spriteRenderer, fadeTime, spriteRenderer.color.a, 1f));        
            }

            else if (tilemap)
            {
                StartCoroutine(FadeRoutine(tilemap, fadeTime, tilemap.color.a, 1f));
            }      
        }
    }

    private IEnumerator FadeRoutine(SpriteRenderer spriteRenderer, float fadeTime, float startValue, float targeTransparency)
    {
        // 코루틴 사용이유: 병렬적으로 해당 함수를 사용하기 위해서. 
        // (그냥 함수쓰면한프레임만에 계산 다되어버림)
        // fadeTime: 페이드아웃 지속시간, targeTransparency: 투명도
        float elapsedTime = 0; // 경과시간

        while(elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime; // 경과시간 재기
            float newAlpha = Mathf.Lerp(startValue, targeTransparency, elapsedTime / fadeTime);
            // startValue의 투명도에서 targeTransparency까지 (elapsedTime / fadeTime)초동안 수렴

            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, newAlpha);
            // 투명도 적용
            yield return null; // 딱히 기다렸다가 반환 안해도 됨.
        }
    }

    private IEnumerator FadeRoutine(Tilemap tilemap, float fadeTime, float startValue, float targeTransparency)
    {
        // 코루틴 사용이유: 병렬적으로 해당 함수를 사용하기 위해서. 
        // (그냥 함수쓰면한프레임만에 계산 다되어버림)
        // fadeTime: 페이드아웃 지속시간, targeTransparency: 투명도
        float elapsedTime = 0; // 경과시간

        while(elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime; // 경과시간 재기
            float newAlpha = Mathf.Lerp(startValue, targeTransparency, elapsedTime / fadeTime);
            // startValue의 투명도에서 targeTransparency까지 (elapsedTime / fadeTime)초동안 수렴

            tilemap.color = new Color(tilemap.color.r, tilemap.color.g, tilemap.color.b, newAlpha);
            // 투명도 적용
            yield return null; // 딱히 기다렸다가 반환 안해도 됨.
        }
    }
}
