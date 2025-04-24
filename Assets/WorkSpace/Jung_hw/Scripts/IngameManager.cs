using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameManager : MonoBehaviour
{
    public static IngameManager Instance;

    Dictionary<int, string> maps;

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
    }

    public void SetMapDict(int id, string map)
    {
        Debug.Log(id + ": " + map);
        maps.Add(id, map);
    }

    public Dictionary<int, string> GetMapData()
    {
        return maps;
    }
}
