using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepAudioController : MonoBehaviour
{
    private CharacterController player;
    private PlayerController playerStatus;
    private AudioSource footstepAudio;

    void Start()
    {
        player = gameObject.GetComponentInParent<CharacterController>();
        playerStatus = gameObject.GetComponentInParent<PlayerController>();
        footstepAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if ((Input.GetKey(KeyCode.A) || 
        Input.GetKey(KeyCode.W) || 
        Input.GetKey(KeyCode.S) || 
        Input.GetKey(KeyCode.D))
        && player.isGrounded
        && playerStatus.alive
        && !playerStatus.win)
        {
            footstepAudio.enabled = true;
        }
        else
        {
            footstepAudio.enabled = false;
        }
    }
}
