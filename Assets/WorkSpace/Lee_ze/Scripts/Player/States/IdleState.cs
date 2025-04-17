using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IPlayerState
{
    private PlayerControl player;

    public void EnterState(PlayerControl player)
    {
        this.player = player;

        this.player.isMoving = false;

        player.playerAnim.SetBool("IsRun", false);
    }

    public void UpdateState()
    {


        // ----> State Change
        if (player.isDash == true)
        {
            player.ChangeStateTo(new DashState());
        }
        if (player.agent.velocity.magnitude > 0.1f)
        {
            player.ChangeStateTo(new RunState());
        }
    }

    public void ExitState()
    {

    }
}
