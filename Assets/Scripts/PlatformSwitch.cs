using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlatformSwitch : MonoBehaviour
{
    public PlatfromMovementControl controllingPlatform;
    public GameObject poofEffect;
    public AudioSource hitAudio;

    void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Attack") && !controllingPlatform.moving)
        {

            hitAudio.Play();

            GameObject effect = Instantiate(poofEffect, transform.position, transform.rotation);
            gameObject.SetActive(false);
            // Destroy(gameObject);

            Destroy(effect.gameObject, 3f);

            controllingPlatform.TargetNextPoint();
            controllingPlatform.moving = true;
        }    
    }
}
