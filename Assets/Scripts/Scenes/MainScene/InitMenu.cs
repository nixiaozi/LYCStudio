using DG.Tweening;
using DG.Tweening.Plugins.Core;
using Doozy.Engine.Touchy;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static EveryGameList;

public class InitMenu : MonoBehaviour
{

    public GameItem TheCurrentEntryGame
    {
        get
        {
            return gameObject.GetComponent<EveryGameList>().AllGames[CurrentRotateCount];
        }
    }

    //这个是定义需要的菜单对象预制件
    public GameObject MenuItem;

    //单倍的半径是多少
    public float PerRadius=500f;
    //当前的半径大小
    private float CurrentRadius;
    //当前旋转过的次数（正负数分别表示不同方向的旋转）
    private int CurrentRotateCount = 0;
    //表示当前所旋转的总角度数
    private float CurrentRotatedAngle = 0f;

    // 这个定义显示的菜单数
    private int MenuCount =1;

    // Start is called before the first frame update
    void Start()
    {
        if (CurrentRotateCount != 0)  // 已经进行初始化后就不需要再次初始化了
            return;

        //初始化获取游戏列表
        var allgame = gameObject.GetComponent<EveryGameList>().AllGames;
        MenuCount = allgame.Count;

        CurrentRadius = PerRadius * MenuCount;

        var initX = 0f;
        var initY = 0f;
        var initZ = PerRadius + CurrentRadius;

        //首先根据当前菜单项多少设置当前对象的位置
        gameObject.transform.Translate(new Vector3(initX, initY, initZ), Space.World);

        Debug.Log("this gameObject 当前位置为：x:" + gameObject.transform.position.x + ";y:"
            + gameObject.transform.position.y + ";z:" + gameObject.transform.position.z);
        var zeroPointTransform = gameObject.transform.position;

        // 选着开始旋转的向量
        var originalTransVector = - (zeroPointTransform - new Vector3(initX, initY, PerRadius));

        float perRotate = 360f / MenuCount;
        for(var i = 0; i < MenuCount; i++)
        {
            var currentTransVector = Quaternion.Euler(0, -perRotate*i, 0) * originalTransVector;
            // Instantiate at position (0, 0, 0) and zero rotation.
           var the= Instantiate(MenuItem,new Vector3(0,0, PerRadius), Quaternion.identity); //添加同向的自转，保证与圆相切 Quaternion.Euler(0, -perRotate * i, 0)
            the.GetComponentInChildren<Canvas>().worldCamera= GameObject.FindObjectOfType<Camera>();  //添加Canvas 的事件相机绑定
            the.name = "GameItem_" + i;
            // 为按钮添加绑定事件
            the.transform.Find("Canvas/Canvas/Panel-EntryButton/Button").GetComponent<Button>().onClick.AddListener(OnClickEntryGame);

            //初始化对象的显示内容
            the.transform.Find("Canvas/GameName").GetComponent<TextMeshProUGUI>().text = allgame[i].GameName;
            the.transform.Find("Canvas/Canvas/Panel-GameType/GameTypeStr").GetComponent<TextMeshProUGUI>().text= allgame[i].GameType;
            the.transform.Find("Canvas/Canvas/Panel-GameResource/GameResourceStr").GetComponent<TextMeshProUGUI>().text = allgame[i].GameResource;
            the.transform.Find("Canvas/Canvas/Panel-GameInfo/GameInfoStr").GetComponent<TextMeshProUGUI>().text = allgame[i].GameInfo;


            //初始化对象的位置
            the.transform.Translate(currentTransVector - originalTransVector);
            the.transform.Rotate(new Vector3(0, -perRotate * i, 0));
            the.transform.SetParent(gameObject.transform,true);


            Debug.Log("MenuItem"+i+" 当前位置为：x:" + the.transform.position.x + ";y:"
            + the.transform.position.y + ";z:" + the.transform.position.z);
        }

        DOTween.Init();
    }

    public void OnTouchLeft(TouchInfo touchInfo)
    {
        var perRotation = 360f / MenuCount;

        //gameObject.transform.DORotateQuaternion(
        //Quaternion.Euler(0, gameObject.transform.rotation.y+ perRotation, 0), 0.8f)
        //.SetEase(Ease.InOutBack);
        //var myVector = new Vector3(0, 0, 0);

        //DOTween.To(() => myVector, x => myVector = x, new Vector3(3, 4, 8), 1);

        //var currentRotate = gameObject.transform.rotation.y + 72f;
        //DOTween.To(() => gameObject.transform.rotation, target => gameObject.transform.rotation = target, new Vector3(0, currentRotate, 0), 1);
        //处理旋转问题是出现奇怪的问题，输入的角度值，会在第二此读取的时候变成小数，以后不管输入什么值要进行角度转换最后都会复原。。。

        Quaternion rotationY = Quaternion.Euler(0f, perRotation, 0f);
        // transform.rotation = rotationY * transform.rotation;
        transform.DORotate(rotationY.eulerAngles, 0.8f, RotateMode.LocalAxisAdd)
            .SetEase(Ease.OutBack);

        CurrentRotateCount++;
        CurrentRotatedAngle += perRotation;
    }

    public void OnTouchRight(TouchInfo touchInfo)
    {
        var perRotation = 360f / MenuCount;


        //Quaternion rotationY = Quaternion.Euler(0f, CurrentRotateAngle- perRotation, 0f)*transform.rotation;
        Quaternion rotationY = Quaternion.Euler(0f, CurrentRotatedAngle - perRotation, 0f); //这里需要指定绝对度数。
        transform.DORotate(rotationY.eulerAngles, 0.8f, RotateMode.Fast) // Fast 表示总寻到最短的旋转路径；FastBeyond360 表示以正旋方式找的的最短旋转路径
            .SetEase(Ease.OutBack);

        CurrentRotateCount--;
        CurrentRotatedAngle -= perRotation;
    }


    private void OnClickEntryGame()
    {
        Debug.Log("You Enter The Game!"+TheCurrentEntryGame.GameName);

        Addressables.LoadSceneAsync(TheCurrentEntryGame.ReferenceSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single).Completed += OnSceneLoaded;

    }


    void OnSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
    {
        //Addressables.UnloadSceneAsync(new SceneInstance());
        // LOGIC THAT KICKSTARTS THE GAMEPLAY
    }


}
