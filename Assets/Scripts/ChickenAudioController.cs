using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenAudioController : MonoBehaviour
{
    public ChickenController chicken;
    private AudioSource chickenAudio;
    public GameObject chasedAudio;
    public CheckPointManager checkPoint;
    public GameObject assignedCheckPoint;


    void Start()
    {
        chicken = gameObject.GetComponentInParent<ChickenController>();
        chickenAudio = GetComponent<AudioSource>();

        chickenAudio.enabled = false;
        chasedAudio.SetActive(false);
    }

    void Update()
    {

        if (checkPoint.currentCheckpoint == assignedCheckPoint)
        {
            chickenAudio.enabled = true;

            if (chicken.isFrozen)
            {
                chickenAudio.enabled = false;
                chasedAudio.SetActive(false);
            }
            else if (!chicken.isFrozen && !chicken.isChased)
            {
                chasedAudio.SetActive(false);
                chickenAudio.enabled = true;
            }
            else if (!chicken.isFrozen && chicken.isChased)
            {
                chasedAudio.SetActive(true);
                chickenAudio.enabled = false;
            }
        }
    }
}
