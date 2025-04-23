using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaBreak : ISkill
{
    private PlayerControl player;

    public void ActiveThisSkill(PlayerControl player)
    {
        this.player = player;

        OnManaBreak();
    }

    private void OnManaBreak()
    {
        player.playerStats.PlayerCurrentStamina -= 40;

        player.ChangeStateTo(new DownState());
    }
}
