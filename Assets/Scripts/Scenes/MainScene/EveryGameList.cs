using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EveryGameList : MonoBehaviour
{
    [System.Serializable]
    public class GameItem
    {
        public string GameName;
        public string GameType;
        public string GameResource;
        public string GameInfo;
        public string ReferenceSceneName;
    }

    public List<GameItem> AllGames;

}
