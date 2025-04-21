using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class PlayerControl : MonoBehaviour, IDamageable
{
    public PlayerStatistics playerStats;

    private IPlayerState currentState;

    public MouseCursorPosition mousePos;

    public NavMeshAgent agent;

    public Collider playerCollider;

    public Animator playerAnim;

    public PlayerAttack attackCheck;


    Vector3 cursorPos;

    Vector3 direction;

    public bool isDash = false;

    public bool isHit = false;

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
        if (isDash == true) // ������ ���� �� �̵� �Ұ�
        {
            return;
        }

        if (attackCheck.isAttack == true) // ���� ���� �� �̵� �Ұ�
        {
            return;
        }

        if (Vector3.Distance(agent.destination, mousePos.hit.point) > 0.1f) // agent�� �������� ���콺 ��Ŭ�� ��ġ ���̰� 0.1���� Ŭ ���
        {
            SetPlayerRotation();

            agent.SetDestination(mousePos.hit.point);
        }
    }

    private void SetPlayerRotation() // �̵��ϴ� �������� rotate
    {
        cursorPos = new Vector3(mousePos.hit.point.x, transform.position.y, mousePos.hit.point.z);

        direction = (cursorPos - transform.position).normalized;

        transform.rotation = Quaternion.LookRotation(direction);
    }

    public void OnStop(InputAction.CallbackContext ctx) // 'S' ���ε�
    {
        if (ctx.phase == InputActionPhase.Started)
        {
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

    #region �ǰ� ���� ��ũ��Ʈ

    public void GetHit(int damage, int downCountStack)
    {
        isHit = true;

        damage = 0; // <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< �ӽ��ڵ�

        playerStats.PlayerHP -= damage;

        playerStats.PlayerDownCount += downCountStack;

        playerAnim.SetTrigger("Hit");
    }

    public void EndHit() // �ǰ� �ִϸ��̼� �� �κп� ȣ��Ǵ� �̺�Ʈ �Լ�
    {
        isHit = false;
    }

    #endregion

    private void OnDisable()
    {
        mousePos.OnDirectionChanged -= GoToDestination;
    }
}
