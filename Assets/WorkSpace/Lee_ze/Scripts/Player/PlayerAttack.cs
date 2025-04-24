using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private PlayerControl player;

    [SerializeField]
    private Sword swordStat;

    public bool isAttack = false;

    public void OnAttack_Z(InputAction.CallbackContext ctx)
    {
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
        if (ctx.phase == InputActionPhase.Started)
        {
            if (player.isDash == true) return;

            player.mousePos.TempGetMouseCursorPosition();

            player.SetPlayerRotation();

            player.playerAnim.SetTrigger("X");

            isAttack = true;
        }
    }

    public void IsAttackToFalse() // 애니메이션 이벤트함수 호출용
    {
        isAttack = false;
    }

    public void NoMoreAttack() // TODO: IDLE 혹은 RUN 애니메이션으로 가기 전 공격 애니메이션에 바인딩해야 할 함수
    {
        player.playerAnim.ResetTrigger("Z");

        player.playerAnim.ResetTrigger("X");
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
