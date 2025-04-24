using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] MapMaker leftMap;
    [SerializeField] MapMaker rightMap;
    // Start is called before the first frame update
    void Start()
    {
        foreach(var data in IngameManager.Instance.GetMapData())
        {
            if(PhotonNetwork.LocalPlayer.ActorNumber == data.Key)
            {
                leftMap.SetTileList(data.Value);
            }
            else
            {
                rightMap.SetTileList(data.Value);
            }
        }
        leftMap.MakeMap();
        rightMap.MakeMap();
    }
}
