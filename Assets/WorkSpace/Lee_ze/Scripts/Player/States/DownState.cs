using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownState : IPlayerState
{
    private PlayerControl player;

    public void EnterState(PlayerControl player)
    {
        this.player = player;

        this.player.isDown = true;

        this.player.playerCollider.enabled = false; // 쓰러질 때 콜라이더 끄기

        this.player.playerAnim.SetTrigger("Down");

        this.player.StartCoroutine(Recovery());
    }

    public void UpdateState()
    {

    }

    public void ExitState()
    {
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

        // 시작됐다면 애니메이션이 끝 날 때 까지 기다린 뒤에
        yield return new WaitUntil(() => 
        player.playerAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

        // Idle로 이동
        player.ChangeStateTo(new IdleState());
    }
}
