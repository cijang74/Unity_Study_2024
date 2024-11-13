using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFllow : MonoBehaviour
{
    private void FaceMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        // 실제 마우스 위치를 입력받음
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        // 게임 내 화면 마우스 위치로 변환
        
        Vector2 direction = transform.position - mousePosition;
        // 내 마우스 위치 - 캐릭터의 위치 => 마우스와 캐릭터에 대한 방향벡터

        transform.right = -direction;
    }

    private void Update()
    {
        FaceMouse();
    }
}
