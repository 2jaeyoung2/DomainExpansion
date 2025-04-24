using System.Collections;
using UnityEngine;

public class SuperArmor : ISkill
{
    private PlayerControl player;

    public void ActiveThisSkill(PlayerControl player)
    {
        this.player = player;

        player.StartCoroutine(OnSuperArmor(2f));
    }

    private IEnumerator OnSuperArmor(float sustainingTime)
    {
        player.playerCollider.enabled = false;

        yield return new WaitForSeconds(sustainingTime);

        player.playerCollider.enabled = true;
    }
}
