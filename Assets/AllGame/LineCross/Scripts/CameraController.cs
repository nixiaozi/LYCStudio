using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    public Transform target;
    public float lerp = 5.0f;

	void LateUpdate () {
        //Lerp Towards the target
        if (target)
            transform.position = Vector3.Lerp(transform.position, new Vector3 (target.position.x, target.position.y,transform.position.z), Time.deltaTime * lerp);
        else
            Debug.LogError("Please Assign all the variables");

	}
}
