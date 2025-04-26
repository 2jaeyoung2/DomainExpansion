using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaBreak : ISkill
{
    private PlayerControl player;

    public void ActiveThisSkill(PlayerControl player)
    {
        this.player = player;

        if (player.playerStats.PlayerCurrentStamina < player.manaBreakCost || player.isDown == true) // ���׹̳� �����ϸ� �� ��
        {
            return;
        }

        OnManaBreak();
    }

    private void OnManaBreak()
    {
        player.playerStats.UseStamina(player.manaBreakCost);

        player.ChangeStateTo(new DownState());
    }
}
