using UnityEngine;
using System.Collections;

public class ChallengeInfo : MonoBehaviour {
    Vector3 startPos;
    public Transform endPos;

    void Start() {
        
        if (!endPos) {
            Debug.Log("Please assign all the variables");
        }
    }


    void OnDrawGizmos() {
        //Draw gizmos for start and end point
        startPos = transform.position;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(startPos,.1f);
        Gizmos.color = Color.red;
        if (endPos)
        {
            Gizmos.DrawWireSphere(endPos.position, .1f);
        }
    }

    public Vector3 GetStartPos() {
            return startPos;
    }

    public Vector3 GetEndPos()
    {
        if (endPos)
        {
            return endPos.position;
        }
        else {
            return Vector3.zero;
        }
    }
}
