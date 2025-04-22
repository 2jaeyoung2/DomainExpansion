using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    [Header("Camera Movement Settings")]

    [SerializeField]
    private float moveSpeed = 10f;

    [SerializeField]
    private float edgeThreshold = 10f;

    [Header("Movement Bounds")]

    [SerializeField]
    private Vector2 xLimit = new Vector2(-50f, 50f);

    [SerializeField]
    private Vector2 zLimit = new Vector2(-50f, 50f);

    private void Update()
    {
        Vector3 moveDir = Vector3.zero;

        Vector3 mousePos = Input.mousePosition;

        if (mousePos.x <= edgeThreshold)
        {
            moveDir += Vector3.left;
        }

        else if (mousePos.x >= Screen.width - edgeThreshold)
        {
            moveDir += Vector3.right;
        }

        if (mousePos.y <= edgeThreshold)
        {
            moveDir += Vector3.back;
        }

        else if (mousePos.y >= Screen.height - edgeThreshold)
        {
            moveDir += Vector3.forward;
        }

        Vector3 newPos = mainCamera.transform.position + moveDir.normalized * moveSpeed * Time.deltaTime;


        newPos.x = Mathf.Clamp(newPos.x, xLimit.x, xLimit.y);

        newPos.z = Mathf.Clamp(newPos.z, zLimit.x, zLimit.y);

        mainCamera.transform.position = newPos;
    }
}
