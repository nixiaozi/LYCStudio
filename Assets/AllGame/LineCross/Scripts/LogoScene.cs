using UnityEngine;
using System.Collections;

public class LogoScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //Start game in 1.5 seconds
        Invoke("StartGame", 1.5f);
	}

    void StartGame() {
        Initiate.Fade("Game", Color.red, 2.0f);
    }
    
}
