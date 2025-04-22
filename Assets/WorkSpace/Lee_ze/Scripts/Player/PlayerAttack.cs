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
            Debug.Log("z");

            playerAnim.SetTrigger("Z");

            isAttack = true;
        }
    }

    public void OnAttack_X(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started)
        {
            Debug.Log("x");

            playerAnim.SetTrigger("X");

            isAttack = true;
        }
    }

    public void IsAttackToFalse() // 애니메이션 이벤트함수 호출용
    {
        isAttack = false;
    }

    public void NoMoreAttack()
    {
        playerAnim.ResetTrigger("Z");

        playerAnim.ResetTrigger("X");
    }

    public void SetDamage(AnimationEvent myEvent)
    {
        swordStat.damage = myEvent.intParameter;

        Debug.Log(swordStat.damage);
    }

    public void SetDownCount(AnimationEvent myEvent)
    {
        swordStat.downCount = myEvent.intParameter;

        Debug.Log(swordStat.downCount);
    }
}
