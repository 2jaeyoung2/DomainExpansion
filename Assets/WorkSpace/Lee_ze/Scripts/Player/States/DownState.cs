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

        this.player.playerCollider.enabled = false; // ������ �� �ݶ��̴� ����

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

        player.playerCollider.enabled = true; // �ݶ��̴� �ѱ�
    }

    private IEnumerator Recovery()
    {
        yield return new WaitForSeconds(2.5f); // �Ѿ����� 2.5�� ��ٸ�

        player.playerAnim.SetTrigger("StandUp"); // �Ͼ�� Ʈ���� �ߵ�

        // �ִϸ��̼��� ���۵� �� ���� ��ٸ���
        yield return new WaitUntil(() => 
        player.playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Fall down_End"));

        // ���۵ƴٸ� �ִϸ��̼��� �� �� �� ���� ��ٸ� �ڿ�
        yield return new WaitUntil(() => 
        player.playerAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

        // Idle�� �̵�
        player.ChangeStateTo(new IdleState());
    }
}
