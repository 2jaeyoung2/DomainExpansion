using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IPlayerState
{
    private PlayerControl player;

    public void EnterState(PlayerControl player)
    {
        this.player = player;

        Debug.Log("IdleStart");

        Debug.Log(player.agent.isStopped);
    }

    public void UpdateState()
    {
        if (player.agent.isStopped == false)
        {
            player.ChangeStateTo(new MoveState());
        }
    }

    public void ExitState()
    {
        Debug.Log("IdleEnd");
    }
}
