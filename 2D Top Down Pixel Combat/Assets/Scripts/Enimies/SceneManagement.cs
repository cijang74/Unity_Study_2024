using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagement : Singleton<SceneManagement>
{
    // 한 맵에서 출구가 여러개 있을 수 있음.
    // 각기 다른 출구에 따라 불러올 씬이 달라야 하므로 이를 구현하도록 하는 스크립트

    public string SceneTransitionName { get; private set; }

    public void SetTransitionName(string sceneTransitionName)
    {
        // 전환 씬의 이름을 입력받아 저장
        this.SceneTransitionName = sceneTransitionName;
    }

    protected override void Awake() // 싱글톤 클래스의 Awake()함수 재정의
    {
        base.Awake(); // 부모 클래스(싱클톤 클래스)의 Awake()함수 실행
        // 실행하면 PlayerController 클래스의 인스턴스를 생성함
    }
}
