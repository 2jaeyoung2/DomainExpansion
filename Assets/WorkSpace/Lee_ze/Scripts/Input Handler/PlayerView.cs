using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerView : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private Transform playerPos;

    [Header("Camera Movement Settings")]

    [SerializeField]
    private float cameraSpeed = 10f;

    [SerializeField]
    private float edgePixel = 50f;

    [Header("Movement Boundary")]

    [SerializeField]
    private Vector2 xLimit = new Vector2(-12f, 12f);

    [SerializeField]
    private Vector2 zLimit = new Vector2(-15f, 25f);

    private Vector2 mousePos;

    private Coroutine moveCamHorizontalCor = null;

    private Coroutine moveCamVerticalCor = null;

    public void OnMouseMove(InputAction.CallbackContext ctx)
    {
        mousePos = ctx.ReadValue<Vector2>();

        // 만약 범위 안에 있으면 return;
        if (IsInHorizontalBoundary() == true && IsInVerticalBoundary() == true)
        {
            return;
        }

        if (moveCamHorizontalCor == null)
        {
            moveCamHorizontalCor = StartCoroutine(MoveCamHorizontal(mousePos));
        }

        if (moveCamVerticalCor == null)
        {
            moveCamVerticalCor = StartCoroutine(MoveCamVertical(mousePos));
        }
    }

    private IEnumerator MoveCamHorizontal(Vector2 mousePos)
    {
        while (IsInHorizontalBoundary() == false)
        {
            Vector3 moveDir = Vector3.zero;

            if (mousePos.x <= edgePixel)
            {
                moveDir += Vector3.left;
            }

            else if (mousePos.x >= Screen.width - edgePixel)
            {
                moveDir += Vector3.right;
            }

            Vector3 camPos = mainCamera.transform.position + moveDir.normalized * cameraSpeed * Time.deltaTime;

            camPos.x = Mathf.Clamp(camPos.x, xLimit.x, xLimit.y);

            camPos.z = Mathf.Clamp(camPos.z, zLimit.x, zLimit.y);

            mainCamera.transform.position = camPos;

            yield return null;
        }

        moveCamHorizontalCor = null;
    }

    private IEnumerator MoveCamVertical(Vector2 mousePos)
    {
        while (IsInVerticalBoundary() == false)
        {
            Vector3 moveDir = Vector3.zero;

            if (mousePos.y <= edgePixel)
            {
                moveDir += Vector3.back;
            }

            else if (mousePos.y >= Screen.height - edgePixel)
            {
                moveDir += Vector3.forward;
            }

            Vector3 camPos = mainCamera.transform.position + moveDir.normalized * cameraSpeed * Time.deltaTime;

            camPos.x = Mathf.Clamp(camPos.x, xLimit.x, xLimit.y);

            camPos.z = Mathf.Clamp(camPos.z, zLimit.x, zLimit.y);

            mainCamera.transform.position = camPos;

            yield return null;
        }

        moveCamVerticalCor = null;
    }

    private bool IsInHorizontalBoundary()
    {
        return mousePos.x < Screen.width - edgePixel && mousePos.x > edgePixel;
    }

    private bool IsInVerticalBoundary()
    {
        return mousePos.y < Screen.height - edgePixel && mousePos.y > edgePixel;
    }

    public void OnToPlayer(InputAction.CallbackContext ctx) // 스페이스바 바인딩
    {
        if (ctx.phase == InputActionPhase.Started)
        {
            CamToPlayer();
        }
    }

    private void CamToPlayer() // 카메라를 플레이어 중심으로 이동
    {
        if (playerPos != null)
        {
            Vector3 newPos = playerPos.position;

            newPos.x = playerPos.position.x;

            newPos.y = mainCamera.transform.position.y;

            newPos.z = playerPos.position.z - 7f;

            mainCamera.transform.position = newPos;
        }
    }
}
