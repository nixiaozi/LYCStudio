using Doozy.Engine.Progress;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class SplashSceneWork : MonoBehaviour
{
    Progressor progressor;
    AsyncOperationHandle<SceneInstance> asyncOperationHandle;

    // Start is called before the first frame update
    void Start()
    {
        progressor = GameObject.FindGameObjectsWithTag("LoadProcessBar")[0].GetComponent<Progressor>();

        progressor.SetValue(0.00f);

        Debug.Log("进行加载主场景的操作");
        asyncOperationHandle = Addressables.LoadSceneAsync("MainScene", UnityEngine.SceneManagement.LoadSceneMode.Single,false);
        asyncOperationHandle.Completed += OnLoadComplete;
    }

    private void Update()
    {
        progressor.SetValue(asyncOperationHandle.PercentComplete);
    }

    public void OnLoadComplete(AsyncOperationHandle<SceneInstance> obj)
    {
        Debug.Log("主场景加载完成");
        asyncOperationHandle.Result.Activate();
    }



}
