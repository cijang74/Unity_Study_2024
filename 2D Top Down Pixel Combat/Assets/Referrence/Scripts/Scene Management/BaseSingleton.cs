using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSingleton : Singleton<BaseSingleton>
{
    // Singleton 클래스는 다른 클래스에서 상속받아 인스턴스를 생성시켜주는 역할.
    // 즉, 오브젝트에 직접적으로 Singleton클래스를 컴포넌트로 줄 수 없다.
    // 따라서 해당 클래스에서 싱글톤클래스를 상속받아 해당 스크립트를 적용시킨
    // 오브젝트를 인스턴스화 시켜준다.
}
