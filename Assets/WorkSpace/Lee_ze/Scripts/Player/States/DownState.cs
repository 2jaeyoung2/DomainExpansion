using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownState : IPlayerState
{
    private PlayerControl player;

    public void EnterState(PlayerControl player)
    {
        this.player = player;

        this.player.agent.isStopped = true;

        this.player.playerCollider.enabled = false; // 쓰러질 때 콜라이더 끄기

        InitPlayerCondition();

        this.player.playerAnim.SetTrigger("Down"); // 쓰러지는 애니메이션

        this.player.StartCoroutine(Recovery());

        Debug.Log("down");
    }

    public void UpdateState()
    {

    }

    public void ExitState()
    {
        player.agent.isStopped = false;

        player.isDown = false;

        player.isHit = false;

        player.playerCollider.enabled = true; // 콜라이더 켜기
    }

    private IEnumerator Recovery()
    {
        yield return new WaitForSeconds(2.5f); // 넘어지고 2.5초 기다림

        player.playerAnim.SetTrigger("StandUp"); // 일어서는 트리거 발동

        // 애니메이션이 시작될 때 까지 기다리고
        yield return new WaitUntil(() => 
        player.playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Fall down_End"));

        // 애니메이션이 끝날 떄 까지 기다림
        yield return new WaitUntil(() =>
        player.playerAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.78f);

        // ----> State Change
        if (player.agent.hasPath == true)
        {
            player.ChangeStateTo(new RunState());
        }

        if (player.agent.hasPath == false)
        {
            player.ChangeStateTo(new IdleState());
        }
    }

    private void InitPlayerCondition()
    {
        player.isDown = true;

        player.isDash = false;

        player.isHit = false;

        player.attackCheck.isAttack = false;

        player.playerAnim.SetBool("IsRun", false);
    }
}
