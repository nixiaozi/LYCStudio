using Doozy.Engine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class BaseEventHandler:MonoBehaviour
{
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("BaseEventHandle");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        GameEventMessage.SendEvent("Start LYC Studio Game! ");
    }
    private void OnEnable()
    {
        //Start listening for game events
        Message.AddListener<GameEventMessage>(OnMessage);
    }

    private void OnDisable()
    {
        //Stop listening for game events
        // Message.RemoveListener<GameEventMessage>(OnMessage);
    }

    private void OnMessage(GameEventMessage message)
    {
        if (message != null)
        {
            Debug.Log("Received the '" + message.EventName + "' game event.");

            switch (message.EventName)
            {
                case "Start LYC Studio Game! ":
                    // OnStartSceneDone("FirstScene");
                    break;
                case "StartSceneDone":
                    OnStartSceneDone("SplashScene");
                    break;
                default:
                    break;
            }

        }

        if (message.Source != null)
            Debug.Log("'" + message.EventName + "' game event was sent by the [" + message.Source.name + "] GameObject.");
    }



    private void OnStartSceneDone(string SceneName)
    {
        Addressables.LoadSceneAsync(SceneName, UnityEngine.SceneManagement.LoadSceneMode.Single).Completed += OnSceneLoaded;
    }


    public void OnSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
    {

    }

}
