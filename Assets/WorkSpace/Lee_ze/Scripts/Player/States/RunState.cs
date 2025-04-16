using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : IPlayerState
{
    private PlayerControl player;

    public void EnterState(PlayerControl player)
    {
        this.player = player;

        this.player.isMoving = true;

        player.playerAnim.SetBool("IsMove", true);
    }

    public void UpdateState()
    {


        // ----> State Change
        if (player.isDash == true)
        {
            player.ChangeStateTo(new DashState());
        }
        if (player.agent.velocity.magnitude < 0.1f)
        {
            player.ChangeStateTo(new IdleState());
        }
    }

    public void ExitState()
    {
        player.isMoving = false;
    }
}
