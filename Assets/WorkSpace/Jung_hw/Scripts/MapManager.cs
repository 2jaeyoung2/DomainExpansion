using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public MapMaker leftMap;
    public MapMaker rightMap;
    // Start is called before the first frame update
    void Start()
    {
        Dictionary<int, string> mapData = GameManager.Instance.GetMapData();

        leftMap.SetTileList(mapData[GameManager.Instance.players[0]]);
        rightMap.SetTileList(mapData[GameManager.Instance.players[1]]);

        leftMap.MakeMap();
        rightMap.MakeMap();
    }
}
