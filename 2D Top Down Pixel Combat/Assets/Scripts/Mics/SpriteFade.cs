using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFade : MonoBehaviour
{
    [SerializeField] private float fadeTime = .4f; // 레이저 잔상 시간

    private SpriteRenderer spriteRenderer;

    private void Awake() 
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public IEnumerator SlowFadeRoutine()
    {
        // MagicLaser스크립트에서 레이저가 다 확장되고 난 후 실행될 레이저 삭제 메서드

        float elapsedTime = 0; // 경과시간
        float startValue = spriteRenderer.color.a;

        while(elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime; // 경과시간 재기
            float newAlpha = Mathf.Lerp(startValue, 0f, elapsedTime / fadeTime);
            // startValue의 투명도에서 targeTransparency까지 (elapsedTime / fadeTime)초동안 수렴

            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, newAlpha);
            // 투명도 적용
            yield return null; // 딱히 기다렸다가 반환 안해도 됨.
        }

        Destroy(gameObject);
    }
}
