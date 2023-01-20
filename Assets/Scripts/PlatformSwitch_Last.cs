using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSwitch_Last : MonoBehaviour
{
    public PlatfromMovementControl_Last lastPlatform;
    public GameObject poofEffect;
    public AudioSource hitAudio;

    void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Attack") && !lastPlatform.moving)
        {

            hitAudio.Play();

            GameObject effect = Instantiate(poofEffect, transform.position, transform.rotation);
            gameObject.SetActive(false);

            Destroy(effect.gameObject, 3f);

            lastPlatform.TargetNextPoint();
            lastPlatform.moving = true;

        }    
    }
}
