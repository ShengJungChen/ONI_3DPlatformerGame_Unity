using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatfromMovementControl_Last : MonoBehaviour
{
    public bool moving = false;
    private bool onBoard = false;
    public PlatformPointManager path;
    public int speed;
    private int targetPointIdx;
    private Transform previousPoint;
    private Transform targetPoint;

    private float timeToPoint;
    private float timeElapsed;

    public Transform point2;
    public Transform point4;

    void Start()
    {
        if (moving == true)
        {
            TargetNextPoint();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (moving == true)
        {
            timeElapsed += Time.deltaTime;
            float elaspedPercentage = timeElapsed / timeToPoint;
            elaspedPercentage = Mathf.SmoothStep(0, 1, elaspedPercentage);
            transform.position = Vector3.Lerp(previousPoint.position, targetPoint.position, elaspedPercentage);
            transform.rotation = Quaternion.Lerp(previousPoint.rotation, targetPoint.rotation, elaspedPercentage);

            if (elaspedPercentage >= 1)
            {
                TargetNextPoint();
            }
        }

        if (transform.position == point2.position && !onBoard)
        {
            moving = false;
        }

        if (transform.position == point4.position)
        {
            if (onBoard)
            {
                moving = false;
            }
            else
            {
                moving = true;
                // TargetNextPoint();
            }
        }

    }

    public void TargetNextPoint()
    {
        // get current point
        previousPoint = path.GetPoint(targetPointIdx);
        // get next point idx
        targetPointIdx = path.GetNextPointIdx(targetPointIdx);
        // set target point with idx
        targetPoint = path.GetPoint(targetPointIdx);

        timeElapsed = 0;
        float distanceToTarget = Vector3.Distance(previousPoint.position, targetPoint.position);
        timeToPoint = distanceToTarget / speed;
    }

    public void TargetIdxPoint()
    {
        // get current point
        previousPoint = path.GetPoint(targetPointIdx);
        // get next point idx
        targetPointIdx = path.GetNextPointIdx(0);
        // set target point with idx
        targetPoint = path.GetPoint(targetPointIdx);

        timeElapsed = 0;
        float distanceToTarget = Vector3.Distance(previousPoint.position, targetPoint.position);
        timeToPoint = distanceToTarget / speed;
    }
    

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
            onBoard = true;
            moving = true;
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(null);
            onBoard = false;
        }    
    }
}
