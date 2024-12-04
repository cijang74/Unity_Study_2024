using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [SerializeField] private Material redFlashMat;
    [SerializeField] private float restoreDeraultMatTime = .2f; // 기본 머티리얼로 돌아가기까지의 시간

    private SpriteRenderer spriteRenderer;
    private Material defaultMat;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMat = spriteRenderer.material;
    }

    public float GetRestoreMatTime()
    {
        return restoreDeraultMatTime;
    }

    public IEnumerator FlashRoutine()
    {
        spriteRenderer.material = redFlashMat;
        yield return new WaitForSeconds(restoreDeraultMatTime);
        spriteRenderer.material = defaultMat;
    }
}
