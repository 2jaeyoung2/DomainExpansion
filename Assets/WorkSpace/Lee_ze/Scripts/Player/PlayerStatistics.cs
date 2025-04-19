using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatistics : MonoBehaviour
{
    private int playerHP;

    public int PlayerHP
    {
        get
        {
            return playerHP;
        }

        set
        {
            playerHP = value;
        }
    }

    private void Start()
    {
        PlayerHP = 100;
    }
}
