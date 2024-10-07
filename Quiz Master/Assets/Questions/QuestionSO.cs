using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quiz Question", fileName = "New Question")]
public class QuestionSO : ScriptableObject
{
    [TextArea(2,6)]
    [SerializeField] string question = "질문을 여기에 입력해주세요";
    [SerializeField] string[] answers = new string[4];
    [SerializeField] int correctAnswerIndex;
    int answersIndex;

    public string GetQuetion()
    {
        return question;
    }

    public int GetCorrectAnswer() // 해당 질문의 정답 인덱스 반환
    {
        return correctAnswerIndex;
    }

    public string GetAnswer(int index) // 정답 문자열을 반환
    {
        return answers[index];
    }
}
