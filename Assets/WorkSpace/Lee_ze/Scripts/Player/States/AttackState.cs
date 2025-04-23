using UnityEngine;

public class AttackState : IPlayerState
{
    private PlayerControl player;

    public void EnterState(PlayerControl player)
    {
        this.player = player;

        if (this.player.agent.hasPath == true) // 공격 시작하면 가던 길을 멈춤
        {
            this.player.agent.ResetPath();
        }

        player.WeaponsOn();

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
        player.WeaponsOff();

        Debug.Log("attack end");
    }
}
