using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEnterance : MonoBehaviour
{
    // 새로운 씬을 로드하였을 때 플레이어 위치를 재정의하는 스크립트
    // 이전 씬에서 새로운 씬으로 일부 데이터를 계승해야함.
    // 예를들어, 오른쪽 출구로 나가면 왼쪽 입구로 들어가게 만들어야함.
    // 그러기 위해서는 '어디로 연결되는' 출구를 이용하였는지를 전달할 필요가 있음.

    [SerializeField] private string transitionName;
    // 출구에서 입구로 다시 돌아올 때를 대비해 저장하는 함수.
    // 내가 이용할 입구를 저장하면 됨.

    private void Start()
    {
        if(transitionName == SceneManagement.Instance.SceneTransitionName)
        // sceneTransitionName(=sceneTransitionName)
        // 내가 이용할 입구가 다음씬에서 마지막으로 사용된 출구라면
        {
            PlayerController.Instance.transform.position = transform.position;
            // 플레이어의 위치는 AreaEnterance가 적용된 객체의 위치와 같다.
        }
    }

}
