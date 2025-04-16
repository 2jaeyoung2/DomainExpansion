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



    Vector3 targetPos;

    Vector3 direction;

    public bool isMoving = false;

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

    #region MoveMents �̵� ���� ��ũ��Ʈ

    private void PlayerNavMeshAgentSettings()
    {
        agent.updateRotation = false;

        agent.acceleration = 999f;

        agent.angularSpeed = 999f;

        agent.speed = 5f;
    }

    private void GoToDestination() // ���콺 ��Ŭ�� �� �� ȣ�� ��(observer pattern ���)
    {
        if (isDash == true)
        {
            return;
        }
        if (Vector3.Distance(agent.destination, mousePos.hit.point) > 0.1f)
        {
            SetPlayerRotation();

            agent.SetDestination(mousePos.hit.point);
        }
    }

    private void SetPlayerRotation() // �̵��ϴ� �������� rotate
    {
        targetPos = new Vector3(mousePos.hit.point.x, transform.position.y, mousePos.hit.point.z);

        direction = (targetPos - transform.position).normalized;

        transform.rotation = Quaternion.LookRotation(direction);
    }

    public void OnStop(InputAction.CallbackContext ctx) // 'S' ���ε�
    {
        if (ctx.phase == InputActionPhase.Started)
        {
            agent.isStopped = true;

            agent.ResetPath();
        }
    }

    #endregion

    #region Dash ���� ��ũ��Ʈ

    public void OnDash(InputAction.CallbackContext ctx) // 'Left Shift' ���ε�
    {
        if (ctx.phase == InputActionPhase.Started)
        {
            isDash = true;
        }
    }

    public IEnumerator Dash()
    {
        agent.ResetPath();

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

    public void OffDash() // �ִϸ��̼� Ư�� �����ӿ� �̺�Ʈ�� ȣ��
    {
        isDash = false;
    }

    #endregion

    private void OnDisable()
    {
        mousePos.OnDirectionChanged -= GoToDestination;
    }
}
