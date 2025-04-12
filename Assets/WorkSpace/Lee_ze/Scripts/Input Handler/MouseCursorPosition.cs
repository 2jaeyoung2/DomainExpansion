using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseCursorPosition : MonoBehaviour
{
    public event Action OnDirectionChanged;

    [SerializeField]
    private Transform mousePosInit; // 플레이어 프리팹 넣어서 초기 hit.point 위치 정해줌

    private Ray ray;

    public RaycastHit hit;

    private Vector3 previousHitPoint;

    private bool isMouseRightHeld = false;

    private int floorLayerMask;

    private void Start()
    {
        floorLayerMask = LayerMask.GetMask("FLOOR");

        previousHitPoint = mousePosInit.position;
    }

    private void Update()
    {
        GetMouseCursorPosition();
    }

    public void OnMouseRightButtonClick(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started)
        {
            isMouseRightHeld = true;
        }
        else if (ctx.phase == InputActionPhase.Canceled)
        {
            isMouseRightHeld = false;
        }
    }

    // 마우스 커서 위치 가져오기. ※ hit.point로 커서 좌표 가져올 수 있음.
    private void GetMouseCursorPosition()
    {
        if (isMouseRightHeld == true)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100, floorLayerMask))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.green);

                if (previousHitPoint != hit.point)
                {
                    previousHitPoint = hit.point;

                    OnDirectionChanged?.Invoke();
                }
            }
        }
    }
}
