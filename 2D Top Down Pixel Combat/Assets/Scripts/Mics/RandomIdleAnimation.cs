using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomIdleAnimation : MonoBehaviour
{
    private Animator myAnimator;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    private void Start() 
    {
        // AnimatorStateInfo는 애니메이션의 현재 상태 정보를 담고있는 클래스
        // 애니메이터의 첫번째 애니메이션 스프라이트 -> 0
        // 즉, 시작 애니메이션 키프레임을 저장하는 느낌
        AnimatorStateInfo state = myAnimator.GetCurrentAnimatorStateInfo(0);

        // Play(애니메이션 이름: 전체 경로 해시 -> 아무거나), 첫번째 키프레임 부터, 애니메이션 시작시간)
        myAnimator.Play(state.fullPathHash, -1, Random.Range(0f,1f));
    }
}
