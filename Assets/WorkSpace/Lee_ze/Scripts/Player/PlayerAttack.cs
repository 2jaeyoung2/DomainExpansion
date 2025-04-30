using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PlayerAttack : MonoBehaviourPun
{
    [SerializeField]
    private PlayerControl player;

    [SerializeField]
    private Sword swordStat;

    public bool isAttack = false;

    public void OnAttack_Z(InputAction.CallbackContext ctx)
    {
        if (photonView.IsMine == false)
        {
            return;
        }

        if (ctx.phase == InputActionPhase.Started)
        {
            if (player.isDash == true) return;

            player.mousePos.TempGetMouseCursorPosition();

            player.SetPlayerRotation();

            player.playerAnim.SetTrigger("Z");

            isAttack = true;
        }
    }

    public void OnAttack_X(InputAction.CallbackContext ctx)
    {
        if (photonView.IsMine == false)
        {
            return;
        }

        if (ctx.phase == InputActionPhase.Started)
        {
            if (player.isDash == true) return;

            player.mousePos.TempGetMouseCursorPosition();

            player.SetPlayerRotation();

            player.playerAnim.SetTrigger("X");

            isAttack = true;
        }
    }

    public void IsAttackToFalse() // �ִϸ��̼� �̺�Ʈ�Լ� ȣ���
    {
        isAttack = false;
    }

    public void NoMoreAttack() // TODO: IDLE Ȥ�� RUN �ִϸ��̼����� ���� �� ���� �ִϸ��̼ǿ� ���ε��ؾ� �� �Լ�
    {
        player.playerAnim.ResetTrigger("Z");

        player.playerAnim.ResetTrigger("X");
    }

    public void SetDamage(AnimationEvent myEvent) // TODO: �ִϸ��̼� Ư�� �����ӿ� ������ ���� ���� ��
    {
        swordStat.damage = myEvent.intParameter;
    }

    public void SetDownCount(AnimationEvent myEvent) // TODO: �ִϸ��̼� Ư�� �����ӿ� ������ ���� ���� ��
    {
        swordStat.downCount = myEvent.intParameter;
    }
}
