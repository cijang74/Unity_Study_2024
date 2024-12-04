using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private TrailRenderer myTrailRenderer;
    [SerializeField] private Transform weaponCollider;
    [SerializeField] private float jumpSpeed = 10f;

    private Knockback knockback;
    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private CapsuleCollider2D myCapusleCollider;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;
    private float startingMoveSpeed; // 대쉬속도에서 원래 속도로 돌아가기 위한 변수
    private Vector2 setGravity;

    private bool isDashing = false;
    private bool isJump = false;
    private bool canDoubleJump;
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

    readonly int JUNP_HASH = Animator.StringToHash("Death");

    protected override void Awake() // 싱글톤 클래스의 Awake()함수 재정의
    {
        base.Awake(); // 부모 클래스(싱클톤 클래스)의 Awake()함수 실행
        // 실행하면 PlayerController 클래스의 인스턴스를 생성함

        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myCapusleCollider = GetComponent<CapsuleCollider2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        knockback =  GetComponent<Knockback>();
    }

    private void Start()
    {
        // =>: 람다식, 연산자 왼쪽이 파라미터, 연산자 오른쪽이 실행문장
        // 즉, _(전달값 X)를 파라미터로하여 연산자 뒤 함수를 실행한 값을 덧붙여준다.
        playerControls.Combat.Dash.performed += _ => Dash();
        playerControls.Movement.Jump.performed +=_=> OnJump();

        startingMoveSpeed = moveSpeed;
        ActiveInventory.Instance.EquitStartingWeapon();
    }

    private void OnEnable() // 해당 조건을 만족해야 해당 스크립트 기능 사용 가능
    {
        playerControls.Enable(); // playerControls이 존재하는지 확인 (플레이어가 존재하는지)
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void FixedUpdate() // 물리나 플레이어 입력에 주로 사용되는 고정 업데이트
    {
        AdjustPlayerFacingDirection(); // 마우스 포인터 위치에 따라 플레이어 방향 전환
        Move(); // 플레이어의 위치 변경
    }

    public Transform GetWeaponCollider()
    {
        return weaponCollider;
    }

    private void PlayerInput() // 애니컨트롤러에서 사용할 플레이어의 위치값 불러오기
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        // PlayerControls InputActions에서 설정되어있는 Movemet액션맵의 Move액션의 변수값을 읽어온다.
        myAnimator.SetFloat("moveX", movement.x); //Animator 컴포넌트의 "MoveX" 파라메터값 변경
        //myAnimator.SetFloat("moveY", movement.y);
    }

    private void Dash() // 대쉬함수
    {
        if(isDashing == false && Stamina.Instance.CurrentStamina > 0)
        {
            Stamina.Instance.UseStamina();
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

    void OnJump() // 점프함수
    {  
        if(isJump == false || canDoubleJump)
        {
            //jump 애니메이션 추가
            isJump = true;
            myAnimator.SetBool("isJump", true);

            if(myCapusleCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) // 1단점프
            {
                rb.velocity += new Vector2(0f, jumpSpeed);
                canDoubleJump = true;
                Debug.Log("canDoubleJump: " + canDoubleJump); //디버깅
            }

            else if(canDoubleJump) // 2단점프
            {
                rb.velocity *= new Vector2(1f, 0f);
                rb.velocity += new Vector2(0f, jumpSpeed);
                canDoubleJump = false;
            }

            //jump 애니메이션 중지
            StartCoroutine(ResetJumpTriggerAfterDelay(1.0f));
        }
        isJump = false;
    }

    // 점프를 누르면 1초만 재생
    private IEnumerator ResetJumpTriggerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        myAnimator.SetBool("isJump", false);
    }

    private void Move() // 플레이어의 위치 변경 (FixedUpdate에서 실행됨)
    {

        if(knockback.GettingKnockedBack || PlayerHealth.Instance.IsDead)
        {
            // 넉벡중, 죽었으면 이동 씹히게 할것. 안씹히게 해 놓으면 밑 코드가 넉벡을 씹음.
            return;
        }

        Vector2 playerVelocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);
        rb.velocity = playerVelocity;
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
