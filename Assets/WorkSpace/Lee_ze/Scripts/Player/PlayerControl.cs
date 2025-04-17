using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class PlayerControl : MonoBehaviour
{
    private IPlayerState currentState;

    public MouseCursorPosition mousePos;

    public NavMeshAgent agent;

    public Collider playerCollider;

    public Animator playerAnim;

    public PlayerAttack attackCheck;


    Vector3 targetPos;

    Vector3 direction;

    public bool isCancled = false;

    public bool isDash = false;

    private void Start()
    {
        PlayerNavMeshAgentSettings();

        mousePos.OnDirectionChanged += GoToDestination;

        ChangeStateTo(new IdleState());
    }

    private void Update()
    {
        currentState?.UpdateState();
    }

    public void ChangeStateTo(IPlayerState nextState)
    {
        currentState?.ExitState();

        currentState = nextState;

        currentState.EnterState(this);
    }

    #region MoveMents 이동 관련 스크립트

    private void PlayerNavMeshAgentSettings()
    {
        agent.updateRotation = false;

        agent.acceleration = 999f;

        agent.angularSpeed = 999f;

        agent.speed = 5f;
    }

    private void GoToDestination() // 마우스 우클릭 할 때 호출 됨(observer pattern 사용)
    {
        if (isDash == true) // 구르기 중일 때 이동 불가
        {
            return;
        }

        if (attackCheck.isAttack == true) // 공격 중일 때 이동 불가
        {
            return;
        }

        if (Vector3.Distance(agent.destination, mousePos.hit.point) > 0.1f) // agent의 목적지와 마우스 우클릭 위치 차이가 0.1보다 클 경우
        {
            SetPlayerRotation();

            agent.SetDestination(mousePos.hit.point);
        }
    }

    private void SetPlayerRotation() // 이동하는 방향으로 rotate
    {
        targetPos = new Vector3(mousePos.hit.point.x, transform.position.y, mousePos.hit.point.z);

        direction = (targetPos - transform.position).normalized;

        transform.rotation = Quaternion.LookRotation(direction);
    }

    public void OnStop(InputAction.CallbackContext ctx) // 'S' 바인딩
    {
        if (ctx.phase == InputActionPhase.Started)
        {
            isCancled = true;

            agent.ResetPath();
        }

        if (ctx.phase == InputActionPhase.Canceled)
        {
            attackCheck.isAttack = false;

            isCancled = false;
        }
    }

    #endregion

    #region Dash 관련 스크립트

    public void OnDash(InputAction.CallbackContext ctx) // 'Left Shift' 바인딩
    {
        if (ctx.phase == InputActionPhase.Started)
        {
            isDash = true;
        }
    }

    public IEnumerator Dash()
    {
        mousePos.TempGetMouseCursorPosition();

        SetPlayerRotation();

        float dashSpeed = 7f;

        float timer = 0f;

        while (timer < 0.7f)
        {
            transform.position += direction.normalized * dashSpeed * Time.deltaTime;

            timer += Time.deltaTime;

            yield return null;
        }
    }

    public void OffDash() // 애니메이션 특정 프레임에 이벤트성 호출
    {
        isDash = false;
    }

    #endregion

    private void OnDisable()
    {
        mousePos.OnDirectionChanged -= GoToDestination;
    }
}
