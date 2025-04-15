using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

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

    #region MoveMents
    private void GoToDestination()
    {
        if (Vector3.Distance(agent.destination, mousePos.hit.point) > 0.1f)
        {
            SetPlayerRotation();

            agent.SetDestination(mousePos.hit.point);
        }
    }

    private void PlayerMovementSettings()
    {
        agent.updateRotation = false;

        agent.acceleration = 999f;

        agent.angularSpeed = 999f;

        agent.speed = 8f;
    }

    private void SetPlayerRotation()
    {
        Vector3 targetPos = new Vector3(mousePos.hit.point.x, transform.position.y, mousePos.hit.point.z);

        Vector3 direction = (targetPos - transform.position).normalized;

        transform.rotation = Quaternion.LookRotation(direction);
    }
    #endregion

    private void OnDisable()
    {
        mousePos.OnDirectionChanged -= GoToDestination;
    }
}
