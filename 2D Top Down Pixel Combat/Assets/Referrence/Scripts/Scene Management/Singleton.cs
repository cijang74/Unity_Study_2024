using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
// 다른 클래스에서 해당 클래스를 제너릭으로 접근할수 있도록 만들어주는 용도.
// T(클래스 종류)는 싱글톤 클래스를 상속하는 클래스로 적용할 수 있다.
{
    private static T instance;
    public static T Instance {get { return instance; } } // 상속받은 클래스에서 접근할 수 있도록

    protected virtual void Awake()
    {
        // 해당 인스턴스가 속한 모노비헤이버 클래스에 할당

        if(instance != null && this.gameObject != null)
        // 만약 인스턴스와 게임오브젝트가 null이 아니라면 -> 이미 같은 인스턴스가 존재한다는 뜻.
        {
            // 중복되지 않게 해당 인스턴스 삭제
            Destroy(this.gameObject);
        }

        else // 인스턴스가 존재하지 않으면 인스턴스 생성
        {
            instance = (T)this;
        }

        if(!gameObject.transform.parent) // 인스턴스의 부모 트랜스폼이 없는경우 (자신이 부모일경우)
        {
            DontDestroyOnLoad(gameObject); // 새로운 씬 로드할 때 해당 인스턴스 삭제하지 마시오
        }
    }
}
