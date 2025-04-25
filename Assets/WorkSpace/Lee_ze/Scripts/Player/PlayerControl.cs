using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using System;
using Photon.Realtime;
using Unity.VisualScripting;

public class PlayerControl : MonoBehaviour
{
    public PlayerStatistics playerStats;

    private IPlayerState currentState;

    private ISkill[] skill = new ISkill[2];

    public MouseCursorPosition mousePos;



    public NavMeshAgent agent;

    public Collider playerCollider;

    public Animator playerAnim;

    public PlayerAttack attackCheck;

    public GameObject[] swords;

    public GameObject coffin;

    

    private Vector3 cursorPos;

    public Vector3 direction;

    private Coroutine rotationCor;


    // V �÷��̾� ����

    public bool isDead = false;

    public bool isDown = false;

    public bool isDash = false;

    public bool isHit = false;


    // V ��ų �ڽ�Ʈ

    public float dashCost = 1f;

    public float manaBreakCost = 40;

    private void Start()
    {
        PlayerNavMeshAgentSettings();

        mousePos.OnDirectionChanged += GoToDestination;

        WeaponsOff();

        ChangeStateTo(new IdleState());

        // ���⿡ ISkill[] �迭�� ��ų ����
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
        if (isDash == true || isHit == true || isDead == true || isDown == true) // ������ ��, �ǰ� ��, ��� ��, �ٿ� �� �̵� �Ұ�
        {
            return;
        }
        
        if (Vector3.Distance(agent.destination, mousePos.hit.point) > 0.1f) // agent�� �������� ���콺 ��Ŭ�� ��ġ ���̰� 0.1���� Ŭ ���
        {
            SetPlayerRotation();

            agent.SetDestination(mousePos.hit.point);
        }
    }

    public void SetPlayerRotation() // �̵��ϴ� �������� rotate
    {
        cursorPos = new Vector3(mousePos.hit.point.x, transform.position.y, mousePos.hit.point.z);

        direction = (cursorPos - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            if (rotationCor != null)
            {
                StopCoroutine(rotationCor);
            }

            rotationCor = StartCoroutine(RotateToDirection(targetRotation));
        }
    }

    private IEnumerator RotateToDirection(Quaternion targetRotation) // �ε巯�� ȸ��
    {
        float speed = 25f;

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);

            yield return null;
        }

        transform.rotation = targetRotation;
    }

    public void OnStop(InputAction.CallbackContext ctx) // 'S' ���ε�
    {
        if (ctx.phase == InputActionPhase.Started)
        {
            agent.ResetPath();
        }
    }

    #endregion

    #region ��ų ���� ��ũ��Ʈ

    public void OnDash(InputAction.CallbackContext ctx) // 'Left Shift' ���ε�
    {
        if (ctx.phase == InputActionPhase.Started)
        {
            if (isDash == true)
            {
                return;
            }
            if (playerStats.PlayerCurrentStamina >= dashCost)
            {
                isDash = true;
            }
        }
    }

    private void C_SetSkill(ISkill skillName)
    {
        skill[0] = skillName;
    }

    private void D_SetSkill(ISkill skillName)
    {
        skill[1] = skillName;
    }

    public void On_C_Skill(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started)
        {
            if (isDash == true)
            {
                return;
            }

            C_SetSkill(new Dash()); // ���� ��ų �޾ƿԴ����� ���� new (��ų��)() ����� ��.

            Debug.Log(1);

            skill[0].ActiveThisSkill(this);
        }
    }

    public void On_D_Skill(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started)
        {

        }
    }

    public void EndDash() // �ִϸ��̼� Ư�� �����ӿ� �̺�Ʈ�� ȣ��
    {
        isDash = false;
    }

    #endregion

    #region ���� ���� ��ũ��Ʈ

    // ���� �ִϸ��̼� Ư�� �����ӿ� ȣ��Ǵ� �̺�Ʈ �Լ�

    public void WeaponsOn()
    {
        foreach (var sword in swords)
        {
            sword.GetComponent<Collider>().enabled = true;
        }
    }

    public void WeaponsOff()
    {
        foreach (var sword in swords)
        {
            sword.GetComponent<Collider>().enabled = false;
        }
    }

    #endregion

    private void OnDisable()
    {
        mousePos.OnDirectionChanged -= GoToDestination;
    }
}
