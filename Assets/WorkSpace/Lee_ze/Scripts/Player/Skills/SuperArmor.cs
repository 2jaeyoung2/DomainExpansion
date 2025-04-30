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

        player.tempSuperArmorEffect.SetActive(true);

        while (sustainingTime >= 0)
        {
            sustainingTime -= Time.deltaTime;

            Vector3 tempPosition = player.transform.position;

            tempPosition.y = player.transform.position.y + 1f;

            player.tempSuperArmorEffect.transform.position = tempPosition;

            yield return null;
        }

        player.tempSuperArmorEffect.SetActive(false);

        player.playerCollider.enabled = true;
    }
}
