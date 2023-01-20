using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public PlayerController player;
    public UIController ui;
    public GameObject currentCheckpoint;
    public GameObject springStart;
    public GameObject springEnd;
    public GameObject springSwitch;
    public GameObject summerStart;
    public GameObject summerEnd;
    public GameObject summerSwitch;
    public GameObject autumnStart;
    public GameObject autumnEnd;
    public GameObject autumnSwitch;
    public GameObject winterStart;
    public AudioClip stageStartAudio;
    private AudioSource audioSource;

    void Start()
    {
        currentCheckpoint = springStart;
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(stageStartAudio);
    }

    void Update()
    {

        if (player.chickenEaten == player.chickenToEat)
        {
            if (currentCheckpoint == springStart)
            {
                currentCheckpoint = springEnd;
                springSwitch.SetActive(true);
            }
            if (currentCheckpoint == summerStart)
            {
                currentCheckpoint = summerEnd;
                summerSwitch.SetActive(true);
            }
            if (currentCheckpoint == autumnStart)
            {
                currentCheckpoint = autumnEnd;
                autumnSwitch.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("CheckPoint"))
        {
            currentCheckpoint = other.gameObject;
            other.gameObject.SetActive(false);

            string stageName = currentCheckpoint.name.Replace("CheckPoint", "");
            stageName = stageName.Replace("Start", "");
            stageName = stageName.Replace("End", "");

            ui.SetStageText(stageName);
            audioSource.PlayOneShot(stageStartAudio);
            
            player.ChickenDisplay();
        }  
    }
}
