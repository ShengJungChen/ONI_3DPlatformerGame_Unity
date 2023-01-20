using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPointManager : MonoBehaviour
{

    // public bool repeat = true;

    public Transform GetPoint(int pointIndex)
    {
        return transform.GetChild(pointIndex);
    }

    public int GetNextPointIdx(int currentIdx)
    {
        int nextPointIdx = currentIdx + 1;
        
        if (nextPointIdx == transform.childCount)
        {
            nextPointIdx = 0;
            // if (repeat)
            // {
            //     nextPointIdx = 0;
            // }
            // else
            // {
            //     nextPointIdx = currentIdx;
            // }
        }
        return nextPointIdx;
    }
}
