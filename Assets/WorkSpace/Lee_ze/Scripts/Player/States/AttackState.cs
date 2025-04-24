using UnityEngine;

public class AttackState : IPlayerState
{
    private PlayerControl player;

    public void EnterState(PlayerControl player)
    {
        this.player = player;

        //if (this.player.agent.hasPath == true) // ���� �����ϸ� ���� ���� ����
        //{
        //    this.player.agent.ResetPath();
        //}

        this.player.agent.speed = 0;

        this.player.WeaponsOn();

        Debug.Log("attack start");
    }

    public void UpdateState()
    {


        // ----> State Change
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

        if (player.attackCheck.isAttack == true)
        {
            return; // ���� ���̶�� �Ʒ� �ڵ� ����
        }

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
    }

    public void ExitState()
    {
        player.WeaponsOff();

        player.agent.speed = 5f;

        player.playerAnim.ResetTrigger("Z");

        player.playerAnim.ResetTrigger("X");

        Debug.Log("attack end");
    }
}
