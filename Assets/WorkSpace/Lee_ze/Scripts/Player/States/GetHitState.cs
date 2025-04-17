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

    }

    public void ExitState()
    {
        Debug.Log("gethit end");
    }
}
