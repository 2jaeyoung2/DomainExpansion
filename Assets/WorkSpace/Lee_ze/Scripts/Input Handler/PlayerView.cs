using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public float scrollSpeed = 15f;

    public float edgeThreshold = 5f; // 화면 가장자리로부터의 거리 (픽셀)

    public float zoomSpeed = 10f; // 줌 속도

    public float minZoom = 20f; // 최소 줌 레벨

    public float maxZoom = 60f; // 최대 줌 레벨

    public float zoomSmoothTime = 0.2f; // 줌 스무스 타임

    [SerializeField]
    private Camera cam;

    private float targetZoom;

    private float zoomVelocity = 0f;

    void Start()
    {
        targetZoom = cam.fieldOfView; // 초기 줌 설정
    }

    void Update()
    {
        Vector3 pos = transform.position;

        // 마우스 위치 확인
        Vector2 mousePos = Input.mousePosition;

        // 오른쪽 스크롤
        if (mousePos.x >= Screen.width - edgeThreshold)
        {
            pos.x += scrollSpeed * Time.deltaTime;
        }

        // 왼쪽 스크롤
        if (mousePos.x <= edgeThreshold)
        {
            pos.x -= scrollSpeed * Time.deltaTime;
        }

        // 위쪽 스크롤
        if (mousePos.y >= Screen.height - edgeThreshold)
        {
            pos.z += scrollSpeed * Time.deltaTime;
        }

        // 아래쪽 스크롤
        if (mousePos.y <= edgeThreshold)
        {
            pos.z -= scrollSpeed * Time.deltaTime;
        }

        // 카메라 위치 업데이트
        transform.position = pos;

        // 마우스 휠로 줌 인/아웃
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0.0f)
        {
            targetZoom -= scroll * zoomSpeed;

            targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        }

        // 스무스 줌 업데이트
        cam.fieldOfView = Mathf.SmoothDamp(cam.fieldOfView, targetZoom, ref zoomVelocity, zoomSmoothTime);
    }
}