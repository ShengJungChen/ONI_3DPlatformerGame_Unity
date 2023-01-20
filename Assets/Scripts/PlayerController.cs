using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerController : MonoBehaviour
{
    // VARIABLE
    [Header("Movement")]
    public float speed;
    public AudioClip jumpAudio;
    public float jumpHeight;
    public float gravityMultiplier;
    private float fallSpeed;
    private float gravity = Physics.gravity.y;

    [Header("Attack")]
    public GameObject firePoint;
    public GameObject attackEffect;
    public GameObject poofEffect;
    public AudioClip attackAudio;


    [Header("Chicken")]
    public int chickenTotal;
    public int chickenToEat = 5;
    public int chickenEaten = 0;
    public AudioClip chickenEatenAudio;
    public bool win
    {
        get
        {
            return chickenEaten == chickenTotal;
        }
    }
    private string timeUsed;
    private float startTime;

    [Header("Energy")]
    public EnergyBar energyBar;
    public float startEnergy;
    public float energy;
    public float energyBoost;
    public bool alive
    {
        get
        {
            return energy > 0;
        }
    }
    public AudioClip deadAudio;
    public AudioClip loseAudio;
    public AudioClip winAudio;

    [Header("Respawn")]
    public CheckPointManager respawnManager;
    public float respawnHeight = 2f;
    public AudioClip respawnAudio;


    // REFERNCE
    private CharacterController controller;
    private Animator animator;
    private GameObject ui;
    private AudioSource audioSource;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();

        ui = GameObject.Find("Canvas");
        energy = startEnergy;
        energyBar.SetMaxHealth(energy);
        ChickenDisplay();

        startTime = Time.time;
    }

    void Update()
    {
        // WIN CONDITION
        if (win)
        {
            Move();

            if (transform.position.y < respawnHeight)
            {
                Respawn();
            }
            return;
        }
        else
        {
            // LOSE CONDITION        
            if (!alive)
            {
                Die();
                return;
            }
            else
            {
                // MOVEMENT: check if the player is alive
                Move();

                // TIMER COUNTDOWN
                energy -= 1 * Time.deltaTime;
                energyBar.SetHealth(energy);

                // COUNT USED TIME
                CountTimeUsed();

                // RESPAWN if fall
                if (transform.position.y < respawnHeight)
                {
                    Respawn();
                }
            }
        }
    }

    private void Move()
    {
        
        // get direction
        float forwardInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 moveDirection = new Vector3(horizontalInput, 0, forwardInput);

        float magnitude = Mathf.Clamp01(moveDirection.magnitude) * speed;
        moveDirection.Normalize();

        Vector3 velocity = moveDirection * magnitude;

        // apply gravity
        gravity = Physics.gravity.y * gravityMultiplier;
        fallSpeed += gravity * Time.deltaTime;

        // check if player is on ground (cannot move if not on ground)
        if (controller.isGrounded)
        {
            animator.SetBool("isGrounded", true);
            fallSpeed = -2f;

            if (moveDirection == Vector3.zero)
            {
                Idle();
            }

            else if (moveDirection != Vector3.zero)
            {
                Run();
            }

            if (Input.GetKey(KeyCode.Space))
            {
                Jump();
            }

            // cannot move while attacking
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                velocity = Vector3.zero;
            }
        }
        else
        {
            animator.SetBool("isGrounded", false);
        }

        // move player
        velocity.y = fallSpeed;
        controller.Move(velocity * Time.deltaTime);

        // rotate player
        if (moveDirection != Vector3.zero)
        {
            transform.forward = moveDirection;
        }

        // ATTACK ACTION
        if(Input.GetKeyDown(KeyCode.J))
        {
            Attack();
        }

    }

    // ACTIVE ACTIONS
    private void Idle()
    {
        animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
    }

    private void Run()
    {
        animator.SetFloat("Speed", 1, 0.1f, Time.deltaTime);
    }

    private void Jump()
    {
        animator.SetTrigger("Jump");
        fallSpeed = Mathf.Sqrt(jumpHeight * -3 * gravity);
        audioSource.PlayOneShot(jumpAudio);
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");
    }

    public void AttackEffectOnEvent()
    {
        GameObject vfx = Instantiate(attackEffect, firePoint.transform.position, firePoint.transform.rotation);
        audioSource.PlayOneShot(attackAudio);
    }

    // REACTIVE ACTIONS
    private void Eat()
    {
        animator.SetTrigger("Eat");
        audioSource.PlayOneShot(chickenEatenAudio);
        energy += energyBoost;
        chickenEaten += 1;
        ChickenDisplay();
        if (energy > startEnergy)
        {
            energyBar.SetMaxHealth(energy);
        }
    }

    private void Die()
    {
        animator.SetTrigger("Die");
    }

    private void DieAudioOnEvent()
    {
        audioSource.PlayOneShot(deadAudio);
    }
    
    private void Respawn()
    {
        transform.position = respawnManager.currentCheckpoint.transform.position;
        audioSource.PlayOneShot(respawnAudio);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Chicken"))
        {
            Eat();

            // instanciate effect and destroy later
            Vector3 effectPosition = new Vector3(other.transform.position.x, other.transform.position.y + 0.5f, other.transform.position.z);
            GameObject newPoofEffect = Instantiate(poofEffect, effectPosition, transform.rotation);
            Destroy(newPoofEffect.gameObject, 3f);

            // set chicken status to inactive
            other.gameObject.SetActive(false);

            // set ui if win
            if (win)
            {
                ui.SendMessage("SetResultText", "YOU WIN");
                ui.SendMessage("SetResultTimeText", timeUsed);
                audioSource.PlayOneShot(winAudio);
            }
        }    
    }

    public void ChickenDisplay()
    {
        switch (respawnManager.currentCheckpoint.name)
        {
            case ("SpringStartCheckPoint"):
                chickenToEat = 5;
                break;
            case ("SummerStartCheckPoint"):
                chickenToEat = 10;
                break;
            case ("AutumnStartCheckPoint"):
                chickenToEat = 15;
                break;
            case ("WinterStartCheckPoint"):
                chickenToEat = 20;
                break;
        }

        ui.SendMessage("SetChickenCount", chickenEaten + " / " + chickenToEat);

    }

    private void CountTimeUsed()
    {
        float time = Time.time - startTime;

        string min = ((int)time / 60).ToString();
        string sec = (time % 60).ToString("f2");

        timeUsed = min + ":" + sec;

        // set ui if lose
        if (energy <= 0)
        {
            ui.SendMessage("SetResultText", "YOU LOST");
            audioSource.PlayOneShot(loseAudio);
        }
    }
}
