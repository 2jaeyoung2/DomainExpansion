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
        if (player.playerStats.PlayerDownCount >= downCount) // 피격으로 Down Count가 일정 량 쌓이면 강제로 넘어짐
        {
            player.playerStats.PlayerDownCount = 0; // Down Count 초기화

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
