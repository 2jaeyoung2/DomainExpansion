using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Placeables", menuName = "Scriptable Object/Placeables", order = int.MaxValue)]
public class Placeables : ScriptableObject
{ 
    [Serializable] 
    public struct Placeable
    {
        public GameObject placeable;
        public int stock;

        public Placeable(GameObject obj, int stocks)
        {
            placeable = obj;
            stock = stocks;
        }
    }

    public List<Placeable> placeableList;

}
