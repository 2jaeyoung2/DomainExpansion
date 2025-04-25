using System.Collections;
using UnityEngine;

public class Dash : ISkill
{
    private PlayerControl player;

    public void ActiveThisSkill(PlayerControl player)
    {
        this.player = player;
        Debug.Log(2);
        if (this.player.playerStats.PlayerCurrentStamina < this.player.manaBreakCost) // ���׹̳� �����ϸ� �� ��
        {
            return;
        }
        Debug.Log(3);
        this.player.isDash = true; // Dash state�� ���� Ʈ����. �ݵ�� �־�� ��.
        Debug.Log(4);
        this.player.StartCoroutine(DashRoutine()); // Dash
        Debug.Log(7);
        PayDashCost();
    }

    private IEnumerator DashRoutine()
    {
        Debug.Log(5);
        player.playerCollider.enabled = false; // ������ ������ �� �ݶ��̴� ��.

        player.playerAnim.SetTrigger("IsDash"); // ������ �ִϸ��̼� ����.

        yield return player.StartCoroutine(OnDash());
        Debug.Log(8);
        player.isDash = false; // Dash state ������ Ʈ����. �ݵ�� �־�� ��.

        player.playerCollider.enabled = true; // ������ ���� �� �ݶ��̴� ��.
    }

    private IEnumerator OnDash() // Dash �� ��ŭ �̵��ϴ� ����. Dash �ִϸ��̼�X
    {
        Debug.Log(6);
        player.mousePos.TempGetMouseCursorPosition();

        player.SetPlayerRotation();

        float dashSpeed = 7f;

        float timer = 0f;

        while (timer < 0.7f)
        {
            player.transform.position += player.direction.normalized * dashSpeed * Time.deltaTime;

            timer += Time.deltaTime;

            yield return null;
        }
    }

    private void PayDashCost()
    {
        player.playerStats.UseStamina(player.dashCost);

        player.playerStats.GetDashDamage(player.playerStats.PlayerCurrentHP * 0.1f); // �뽬 ��� �� �ڽ� ü�� -10(���� ��ġ)
    }
}
