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


    // V 플레이어 상태

    public bool isDead = false;

    public bool isDown = false;

    public bool isDash = false;

    public bool isHit = false;


    // V 스킬 코스트

    public float dashCost = 1f;

    public float manaBreakCost = 40;

    private void Start()
    {
        PlayerNavMeshAgentSettings();

        mousePos.OnDirectionChanged += GoToDestination;

        WeaponsOff();

        ChangeStateTo(new IdleState());

        // 여기에 ISkill[] 배열로 스킬 설정
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
        if (isDash == true || isHit == true || isDead == true || isDown == true) // 구르기 중, 피격 시, 사망 시, 다운 시 이동 불가
        {
            return;
        }
        
        if (Vector3.Distance(agent.destination, mousePos.hit.point) > 0.1f) // agent의 목적지와 마우스 우클릭 위치 차이가 0.1보다 클 경우
        {
            SetPlayerRotation();

            agent.SetDestination(mousePos.hit.point);
        }
    }

    public void SetPlayerRotation() // 이동하는 방향으로 rotate
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

    private IEnumerator RotateToDirection(Quaternion targetRotation) // 부드러운 회전
    {
        float speed = 25f;

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);

            yield return null;
        }

        transform.rotation = targetRotation;
    }

    public void OnStop(InputAction.CallbackContext ctx) // 'S' 바인딩
    {
        if (ctx.phase == InputActionPhase.Started)
        {
            agent.ResetPath();
        }
    }

    #endregion

    #region 스킬 관련 스크립트

    public void OnDash(InputAction.CallbackContext ctx) // 'Left Shift' 바인딩
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

            C_SetSkill(new Dash()); // 무슨 스킬 받아왔는지에 따라 new (스킬명)() 해줘야 함.

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

    public void EndDash() // 애니메이션 특정 프레임에 이벤트성 호출
    {
        isDash = false;
    }

    #endregion

    #region 무기 관련 스크립트

    // 공격 애니메이션 특정 프레임에 호출되는 이벤트 함수

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
