using System.Collections;
using UnityEngine;

public class Dash : ISkill
{
    private PlayerControl player;

    public void ActiveThisSkill(PlayerControl player)
    {
        this.player = player;

        if (this.player.playerStats.PlayerCurrentStamina < this.player.dashCost) // 스테미나 부족하면
        {
            return; // 나가기
        }

        this.player.isDash = true; // Dash state로 가는 트리거. 반드시 있어야 함.

        this.player.StartCoroutine(DashRoutine()); // Dash

        this.player.StartCoroutine(ActivateDashHitBox());

        PayDashCost();
    }

    private IEnumerator DashRoutine()
    {
        player.playerCollider.enabled = false; // 구르기 시작할 때 콜라이더 끔.

        player.playerAnim.SetTrigger("IsDash"); // 구르기 애니메이션 시작.

        yield return player.StartCoroutine(OnDash());

        player.isDash = false; // Dash state 끝내는 트리거. 반드시 있어야 함.

        player.playerCollider.enabled = true; // 구르기 끝날 때 콜라이더 켬.
    }

    private IEnumerator OnDash() // Dash 한 만큼 이동하는 로직. Dash 애니메이션X
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

    private IEnumerator ActivateDashHitBox()
    {
        Vector3 tempPosition = player.transform.position;

        tempPosition.y = -20;

        player.tempDashHitBox.transform.position = tempPosition;

        player.tempDashHitBox.transform.rotation = player.transform.rotation;

        player.tempDashHitBox.SetActive(true);

        yield return new WaitUntil(() =>
        player.playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Dash"));

        yield return new WaitUntil(() =>
        player.playerAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f);

        player.tempDashHitBox.SetActive(false);
    }

    private void PayDashCost()
    {
        player.playerStats.UseStamina(player.dashCost);

        player.playerStats.GetDashDamage(player.playerStats.PlayerCurrentHP * 0.1f); // 대쉬 사용 시 자신 체력 -10(임의 수치)
    }
}
