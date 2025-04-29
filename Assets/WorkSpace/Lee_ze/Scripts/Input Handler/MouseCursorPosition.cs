using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class MouseCursorPosition : MonoBehaviourPun
{
    public event Action OnDirectionChanged;

    private Ray ray;

    public RaycastHit hit;

    public Vector3 previousHitPoint;

    public bool isRayOn = false;

    private int floorLayerMask;

    [SerializeField]
    private GameObject cursorPointer;

    private GameObject tempCursorPointer;

    private void Start()
    {
        tempCursorPointer = Instantiate(cursorPointer);

        tempCursorPointer.SetActive(false);
        
        floorLayerMask = LayerMask.GetMask("FLOOR"); // FLOOR 레이어와만 충돌시키기 위한 레이어마스크 생성
    }

    private void Update()
    {
        GetMouseCursorPosition();
    }

    public void OnMouseRightButtonClick(InputAction.CallbackContext ctx) // '마우스 우클릭' 바인딩
    {
        if (photonView.IsMine == false)
        {
            return;
        }

        if (ctx.phase == InputActionPhase.Started)
        {
            tempCursorPointer.SetActive(true);

            isRayOn = true;
        }

        if (ctx.phase == InputActionPhase.Canceled)
        {
            tempCursorPointer.SetActive(false);

            isRayOn = false;
        }
    }

    // 마우스 커서 위치 가져오기. ※ hit.point로 커서 좌표 가져올 수 있음.
    private void GetMouseCursorPosition()
    {
        if (isRayOn == true)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100, floorLayerMask))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.green);

                tempCursorPointer.transform.position = hit.point + new Vector3(0, 0.2f, 0);

                if (previousHitPoint != hit.point)
                {
                    previousHitPoint = hit.point;

                    OnDirectionChanged?.Invoke();
                }
            }
        }
    }

    public void TempGetMouseCursorPosition()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100, floorLayerMask))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.green);
        }
    }
}
