using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EconomyManager : Singleton<EconomyManager>
{
    private TMP_Text goldText;
    private int currentGold = 0;

    // const: #define이랑 같은 의미
    const string COIN_AMOUNT_TEXT = "Gold Amount Text";

    protected override void Awake() 
    {
        base.Awake();
    }

    public void UpdateCurrentGold()
    {
        currentGold += 1;

        if(goldText == null)
        {
            goldText = GameObject.Find(COIN_AMOUNT_TEXT).GetComponent<TMP_Text>();
        }

        // D3를 인자값으로 주면 무조건 세 글자 이상이 포함되도록 함.
        // ex. 1 -> 001
        goldText.text = currentGold.ToString("D3");
    }
}
