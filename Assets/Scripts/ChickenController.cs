using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChickenController : MonoBehaviour
{

    // VARIABLE
    public bool isFrozen = false;
    public bool isChased = false;
    [Header("Walk Control")]
    public float walkRaduis;
    public float walkSpeed;
    [Header("Run Control")]
    public float runDistance;
    public float runSpeed;
    [Header("Frozen Effect")]
    public Material normalMat;
    public Material frozenMat;
    public int frozenTime;
    public AudioClip freezeAudio;

    // REFERENCE
    public GameObject player;
    private NavMeshAgent agent;
    private Animator animator;
    private Renderer rend;

    private List<Vector3> positions = new List<Vector3>();
    private AudioSource audioSource;

    void Start()
    {

        InvokeRepeating("CheckIfStuck", 0f, 0.05f);

        agent = GetComponent<NavMeshAgent>();
        agent.speed = walkSpeed;

        animator = GetComponent<Animator>();

        StartCoroutine(Wander());

        rend = gameObject.GetComponentInChildren<Renderer>();

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {

        if (!gameObject.activeSelf)
        {
            CancelInvoke();
        }
        
        // if the chicken if not frozen, it can move
        if (!isFrozen)
        {
            // can wander and have animation
            agent.enabled = true;
            animator.enabled = true;
            rend.sharedMaterial = normalMat;

            // get distance between player and chicken & check if player is in range
            float distance = Vector3.Distance(transform.position, player.transform.position);
            isChased = distance < runDistance ? true : false;

            // chased condition
            animator.SetBool("isChased", isChased);

            if (!isChased && agent.remainingDistance <= agent.stoppingDistance)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Turn Head"))
                {
                    agent.velocity = Vector3.zero;
                    agent.speed = 0;
                    agent.isStopped = true;
                }
                else
                {
                    agent.isStopped = false;
                    StartCoroutine(Wander());
                }
            } 

            if (isChased)
            {
                Run();
            }
        }
        else
        {
            agent.enabled = false;
            animator.enabled = false;
        }
        
    }

    public Vector3 RandomLocation()
    {
        Vector3 finalPosition = Vector3.zero;
        Vector3 randomPosition = Random.insideUnitSphere * walkRaduis;
        randomPosition += transform.position;
        if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, walkRaduis, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    public IEnumerator Wander()
    {
        // start wandering animation & set speed
        animator.SetFloat("Speed", 1f);
        agent.speed = walkSpeed;
        agent.SetDestination(RandomLocation());
        // stop for random time
        agent.speed = 0;
        animator.SetFloat("Speed", 0f);
        // random eat animation during stop time
        animator.SetBool("isEating", Random.value > 0.5f);
        int stopTime = Random.Range(0, 2);
        yield return new WaitForSeconds(stopTime);
        animator.SetBool("isEating", false);
        // after stoping, resume wandering
        agent.speed = walkSpeed;
        animator.SetFloat("Speed", 1f);
    }

    public void Run()
    {

        Vector3 normDir = (player.transform.position - transform.position).normalized;
        normDir = Quaternion.AngleAxis(Random.Range(-90,90), Vector3.up) * normDir;
        agent.SetDestination(transform.position - normDir * walkRaduis);
        agent.speed = runSpeed;

    }

    private IEnumerator OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Attack"))
        {
            // destory attack effect
            Destroy(other.gameObject);

            // play audio
            audioSource.PlayOneShot(freezeAudio);

            // stop chicken movement & animation & apply frozen look
            isFrozen = true;
            rend.sharedMaterial = frozenMat;
            yield return new WaitForSeconds(frozenTime * 2 / 3);
            // blink faster when 2/3 of the frozen time has passed
            rend.material.SetFloat("_BlinkSpeed", 10);
            yield return new WaitForSeconds(frozenTime / 3);

            // resume movement & animation & look
            isFrozen = false;
        }
    }

    private void CheckIfStuck()
    {
        if (isChased)
        {
            positions.Add(transform.position);
            if (positions.Count > 2)
            {
                if (positions[0] == positions[1])
                {
                    // print("Stuck");
                    agent.enabled = false;
                    // agent.enabled = true;
                    positions.Clear();
                }
                else
                {
                    positions.RemoveAt(0);
                }
            }
        }
    }
}
