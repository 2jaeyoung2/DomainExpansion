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

        if (player.attackCheck.isAttack == true)
        {
            player.ChangeStateTo(new AttackState());

            return;
        }

        if (player.agent.hasPath == true)
        {
            player.ChangeStateTo(new RunState());

            return;
        }

        if (player.isHit == true)
        {
            player.ChangeStateTo(new GetHitState());

            return;
        }
    }

    public void ExitState()
    {
        Debug.Log("idle end");
    }
}
