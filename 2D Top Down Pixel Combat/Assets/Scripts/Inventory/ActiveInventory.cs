using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveInventory : MonoBehaviour
// 인벤토리에서 무기 선택하면 하이라이트 표시를 활성화시키는 스크립트
{
    private int activeSlotIndexNum = 0; // 셀렉트 인덱스
    private PlayerControls playerControls;

    private void Awake() 
    {
        playerControls = new PlayerControls();
    }

    private void Start() 
    {
        playerControls.Inventory.Keyboard.performed += ctx => ToggleActiveSlot((int)ctx.ReadValue<float>());
        // ctx는 내가 누른 값의 value를 읽기위해 접근가능한 변수
        // 예를 들어 1을 누르면 ctx에 누른키가 저장이되고, ReadValue를 통해 반환시킬 수 있다.

        ToggleActiveHighlight(0); // 기본 선택된 무기는 인벤토리 슬롯의 0번째 무기
    }

    private void OnEnable() 
    {
        playerControls.Enable();
        // 플레이어가 존재하는지 확인
    }

    private void ToggleActiveSlot(int numValue)
    // 선택한 키 넘버에 따라 무기 변경
    {
        ToggleActiveHighlight(numValue - 1); // 인덱스와 실제 수 차이 -1
    }

    private void ToggleActiveHighlight(int indexNum)
    // 선택한 키 넘버에 따라 하이라이트 이미지 활성화
    {
        activeSlotIndexNum = indexNum;

        foreach (Transform inventorySlot in this.transform)
        // for(inventorySlot[i]; i < inventorySlot.Length; i++)과 같은 역할을 하는 C#반복문
        // 이 스크립트가 적용된 오브젝트의 자식(this.transform에는 자식 정보가 포함됨)에 접근
        {
            inventorySlot.GetChild(0).gameObject.SetActive(false);
            // 객체의 첫번째 자식 오브젝트를 비활성화
        }

        this.transform.GetChild(indexNum).GetChild(0).gameObject.SetActive(true);
        // 해당 객체의 indexNum번째 자식의 첫번째 자식(하이라이트 이미지)를 활성화

        ChangeActiveWeapon();
    }

    private void ChangeActiveWeapon()
    {
        if(ActiveWeapon.Instance.CurrentActiveWeapon != null)
        // 현재 선택중인 무기를 삭제하는 제어문. 항상 가장 상단에 위치시켜놓아야함.
        {
            Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);
        }

        // GameObject weaponToSpawn = transform.GetChild(activeSlotIndexNum).GetComponent<InventorySlot>().GetWeaponInfo().weaponPrefab; 
        // 해당 스크립트가 적용된 오브젝트의 activeSlotIndexNum번째 자식오브젝트 -> InventorySlot n
        // InventorySlot의 GetWeaponInfo()함수 실행 -> WeaponInfo리턴 -> Weaponprefab 접근

        // 위 주석 내용을 짧게 줄인 코드가 아래 코드
        Transform childTransform = transform.GetChild(activeSlotIndexNum);
        InventorySlot inventorySlot = childTransform.GetComponentInChildren<InventorySlot>();
        WeaponInfo weaponInfo = inventorySlot.GetWeaponInfo();

        if(weaponInfo == null)
        // 만약 인벤토리 안에 담겨있는 무기가 존재하지 않는다면
        {
            ActiveWeapon.Instance.WeaponNull(); // CurrentActiveWeapon를 null로 만들어줌
            return;
            // 아래 라인들 실행되지 않도록 리턴해버림
        }

        GameObject weaponToSpawn = weaponInfo.weaponPrefab; // <- 강의랑 다르게 내가 조정한 부분

        GameObject newWeapon = Instantiate(weaponToSpawn, ActiveWeapon.Instance.transform.position, Quaternion.identity);
        // ActiveWeapon오브젝트의 위치로 weaponToSpawn프리펩을 인스턴스화
        ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0,0,0);
        // 쿼터니언 초기화

        newWeapon.transform.parent = ActiveWeapon.Instance.transform;
        // 인스턴스화한 weaponToSpawn프리펩의 부모를 ActiveWeapon오브젝트로 설정

        ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());
    }
}
