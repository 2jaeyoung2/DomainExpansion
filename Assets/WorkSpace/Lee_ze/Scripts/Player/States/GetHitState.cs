using UnityEngine;

public class GetHitState : IPlayerState
{
    private PlayerControl player;

    private int downCount = 20;

    public void EnterState(PlayerControl player)
    {
        this.player = player;

        this.player.agent.isStopped = true;

        Debug.Log("gethit start");
    }

    public void UpdateState()
    {


        // ----> State Change
        if (player.playerStats.PlayerDownCount >= downCount) // �ǰ����� Down Count�� ���� �� ���̸� ������ �Ѿ���
        {
            player.playerStats.PlayerDownCount = 0; // Down Count �ʱ�ȭ

            player.ChangeStateTo(new DownState());

            return;
        }

        if (player.isHit == false)
        {
            player.agent.isStopped = false;

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
        }
    }

    public void ExitState()
    {
        Debug.Log("gethit end");
    }
}
