using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    [SerializeField] float delay_time = 1f;
    [SerializeField] ParticleSystem finishEffect;

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            finishEffect.Play();
            Invoke("ReloadScene", delay_time);
        }
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
}
