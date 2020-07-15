using UnityEngine;
using System.Collections;

public class CollisionSensor : MonoBehaviour {

    void OnTriggerEnter2D() {
        //See if we have collided and if yes the call the over message and end the game
        transform.root.SendMessage("Over", SendMessageOptions.RequireReceiver);
    }
}
