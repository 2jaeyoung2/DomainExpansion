using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : IPlayerState
{
    private PlayerControl player;

    public void EnterState(PlayerControl player)
    {
        this.player = player;

        player.playerAnim.SetBool("IsMove", true);
    }

    public void UpdateState()
    {
        if (player.agent.velocity.magnitude < 0.1f)
        {
            player.ChangeStateTo(new IdleState());
        }
    }

    public void ExitState()
    {
        player.playerAnim.SetBool("IsMove", false);
    }
}
