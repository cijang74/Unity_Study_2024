using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{
    [SerializeField] private string sceneToLoad; // 불러올 씬
    [SerializeField] private string sceneTransitionName;
    // 다음씬에서 어느 출구로 스폰? ex) 오른쪽 출구..

    private void OnTriggerEnter2D(Collider2D other) // 트리거 충돌 감지
    {
        if(other.gameObject.GetComponent<PlayerController>()) // 플레이어?
        {
            SceneManagement.Instance.SetTransitionName(sceneTransitionName);
            // 씬 메니저에 어느쪽으로 나오게 되는지 전달 (ex. 오른쪽 출구)
            SceneManager.LoadScene(sceneToLoad); // 씬 불러오기
        }
    }
}
