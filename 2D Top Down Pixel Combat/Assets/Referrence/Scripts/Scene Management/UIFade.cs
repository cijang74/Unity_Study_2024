using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : Singleton<UIFade>
{
    [SerializeField] private Image fadeScreen;
    [SerializeField] private float fadeSpeed = 1f;

    private IEnumerator fadeRoutine;

    public void FadeToBlack()
    {
        if(fadeRoutine != null) // 현재 실행중인 코루틴이 있다면
        {
            StopCoroutine(fadeRoutine); // 현재 실행중인d 코루틴 중지
        }

        fadeRoutine = FadeRoutine(1);
        StartCoroutine(fadeRoutine);
    }

    public void FadeToClear()
    {
        if(fadeRoutine != null) // 현재 실행중인 코루틴이 있다면
        {
            StopCoroutine(fadeRoutine); // 해당 코루틴 중지
        }

        fadeRoutine = FadeRoutine(0);
        StartCoroutine(fadeRoutine);
    }

    private IEnumerator FadeRoutine(float targetAlpha)
    {
        while(!Mathf.Approximately(fadeScreen.color.a, targetAlpha))
        // fadescreen.color.a와 targetAlpha의 값이 같아지면 True를 반환, !했으니 같아지면 반복문 종료
        {
            float alpha = Mathf.MoveTowards(fadeScreen.color.a, targetAlpha, fadeSpeed * Time.deltaTime);
            // fadeScreen.color.a값을 targetAlpha값이 될때까지 fadeSpeed * Time.deltaTime만큼 빼거나 더한다.
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, alpha);
            yield return null;
        }
    }
}
