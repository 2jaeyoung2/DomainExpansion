using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameManager : MonoBehaviour
{
    [SerializeField] MapManager mapManager;
    [SerializeField] GameObject playerObj;

    void Start()
    {
        Debug.Log("r: " + GameManager.Instance.GetSpawnPoint(GameManager.Instance.players[0]) +
            " b: " + GameManager.Instance.GetSpawnPoint(GameManager.Instance.players[1]));
        //진짜 구린데 이거 어캄
        Instantiate(playerObj, mapManager.leftMap.transform.GetChild(GameManager.Instance.GetSpawnPoint(GameManager.Instance.players[0]))
            .GetChild(0).position, Quaternion.identity); //레드

        Instantiate(playerObj, mapManager.rightMap.transform.GetChild(GameManager.Instance.GetSpawnPoint(GameManager.Instance.players[1]))
            .GetChild(0).position, Quaternion.identity); //블루

    }
}
