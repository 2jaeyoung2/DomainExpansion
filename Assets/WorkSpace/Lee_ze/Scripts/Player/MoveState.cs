using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : IPlayerState
{
    private PlayerControl player;

    public void EnterState(PlayerControl player)
    {
        this.player = player;

        Debug.Log("MoveStart");
    }

    public void UpdateState()
    {
        if (player.agent.isStopped == true)
        {
            player.ChangeStateTo(new MoveState());
        }
    }

    public void ExitState()
    {
        Debug.Log("MoveEnd");
    }
}
