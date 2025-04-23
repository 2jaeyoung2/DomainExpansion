using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private Animator playerAnim;

    [SerializeField]
    private Sword swordStat;

    public bool isAttack = false;

    public void OnAttack_Z(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started)
        {
            playerAnim.SetTrigger("Z");

            isAttack = true;
        }
    }

    public void OnAttack_X(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started)
        {
            playerAnim.SetTrigger("X");

            isAttack = true;
        }
    }

    public void IsAttackToFalse() // �ִϸ��̼� �̺�Ʈ�Լ� ȣ���
    {
        isAttack = false;
    }

    public void NoMoreAttack() // TODO: IDLE Ȥ�� RUN �ִϸ��̼����� ���� �� ���� �ִϸ��̼ǿ� ���ε��ؾ� �� �Լ�
    {
        playerAnim.ResetTrigger("Z");

        playerAnim.ResetTrigger("X");
    }

    public void SetDamage(AnimationEvent myEvent)
    {
        swordStat.damage = myEvent.intParameter;
    }

    public void SetDownCount(AnimationEvent myEvent)
    {
        swordStat.downCount = myEvent.intParameter;
    }
}
