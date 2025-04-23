using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHitState : IPlayerState
{
    private PlayerControl player;

    public void EnterState(PlayerControl player)
    {
        this.player = player;

        Debug.Log("gethit start");
    }

    public void UpdateState()
    {


        // ----> State Change
        if (player.playerStats.PlayerDownCount >= 20) // 피격으로 Down Count가 일정 량 쌓이면 강제로 넘어짐
        {
            player.playerStats.PlayerDownCount = 0; // Down Count 초기화

            player.ChangeStateTo(new DownState());

            return;
        }

        if (player.isHit == false)
        {
            if (player.agent.hasPath == false)
            {
                player.ChangeStateTo(new IdleState());

                return;
            }
        }
    }

    public void ExitState()
    {
        Debug.Log("gethit end");
    }
}
