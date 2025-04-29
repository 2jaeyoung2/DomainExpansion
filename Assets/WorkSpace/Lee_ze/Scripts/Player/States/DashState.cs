using System.Collections;
using UnityEngine;

public class DashState : IPlayerState
{
    private PlayerControl player;

    public void EnterState(PlayerControl player)
    {
        this.player = player;

        this.player.playerCollider.enabled = false; // 구를 때 콜라이더 끄기

        this.player.playerAnim.SetTrigger("IsDash");

        InitPlayerCondition();

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

        player.playerCollider.enabled = true; // 구르기 끝나면 콜라이더 켜기

        Debug.Log("dash end");
    }

    private void InitPlayerCondition()
    {
        player.playerAnim.ResetTrigger("Z");

        player.playerAnim.ResetTrigger("X");

        player.attackCheck.isAttack = false;
    }
}
