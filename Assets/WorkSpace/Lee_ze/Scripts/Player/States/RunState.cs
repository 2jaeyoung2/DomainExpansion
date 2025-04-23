using UnityEngine;

public class RunState : IPlayerState
{
    private PlayerControl player;

    public void EnterState(PlayerControl player)
    {
        this.player = player;

        player.playerAnim.SetBool("IsRun", true);

        Debug.Log("run start");
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

        if (player.agent.hasPath == false)
        {
            player.ChangeStateTo(new IdleState());

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
        Debug.Log("run end");
    }
}
