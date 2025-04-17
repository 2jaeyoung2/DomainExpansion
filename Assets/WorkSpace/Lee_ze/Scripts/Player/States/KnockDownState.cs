using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockDownState : IPlayerState
{
    private PlayerControl player;

    public void EnterState(PlayerControl player)
    {
        this.player = player;

        Debug.Log("knockdown start");
    }

    public void UpdateState()
    {

    }

    public void ExitState()
    {
        Debug.Log("knockdown end");
    }
}
