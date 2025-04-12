using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private MouseCursorPosition mousePos;

    [SerializeField]
    private NavMeshAgent agent;

    private void Start()
    {
        PlayerMovementSettings();

        mousePos.OnDirectionChanged += GoToDestination;
    }

    public void GoToDestination()
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
