using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Singleton<PlayerHealth>
{
    // 플레이어의 HP와 피격 관련 스크립트

    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockBackTrustAmount = 10f;
    [SerializeField] private float DamageRecoveryTime = 1f;

    const string  HEALTH_SLIDER_TEXT= "Health Slider";

    private int currentHealth;

    private Slider heathSlider;
    private Knockback knockback;
    private Flash flash;
    private bool canTakeDamage = true;
    
    protected override void Awake() 
    {
        base.Awake();

        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
    }

    private void Start() 
    {
        currentHealth = maxHealth;
        UpdateHealthSlider();
    }

    private void OnCollisionStay2D(Collision2D other)
    // 플레이어와 other이 계속 닿고있다면 (OncollisionEnter2D쓰면 부비부비해도 1번만 피격)
    {
        EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();

        if(enemy)
        {
            TakeDamage(1, other.transform);
        }
    }

    public void HealPlayer()
    {
        if(currentHealth < maxHealth)
        {
            currentHealth++;

            // UI 업데이트
            UpdateHealthSlider();
        }
    }

    public void TakeDamage(int damageAmount, Transform hitTransform)
    // 데미지양과 데미지를 입힌 오브젝트의 위치를 입력받음
    {
        if(!canTakeDamage == true)
        // 만약 무적시간이면 넉백과 데미지 받지않도록 아래 함수들 무시
        {
            return;
        }

        // 카메라 흔들림
        ScreenShakeManager.Instance.ShakeScreen();

        // 플레이어에게 넉백과 플래시
        knockback.GetKnockedBack(hitTransform, knockBackTrustAmount);
        StartCoroutine(flash.FlashRoutine());

        // 플레이어에게 데미지를 주는 함수
        canTakeDamage = false;
        currentHealth -= damageAmount;
        StartCoroutine(DamageRecoveryRoutine());

        // 플레이어 죽는피인지 체크
        CheckIfPlayerDeath();

        // UI 업데이트
        UpdateHealthSlider();
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        // 한프레임에 연속 피격받는것을 방지하기 위한 코루틴
        yield return new WaitForSeconds(DamageRecoveryTime);
        canTakeDamage = true;
    }

    private void UpdateHealthSlider()
    // 체력 UI 연결하여 업데이트
    {
        if(heathSlider == null)
        {
            heathSlider = GameObject.Find(HEALTH_SLIDER_TEXT).GetComponent<Slider>();
        }

        heathSlider.maxValue = maxHealth;
        heathSlider.value = currentHealth;
    }

    private void CheckIfPlayerDeath()
    {
        if(currentHealth <= 0)
        {
            currentHealth = 0; // UI 슬라이더가 음수방향으로 뚫리지 않게
            Debug.Log("Player Death");
        }
    }
}
