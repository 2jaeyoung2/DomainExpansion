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

        this.player.playerCollider.enabled = false; // ������ �� �ݶ��̴� ����

        InitPlayerCondition();

        this.player.playerAnim.SetTrigger("Down"); // �������� �ִϸ��̼�

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

        player.playerCollider.enabled = true; // �ݶ��̴� �ѱ�
    }

    private IEnumerator Recovery()
    {
        yield return new WaitForSeconds(2.5f); // �Ѿ����� 2.5�� ��ٸ�

        player.playerAnim.SetTrigger("StandUp"); // �Ͼ�� Ʈ���� �ߵ�

        // �ִϸ��̼��� ���۵� �� ���� ��ٸ���
        yield return new WaitUntil(() => 
        player.playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Fall down_End"));

        // �ִϸ��̼��� ���� �� ���� ��ٸ�
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
