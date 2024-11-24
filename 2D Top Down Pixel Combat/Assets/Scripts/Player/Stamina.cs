using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Stamina : Singleton<Stamina>
{
    public int CurrentStamina {get; private set; }

    [SerializeField] private Sprite fullStaminaImage, emptyStaminaImage;
    [SerializeField] private int timeBtweenStaminaRefresh = 3;

    const string STAMINA_CONTAINER_TEXT = "Stamina Container";

    private Transform staminaContainer;
    private int startingStamina = 3;
    private int maxStamina;

    protected override void Awake() 
    {
        base.Awake();

        maxStamina = startingStamina;
        CurrentStamina = startingStamina;
    }

    private void Start() 
    {
        staminaContainer = GameObject.Find(STAMINA_CONTAINER_TEXT).transform;
    }

    public void UseStamina()
    {
        CurrentStamina--;
        UpdateStaminaImages();
        StopAllCoroutines(); // 해당 클래스에서 실행되고있는 모든 코루틴 종료 -> 하나의 인스턴스만 실행
        StartCoroutine(RefreshStaminaRoutine());
    }

    public void RefreshStamina()
    {
        // 스테미나가 최대치를 넘지 않으면 회복
        if(CurrentStamina < maxStamina && !PlayerHealth.Instance.IsDead)
        {
            CurrentStamina++;
        }

        UpdateStaminaImages();
    }

    private IEnumerator RefreshStaminaRoutine()
    {
        // timeBtweenStaminaRefresh초마다 자동 스테미나 회복
        while(true)
        {
            yield return new WaitForSeconds(timeBtweenStaminaRefresh);
            RefreshStamina();
        }
    }

    public void RefreshStaminaOnDeath()
    {
        CurrentStamina = startingStamina;
        UpdateStaminaImages();
    }

    private void UpdateStaminaImages()
    {
        for (int i = 0; i < maxStamina; i++)
        {
            Transform child = staminaContainer.GetChild(i);
            Image image = child?.GetComponent<Image>();

            // 게임 오브젝트 child의 index는 0부터 시작이니까 -1 해준다.
            if(i <= CurrentStamina - 1)
            // 현재 가지고있는 스테미나만큼 채워져있는 이미지로 UI를 바꿔준다.
            {
                image.sprite = fullStaminaImage;
            }

            else
            // 최대 스테미나 수 - 현재 스테미나 수 해서 남은 칸은 비워져있는 이미지로 UI를 바꿔준다.
            {
                image.sprite = emptyStaminaImage;

            }
        }
    }
}
