using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AllGamePooler: MonoBehaviour
{

    [System.Serializable]
    public class GameTypeInfo
    {
        [System.Serializable]
        public class GameInfo
        {
            public string GameName;
            public Sprite GameImg;
            public string StartScene;
            public string GameIntro;
        }

        public string GameType;
        public Sprite GameTypeImg;
        public List<GameInfo> Games;
    }

    
    public List<GameTypeInfo> allGameType;

    /// <summary>
    /// 启动时执行
    /// </summary>
    private void Awake()
    {
        
    }


}
