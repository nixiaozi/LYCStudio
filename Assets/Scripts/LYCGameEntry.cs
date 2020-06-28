using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class LYCGameEntry : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Addressables.LoadSceneAsync("StartScene").Completed += OnLoadComplete;
    }



    private void OnLoadComplete(AsyncOperationHandle<SceneInstance> obj)
    {
        // 编写加载成功后的代码

    }

}
