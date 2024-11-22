using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 3;
    [SerializeField] private GameObject deathVFXPrefab;
    [SerializeField] private float knockbackThrust = 15f; // 넉벡량

    private int currentHealth;
    private Knockback knockback;
    private Flash flash;

    private void Awake()
    {
        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
    }

    private void Start()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage(int damage)
    {
        // 데미지를 입었을 때 실행되는 함수
        currentHealth -= damage;
        knockback.GetKnockedBack(PlayerController.Instance.transform, knockbackThrust); // 넉벡시키기
        StartCoroutine(flash.FlashRoutine()); // Flash시키기
        StartCoroutine(CheckDetectorDeathRoutime()); // 죽는피인지 검사하기
    }

    private IEnumerator CheckDetectorDeathRoutime()
    {
        // GetRestoreMatTime초 있다가 삭제하기 위해 만들어놓은 코루틴
        yield return new WaitForSeconds(flash.GetRestoreMatTime());
        DetectDeath();
    }

    private void DetectDeath()
    {
        // 죽음을 검사하고 죽을때 실행되는 함수
        if(currentHealth <= 0)
        {
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity); // 해당 위치로 인스턴스화
            GetComponent<PickUpSpawner>().DropItems();
            Destroy(gameObject);
        }
    }
}
