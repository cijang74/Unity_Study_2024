using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    private Image image;

    private void Awake() 
    {
        image = GetComponent<Image>();
    }

    void Start()
    {
        Cursor.visible = false; // 운영체제의 기본 마우스 아이콘 시각 비활성화

        if(Application.isPlaying) // 유니티 에디터에서 실행하면 True 반환
        {
            Cursor.lockState = CursorLockMode.None; // 잠금 비활성화
        }

        else
        {
            Cursor.lockState = CursorLockMode.Confined; // 커서가 게임 화면 밖으로 나가지 않게함
        }
    }

    void Update()
    {
        Vector2 cursorPos = Input.mousePosition; // 커서 위치
        image.rectTransform.position = cursorPos;

        if(!Application.isPlaying)
        {
            // 플레이중이면 Cursor.visible = false; 라인 실행 안되도록 리턴
            return;
        }

        Cursor.visible = false;
    }
}
