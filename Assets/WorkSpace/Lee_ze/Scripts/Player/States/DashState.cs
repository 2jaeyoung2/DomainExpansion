using System.Collections;
using UnityEngine;

public class DashState : IPlayerState
{
    private PlayerControl player;

    public void EnterState(PlayerControl player)
    {
        this.player = player;

        this.player.playerCollider.enabled = false; // ���� �� �ݶ��̴� ����

        this.player.agent.ResetPath();

        this.player.StartCoroutine(this.player.Dash());

        this.player.playerAnim.SetTrigger("IsDash");

        PayDashCost();

        this.player.StartCoroutine(ResetIsDash());

        Debug.Log("dash start");
    }

    public void UpdateState()
    {


        // ----> State Change
        if (player.isDash == false)
        {
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

        player.playerStats.GetDashDamage(10); // �뽬 ��� �� �ڽ� ü�� -10(���� ��ġ)
    }

    private IEnumerator ResetIsDash()
    {
        yield return new WaitForSeconds(1f);

        player.isDash = false;

        Debug.Log(player.isDash);
    }
}
