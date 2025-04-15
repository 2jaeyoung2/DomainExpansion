using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public float scrollSpeed = 15f;

    public float edgeThreshold = 5f; // ȭ�� �����ڸ��κ����� �Ÿ� (�ȼ�)

    public float zoomSpeed = 10f; // �� �ӵ�

    public float minZoom = 20f; // �ּ� �� ����

    public float maxZoom = 60f; // �ִ� �� ����

    public float zoomSmoothTime = 0.2f; // �� ������ Ÿ��

    [SerializeField]
    private Camera cam;

    private float targetZoom;

    private float zoomVelocity = 0f;

    void Start()
    {
        targetZoom = cam.fieldOfView; // �ʱ� �� ����
    }

    void Update()
    {
        Vector3 pos = transform.position;

        // ���콺 ��ġ Ȯ��
        Vector2 mousePos = Input.mousePosition;

        // ������ ��ũ��
        if (mousePos.x >= Screen.width - edgeThreshold)
        {
            pos.x += scrollSpeed * Time.deltaTime;
        }

        // ���� ��ũ��
        if (mousePos.x <= edgeThreshold)
        {
            pos.x -= scrollSpeed * Time.deltaTime;
        }

        // ���� ��ũ��
        if (mousePos.y >= Screen.height - edgeThreshold)
        {
            pos.z += scrollSpeed * Time.deltaTime;
        }

        // �Ʒ��� ��ũ��
        if (mousePos.y <= edgeThreshold)
        {
            pos.z -= scrollSpeed * Time.deltaTime;
        }

        // ī�޶� ��ġ ������Ʈ
        transform.position = pos;

        // ���콺 �ٷ� �� ��/�ƿ�
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0.0f)
        {
            targetZoom -= scroll * zoomSpeed;

            targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        }

        // ������ �� ������Ʈ
        cam.fieldOfView = Mathf.SmoothDamp(cam.fieldOfView, targetZoom, ref zoomVelocity, zoomSmoothTime);
    }
}