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

    private Ray ray;

    public RaycastHit hit;

    public Vector3 previousHitPoint;

    private bool isMouseRightHeld = false;

    private int floorLayerMask;

    private void Start()
    {
        floorLayerMask = LayerMask.GetMask("FLOOR");
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
