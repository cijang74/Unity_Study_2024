using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;

    void Awake() // 초기화, 해당 스크립트가 적용된 컴포넌트 정보 불러오기
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable() // 플레이어 컨트롤을 활성화 할 때 사용
    {
        playerControls.Enable();
    }

    void Update()
    {
        PlayerInput(); // 애니컨트롤러에서 사용할 플레이어의 위치값 불러오기
    }

    private void FixedUpdate() // 물리나 플레이어 입력에 주로 사용되는 고정 업데이트
    {
        AdjustPlayerFacingDirection(); // 마우스 포인터 위치에 따라 플레이어 방향 전환
        Move(); // 플레이어의 위치 변경
    }

    private void PlayerInput() // 애니컨트롤러에서 사용할 플레이어의 위치값 불러오기
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();
        // PlayerControls InputActions에서 설정되어있는 Movemet액션맵의 Move액션의 변수값을 읽어온다.

        myAnimator.SetFloat("moveX", movement.x); //Animator 컴포넌트의 "MoveX" 파라메터값 변경
        myAnimator.SetFloat("moveY", movement.y);
    }

    private void Move() // 플레이어의 위치 변경
    {
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime)); 
        // 움직이려는 객체의 최근 포지션 + (이동벡터값 * (이동속도 * 프레임))
    }

    private void AdjustPlayerFacingDirection() // 마우스 포인터 위치에 따라 플레이어 방향 전환
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
        // 카메라 기준으로의 플레이어의 위치값을 선정하여 저장

        if(mousePos.x < playerScreenPoint.x) // 마우스 x좌표값이 플레이어의x좌표값보다 작다면
        {
            mySpriteRenderer.flipX = true; // 뒤집기
        }
        else
        {
            mySpriteRenderer.flipX = false; // 안뒤집기
        }
    }
}
