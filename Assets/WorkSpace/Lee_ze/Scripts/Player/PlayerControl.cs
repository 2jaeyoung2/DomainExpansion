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

    public GameObject[] swords;

    public GameObject coffin;


    Vector3 cursorPos;

    Vector3 direction;

    Coroutine rotationCor;

    public bool isDash = false;

    public bool isHit = false;

    public float dashCost = 15;

    private void Start()
    {
        PlayerNavMeshAgentSettings();

        mousePos.OnDirectionChanged += GoToDestination;

        WeaponsOff();

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
        float speed = 30f;

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

    #region Dash 관련 스크립트

    public void OnDash(InputAction.CallbackContext ctx) // 'Left Shift' 바인딩
    {
        if (ctx.phase == InputActionPhase.Started)
        {
            if (playerStats.PlayerStamina >= dashCost) // 스테미나가 15 이상일 때
            {
                isDash = true;
            }
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

    #region 피격 관련 스크립트

    public void GetHit(int damage, int downCountStack)
    {
        isHit = true;

        playerStats.PlayerHP -= damage;

        if (playerStats.PlayerHP <= 0)
        {
            ChangeStateTo(new DeadState());
        }

        playerStats.PlayerDownCount += downCountStack;

        playerAnim.SetTrigger("Hit");
    }

    public void EndHit() // 피격 애니메이션 끝 부분에 호출되는 이벤트 함수
    {
        isHit = false;
    }

    #endregion

    #region 무기 관련 스크립트

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
