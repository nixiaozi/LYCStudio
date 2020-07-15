using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {
    //Fade volor
    public Color fadeColor = Color.white;
    public Image soundButtonStart;
    public Image soundButtonEnd;
    public Sprite soundOn;
    public Sprite soundOff;

    // Use this for initialization
    void Start () {
        //Set looks for the button at start
        SetSoundButton();
    }
    //Put your leaderboard code here
    public void Leaderboard() {
        Debug.Log("Leaderboard Goes here , Click to Open In IDE");
    }

    //Restart the Game
    public void Replay() {
        Initiate.Fade(Application.loadedLevelName, fadeColor, 2.0f);
    }

    public void ChangeSound() {
        //Turn sound on or off

        if (PlayerPrefs.GetInt("Sound", 1) == 1) {
            PlayerPrefs.SetInt("Sound", 0);
        }
        else if (PlayerPrefs.GetInt("Sound", 1) == 0)
        {
            PlayerPrefs.SetInt("Sound", 1);
        }

        SetSoundButton();

    }

    //Set how the audio button looks
    void SetSoundButton() {

        if (!soundOn || !soundOff || !soundButtonEnd || !soundButtonStart)
            Debug.LogError("Please Assign all the variables");

        if (PlayerPrefs.GetInt("Sound", 1) == 1)
        {
            AudioListener.volume = 1.0f;
            soundButtonStart.sprite = soundOn;
            soundButtonEnd.sprite = soundOn;
        }
        else {
            AudioListener.volume = 0.0f;
            soundButtonStart.sprite = soundOff;
            soundButtonEnd.sprite = soundOff;
        }
    }

}
