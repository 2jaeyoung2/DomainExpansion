using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IPlayerState
{
    private PlayerControl player;

    public void EnterState(PlayerControl player)
    {
        this.player = player;

        player.playerAnim.SetBool("IsRun", false);

        Debug.Log("idle start");
    }

    public void UpdateState()
    {


        // ----> State Change
        if (player.isDash == true)
        {
            player.ChangeStateTo(new DashState());

            return;
        }

        if (player.attackCheck.isAttack == true) // АјАн state
        {
            player.ChangeStateTo(new AttackState());

            return;
        }

        if (player.agent.hasPath == true)
        {
            player.ChangeStateTo(new RunState());

            return;
        }
    }

    public void ExitState()
    {
        Debug.Log("idle end");
    }
}
