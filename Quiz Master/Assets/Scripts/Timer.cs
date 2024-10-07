using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] float timeToCompleteQuestion = 30f;
    [SerializeField] float timeToShowCorrectAnswer = 10f;

    public bool isAnsweringQuestion = false; // 지금 검토시간인가?
    float timerValue = -1f;
    void Update()
    {
        UpdateTimer();
        Debug.Log(timerValue);
    }

    // 문제를 풀고있으면 30초 제공, 문제 검토시간은 10초 제공
    void UpdateTimer()
    {
        timerValue-=Time.deltaTime;

        if(!isAnsweringQuestion && timerValue <= 0)// 푸는시간이 아니었고, 타이머 끝
        {
            timerValue = timeToCompleteQuestion; // 다음 제공 시간 30초
            isAnsweringQuestion = true; // 다음은 푸는시간
        }

        else if (isAnsweringQuestion && timerValue <= 0) // 푸는시간이었고, 타이머 끝
        {
            timerValue = timeToShowCorrectAnswer; // 다음 제공 시간 10초
            isAnsweringQuestion = false; // 다음은 푸는시간 아님
        }
    }
}
