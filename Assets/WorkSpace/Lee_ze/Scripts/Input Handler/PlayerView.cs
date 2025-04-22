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
    private float cameraSpeed = 30f;

    [SerializeField]
    private float edgePixel = 50f;

    [Header("Movement Boundary")]

    [SerializeField]
    private Vector2 xLimit = new Vector2(-12f, 12f);

    [SerializeField]
    private Vector2 zLimit = new Vector2(-15f, 25f);

    private void Update()
    {
        Vector3 moveDir = Vector3.zero;

        Vector3 mousePos = Input.mousePosition;

        if (mousePos.x <= edgePixel)
        {
            moveDir += Vector3.left;
        }

        else if (mousePos.x >= Screen.width - edgePixel)
        {
            moveDir += Vector3.right;
        }

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
    }

    public void OnToPlayer(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started)
        {
            CamToPlayer();
        }
    }

    private void CamToPlayer()
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
