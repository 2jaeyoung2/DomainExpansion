using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public MouseCursorPosition mousePos;

    public NavMeshAgent agent;

    private IPlayerState currentState;

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

    private void GoToDestination()
    {
        if (Vector3.Distance(agent.destination, mousePos.hit.point) > 0.1f)
        {
            agent.SetDestination(mousePos.hit.point);
        }
    }

    private void PlayerMovementSettings()
    {
        agent.updateRotation = false;

        agent.acceleration = 999f;

        agent.angularSpeed = 999f;

        agent.speed = 10f;
    }

    private void OnDisable()
    {
        mousePos.OnDirectionChanged -= GoToDestination;
    }
}
