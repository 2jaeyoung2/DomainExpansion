using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : IPlayerState
{
    private PlayerControl player;

    public void EnterState(PlayerControl player)
    {
        this.player = player;

        this.player.playerCollider.enabled = false; // ���� �� �ݶ��̴� ����

        this.player.StartCoroutine(this.player.Dash());

        this.player.playerAnim.SetTrigger("IsDash");
    }

    public void UpdateState()
    {


        // ----> State Change
        if (player.isDash == false)
        {
            // if(����Ű'z'�� ���ȴٸ�) Attack State�� �̵�
            // �ƴϸ� ������ Idle state�� �̵�

            if (player.agent.velocity.magnitude < 0.1f)
            {
                player.ChangeStateTo(new IdleState());
            }
            if (player.agent.velocity.magnitude > 0.1f)
            {
                player.ChangeStateTo(new RunState());
            }
        }
    }

    public void ExitState()
    {
        if (player.isMoving == false)
        {
            player.playerAnim.SetBool("IsMove", false);
        }
        else if (player.isMoving == true)
        {
            player.playerAnim.SetBool("IsMove", true);
        }

        player.playerCollider.enabled = true; // �ݶ��̴� �ѱ�
    }
}
