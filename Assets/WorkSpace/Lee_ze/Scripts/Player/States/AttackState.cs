using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IPlayerState
{
    private PlayerControl player;

    public void EnterState(PlayerControl player)
    {
        this.player = player;
    }

    public void UpdateState()
    {

    }

    public void ExitState()
    {

    }
}
