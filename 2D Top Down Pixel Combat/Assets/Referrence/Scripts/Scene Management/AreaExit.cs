using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{
    [SerializeField] private string sceneToLoad; // 불러올 씬
    [SerializeField] private string sceneTransitionName;
    // 다음씬에서 어느 출구로 스폰? ex) 오른쪽 출구..

    private float waitRoLoadTime = 1f; // 씬 불러올때 시간 얼마나 줄것인지

    private void OnTriggerEnter2D(Collider2D other) // 트리거 충돌 감지
    {
        if(other.gameObject.GetComponent<PlayerController>()) // 플레이어?
        {
            UIFade.Instance.FadeToBlack();
            // 화면 검은색으로 페이드아웃 시키기

            SceneManagement.Instance.SetTransitionName(sceneTransitionName);
            // 씬 메니저에 어느쪽으로 나오게 되는지 전달 (ex. 오른쪽 출구)

            StartCoroutine(LoadSceneRoutine());
        }
    }

    private IEnumerator LoadSceneRoutine()
    {
        // 페이드아웃할 시간 1초 기다려 주고 씬 불러오도록 해주는 코루틴
        // while(waitRoLoadTime >= 0)
        // {
        //     waitRoLoadTime -= Time.deltaTime;
        //     yield return null;
        // }

        yield return new WaitForSeconds(waitRoLoadTime);

        SceneManager.LoadScene(sceneToLoad); // 씬 불러오기
    }
}
