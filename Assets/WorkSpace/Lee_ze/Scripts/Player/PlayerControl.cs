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

        // Idle State�� �ʱ�ȭ
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

    #region MoveMents �̵� ���� ��ũ��Ʈ

    private void PlayerMovementSettings()
    {
        agent.updateRotation = false;

        agent.acceleration = 999f;

        agent.angularSpeed = 999f;

        agent.speed = 8f;
    }

    private void GoToDestination() // ���콺 ��Ŭ�� �� �� ȣ�� ��(observer pattern ���)
    {
        if (Vector3.Distance(agent.destination, mousePos.hit.point) > 0.1f)
        {
            SetPlayerRotation();

            agent.SetDestination(mousePos.hit.point);
        }
    }

    private void SetPlayerRotation() // �̵��ϴ� �������� rotate
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
