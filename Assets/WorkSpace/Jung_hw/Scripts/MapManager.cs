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
        leftMap.RandomGenerator();
        rightMap.RandomGenerator();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
