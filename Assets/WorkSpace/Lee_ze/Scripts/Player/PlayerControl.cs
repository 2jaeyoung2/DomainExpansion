using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public MouseCursorPosition mousePos;

    public NavMeshAgent agent;

    private IPlayerState currentState;

    public Animator playerAnim;

    private void Start()
    {
        PlayerMovementSettings();

        mousePos.OnDirectionChanged += GoToDestination;

        // Idle State로 초기화
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

    private void PlayerMovementSettings()
    {
        agent.updateRotation = false;

        agent.acceleration = 999f;

        agent.angularSpeed = 999f;

        agent.speed = 8f;
    }

    private void GoToDestination() // 마우스 우클릭 할 때 호출 됨(observer pattern 사용)
    {
        if (Vector3.Distance(agent.destination, mousePos.hit.point) > 0.1f)
        {
            SetPlayerRotation();

            agent.SetDestination(mousePos.hit.point);
        }
    }

    private void SetPlayerRotation() // 이동하는 방향으로 rotate
    {
        Vector3 targetPos = new Vector3(mousePos.hit.point.x, transform.position.y, mousePos.hit.point.z);

        Vector3 direction = (targetPos - transform.position).normalized;

        transform.rotation = Quaternion.LookRotation(direction);
    }

    public void OnStop(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started)
        {
            agent.isStopped = true;

            agent.ResetPath();
        }
    }

    #endregion

    private void OnDisable()
    {
        mousePos.OnDirectionChanged -= GoToDestination;
    }
}
