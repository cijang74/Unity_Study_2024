using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] QuestionSO question;
    [SerializeField] GameObject[] answerButtons;
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;
    Image buttonImage;

    int correctAnswerIndex;

    public void OnAnswerSelected(int index) // 정답에 따른 버튼 스프라이트 변화
    {
        if(index == question.GetCorrectAnswer())
        {
            questionText.text = "정답!";
            buttonImage = answerButtons[index].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
        }

        else
        {
            correctAnswerIndex = question.GetCorrectAnswer();
            string correctAnswer = question.GetAnswer(correctAnswerIndex);

            questionText.text = "땡!! 정답은..\n" + correctAnswer;

            buttonImage = answerButtons[correctAnswerIndex].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
        }
        SetButtonState(false); // 모든 선택이 끝나면 버튼 비활성화
        Invoke("GetNextQuestion", 5);
    }

    void Start()
    {
        DisplayQuestion();
    }

    void GetNextQuestion()
    {
        SetButtonState(true);
        SetDefaultButtonSprite();
        DisplayQuestion();
    }

    void DisplayQuestion() // 버튼 텍스트 변경
    {
        for(int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>(); // 자식 컴포넌트에서 텍스트 불러오기
            buttonText.text = question.GetAnswer(i);
        }
        questionText.text = question.GetQuetion(); // SO파일에서 불러온 메서드
    }

    void SetButtonState(bool state) // 버튼 활성화/비활성화
    {
        for(int i = 0; i < answerButtons.Length; i++)
        {
            Button button = answerButtons[i].GetComponent<Button>();
            button.interactable = state;
        }
    }

    void SetDefaultButtonSprite() // 각 버튼을 기존 스프라이트로 변경
    {
        for(int i = 0; i < answerButtons.Length; i++)
        {
            buttonImage = answerButtons[i].GetComponent<Image>();
            buttonImage.sprite = defaultAnswerSprite;
        }
    }
}
