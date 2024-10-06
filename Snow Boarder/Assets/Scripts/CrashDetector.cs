using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrashDetector : MonoBehaviour
{
    [SerializeField] float delay_time = 0.5f;
    [SerializeField] ParticleSystem crashEffect;
    [SerializeField] AudioClip crashSFX;
    bool flag = true;

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Ground" && flag)
        {
            flag = false;
            FindObjectOfType<PlayerController>().DisableControls();
            crashEffect.Play();
            GetComponent<AudioSource>().PlayOneShot(crashSFX);
            Invoke("ReloadScene", delay_time);
        }
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
}
