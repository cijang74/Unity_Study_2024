using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    protected int hp = 10;  //몬스터 hp
    public UnityEvent DieEvent;
    public virtual void Damage(int damage)      //플레이어 공격한 함수 끝에 .Damage(20) 이런 식으로 활용
    {
        hp -= damage;
    }
    protected virtual void Die()
    {
        DieEvent.Invoke();
        this.gameObject.SetActive(false);
    }
}
