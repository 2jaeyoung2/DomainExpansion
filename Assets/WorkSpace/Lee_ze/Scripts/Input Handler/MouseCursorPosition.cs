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

    private bool isMouseRightHeld = false;

    private void Update()
    {
        GetMouseCursorPosition();
    }

    public void OnMouseRightButtonClick(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started)
        {
            isMouseRightHeld = true;
            Debug.Log("����");
        }
        else if (ctx.phase == InputActionPhase.Canceled)
        {
            isMouseRightHeld = false;
            Debug.Log("��");
        }
    }

    // ���콺 Ŀ�� ��ġ ��������. �� hit.point�� Ŀ�� ��ǥ ������ �� ����.
    private void GetMouseCursorPosition()
    {
        if (isMouseRightHeld == true)
        {
            Debug.Log(1);
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.green);

                OnDirectionChanged?.Invoke();
                Debug.Log("invoked");
            }
        }
    }
}
