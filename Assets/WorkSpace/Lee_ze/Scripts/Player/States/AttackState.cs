using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IPlayerState
{
    private PlayerControl player;

    public void EnterState(PlayerControl player)
    {
        this.player = player;

        if (this.player.agent.hasPath == true)
        {
            this.player.agent.ResetPath();
        }

        Debug.Log("attack start");
    }

    public void UpdateState()
    {


        // ----> State Change
        if (player.attackCheck.isAttack == false)
        {
            if (player.agent.hasPath == false)
            {
                player.ChangeStateTo(new IdleState());

                return;
            }

            if (player.agent.hasPath == true)
            {
                player.ChangeStateTo(new RunState());

                return;
            }

            if (player.isDash == true)
            {
                player.ChangeStateTo(new DashState());

                return;
            }
        }
        
        //if (player.isCancled == true)
        //{
        //    player.playerAnim.SetTrigger("ToIdle");

        //    player.ChangeStateTo(new IdleState());
        //}
    }

    public void ExitState()
    {
        Debug.Log("attack end");
    }
}
