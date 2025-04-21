using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private Animator playerAnim;

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

    public void IsAttackToFalse()
    {
        isAttack = false;
    }

    public void NoMoreAttack()
    {
        playerAnim.ResetTrigger("Z");

        playerAnim.ResetTrigger("X");
    }
}
