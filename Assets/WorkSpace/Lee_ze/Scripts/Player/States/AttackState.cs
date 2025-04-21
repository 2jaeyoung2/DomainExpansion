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
            if (player.agent.hasPath == true)
            {
                Debug.Log("A2R");
                player.ChangeStateTo(new RunState());

                return;
            }

            if (player.agent.hasPath == false)
            {
                Debug.Log("A2I");
                player.ChangeStateTo(new IdleState());

                return;
            }

            if (player.isDash == true)
            {
                player.ChangeStateTo(new DashState());

                return;
            }

            if (player.isHit == true)
            {
                player.ChangeStateTo(new GetHitState());

                return;
            }
        }
    }

    public void ExitState()
    {
        Debug.Log("attack end");
    }
}
