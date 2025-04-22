using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatistics : MonoBehaviour
{
    private float playerHP;

    private float playerStamina;

    private int playerDownCount;

    public float PlayerHP
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

    public float PlayerStamina
    {
        get
        {
            return playerStamina;
        }
        set
        {
            playerStamina = value;
        }
    }

    public int PlayerDownCount
    {
        get
        {
            return playerDownCount;
        }
        set
        {
            playerDownCount = value;
        }
    }

    private void Start()
    {
        PlayerHP = 30; // TODO: 200쯤으로 설정하면 될 듯.

        PlayerStamina = 100;

        PlayerDownCount = 0;
    }
}
