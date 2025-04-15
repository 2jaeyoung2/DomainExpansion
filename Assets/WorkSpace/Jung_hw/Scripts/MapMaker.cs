using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMaker : MonoBehaviour
{
    [SerializeField] List<GameObject> tiles;
    [SerializeField] int hor;
    [SerializeField] int ver;
    List<int> tileList;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(hor * ver);
        int temp = hor * ver;
        tileList = new List<int>();
        for(int i = 0; i < temp; i++)
        {
            tileList.Add(Random.Range(0, tiles.Count));
        }
        Debug.Log(tileList.Count);
        Debug.Log(string.Join(" ", tileList.ToArray()));
        MakeMap();
    }

    public void MakeMap()
    {
        Debug.Log("MakeMap");
        for (int i = 0; i < tileList.Count; i++)
        {
            Vector3 pos = new Vector3(i / hor, 0, i % hor);
            Instantiate(tiles[tileList[i]], transform.position + pos, Quaternion.identity);
        }
    }
}
