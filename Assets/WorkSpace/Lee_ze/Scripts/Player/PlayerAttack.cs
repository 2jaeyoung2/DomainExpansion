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

    public void IsAttackToFalse() // 애니메이션 이벤트함수 호출용
    {
        isAttack = false;
    }

    public void NoMoreAttack() // TODO: IDLE 혹은 RUN 애니메이션으로 가기 전 공격 애니메이션에 바인딩해야 할 함수
    {
        player.playerAnim.ResetTrigger("Z");

        player.playerAnim.ResetTrigger("X");
    }

    public void SetDamage(AnimationEvent myEvent) // TODO: 애니메이션 특정 프레임에 설정된 값을 가져 옴
    {
        swordStat.damage = myEvent.intParameter;
    }

    public void SetDownCount(AnimationEvent myEvent) // TODO: 애니메이션 특정 프레임에 설정된 값을 가져 옴
    {
        swordStat.downCount = myEvent.intParameter;
    }
}
