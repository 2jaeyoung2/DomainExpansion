using System.Collections;
using UnityEngine;

public class Dash : ISkill
{
    private PlayerControl player;

    public void ActiveThisSkill(PlayerControl player)
    {
        this.player = player;

        if (this.player.playerStats.PlayerCurrentStamina < this.player.dashCost) // ���׹̳� �����ϸ�
        {
            return; // ������
        }

        this.player.isDash = true; // Dash state�� ���� Ʈ����. �ݵ�� �־�� ��.

        this.player.StartCoroutine(DashRoutine()); // Dash

        PayDashCost();
    }

    private IEnumerator DashRoutine()
    {

        player.playerCollider.enabled = false; // ������ ������ �� �ݶ��̴� ��.

        player.playerAnim.SetTrigger("IsDash"); // ������ �ִϸ��̼� ����.

        yield return player.StartCoroutine(OnDash());

        player.isDash = false; // Dash state ������ Ʈ����. �ݵ�� �־�� ��.

        player.playerCollider.enabled = true; // ������ ���� �� �ݶ��̴� ��.
    }

    private IEnumerator OnDash() // Dash �� ��ŭ �̵��ϴ� ����. Dash �ִϸ��̼�X
    {
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
