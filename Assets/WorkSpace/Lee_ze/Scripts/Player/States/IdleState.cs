using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics.Internal;
using UnityEngine;

public class IdleState : IPlayerState
{
    private PlayerControl player;

    public void EnterState(PlayerControl player)
    {
        this.player = player;

        Debug.Log("IdleStart");
    }

    public void UpdateState()
    {
        if (player.agent.velocity.magnitude > 0.1f)
        {
            player.ChangeStateTo(new MoveState());
        }
    }

    public void ExitState()
    {
        Debug.Log("IdleEnd");
    }
}
