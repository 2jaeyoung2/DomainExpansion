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
        mousePos.OnDirectionChanged += GoToDestination;
    }

    public void GoToDestination()
    {
        if (agent.remainingDistance > 0)
        {
            agent.SetDestination(mousePos.hit.point);

            Debug.Log(mousePos.hit.point);
        }
    }

    private void OnDisable()
    {
        mousePos.OnDirectionChanged -= GoToDestination;
    }
}
