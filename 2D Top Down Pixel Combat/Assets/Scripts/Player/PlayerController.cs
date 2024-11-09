using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private TrailRenderer myTrailRenderer;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;
    private float startingMoveSpeed; // 대쉬속도에서 원래 속도로 돌아가기 위한 변수

    private bool isDashing = false;
    private bool facingLeft = false;
    public bool FacingLeft { get {return facingLeft; } }
    // 위 생략방식은 아래 함수들과 같은 역할을 한다. 단, 참조 변수는 무조건 private이어야만 한다.
    // public bool GetFacingLeft()
    // {
    //     return facingLeft;
    // }

    // public void SetFacingLeft(bool value)
    // {
    //     facingLeft = value;
    // }

    protected override void Awake() // 싱글톤 클래스의 Awake()함수 재정의
    {
        base.Awake(); // 부모 클래스(싱클톤 클래스)의 Awake()함수 실행
        // 실행하면 PlayerController 클래스의 인스턴스를 생성함

        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // =>: 람다식, 연산자 왼쪽이 파라미터, 연산자 오른쪽이 실행문장
        // 즉, _(전달값 X)를 파라미터로하여 연산자 뒤 함수를 실행한 값을 덧붙여준다.
        playerControls.Combat.Dash.performed += _ => Dash();
        startingMoveSpeed = moveSpeed;
    }

    private void OnEnable() // 플레이어 컨트롤을 활성화 할 때 사용
    {
        playerControls.Enable();
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

    private void Dash() // 대쉬함수
    {
        if(isDashing == false)
        {
            isDashing = true;
            moveSpeed *= dashSpeed;
            myTrailRenderer.emitting = true; // Trail(대쉬 이펙트) 켜기
            StartCoroutine(EndDashRoutine());
        }
    }

    private IEnumerator EndDashRoutine()
    {
        // 대쉬를 사용하면 일정시간만 이동속도가 빨라지도록 하는 코루틴
        float dashTime = .2f; // 대쉬 지속시간
        float dashCD = .25f; // 대쉬 쿨타임
        yield return new WaitForSeconds(dashTime); // dashTime초 이후 다음 라인 실행
        moveSpeed = startingMoveSpeed;
        myTrailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCD); // DashCD초 이후 다음 라인 실행
        isDashing = false;
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
            facingLeft = true;
        }
        else
        {
            mySpriteRenderer.flipX = false; // 안뒤집기
            facingLeft = false;
        }
    }

    void Update()
    {
        PlayerInput(); // 애니컨트롤러에서 사용할 플레이어의 위치값 불러오기
    }
}
