using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    private ParticleSystem ps;

    private void Awake() 
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void Update() 
    {
        if(ps && !ps.IsAlive()) // 만약 파티클시스템이 존재하고, 그것이 또한 실행중이 아니라면
        {
            DestroySelfAnimEvent(); // 해당 스크립트가 적용되어있는 오브젝트를 삭제
        }    
    }

    public void DestroySelfAnimEvent()
    {
        Destroy(gameObject);
    }
}
