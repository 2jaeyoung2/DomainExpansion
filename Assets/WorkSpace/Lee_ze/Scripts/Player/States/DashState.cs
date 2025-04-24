using System.Collections;
using UnityEngine;

public class DashState : IPlayerState
{
    private PlayerControl player;

    public void EnterState(PlayerControl player)
    {
        this.player = player;

        InitPlayerCondition();

        this.player.playerCollider.enabled = false; // ���� �� �ݶ��̴� ����

        this.player.StartCoroutine(DashRoutine());

        this.player.playerAnim.SetTrigger("IsDash");

        PayDashCost();

        Debug.Log("dash start");
    }

    public void UpdateState()
    {


        // ----> State Change
        if (player.isDash == true)
        {
            return;
        }

        if (player.agent.hasPath == false)
        {
            player.ChangeStateTo(new IdleState());

            return;
        }

        if (player.agent.hasPath == true)
        {
            player.ChangeStateTo(new RunState());

            return;
        }

        if (player.attackCheck.isAttack == true)
        {
            player.ChangeStateTo(new AttackState());

            return;
        }
    }

    public void ExitState()
    {
        if (player.agent.hasPath == false)
        {
            player.playerAnim.SetBool("IsRun", false);
        }

        else if (player.agent.hasPath == true)
        {
            player.playerAnim.SetBool("IsRun", true);
        }

        player.playerCollider.enabled = true; // �ݶ��̴� �ѱ�

        Debug.Log("dash end");
    }

    private void PayDashCost()
    {
        player.playerStats.UseStamina(player.dashCost);

        player.playerStats.GetDashDamage(player.playerStats.PlayerCurrentHP * 0.1f); // �뽬 ��� �� �ڽ� ü�� -10(���� ��ġ)
    }

    private IEnumerator DashRoutine()
    {
        yield return player.StartCoroutine(player.Dash());

        player.isDash = false;

        player.playerCollider.enabled = true;
    }

    private void InitPlayerCondition()
    {
        player.playerAnim.ResetTrigger("Z");

        player.playerAnim.ResetTrigger("X");

        player.attackCheck.isAttack = false;
    }
}
