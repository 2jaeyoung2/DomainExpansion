using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    Dictionary<int, string> maps;
    Dictionary<int, int> spawnLoc; //TODO: actornum, 타일번호(child 순서) 저장

    public List<int> players; //0: Red 1. Blue

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        maps = new();
        spawnLoc = new();
        players = new(2);
    }

    public void SetMapDict(int id, string map)
    {
        Debug.Log(id + ": " + map);
        maps.Add(id, map);
    }

    public void SetSpawnPoint(int id, int index)
    {
        Debug.Log("SSP " + id + " " + index);
        spawnLoc.Add(id, index);
    }

    public int GetSpawnPoint(int id)
    {
        return spawnLoc.ContainsKey(id) ? spawnLoc[id] : -1;
    }

    public void SetId(int actorNum)
    {
        players.Add(actorNum);
    }

    public Dictionary<int, string> GetMapData()
    {
        return maps;
    }
}
