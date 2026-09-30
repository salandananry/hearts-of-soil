using UnityEngine;
using UnityEngine.EventSystems;

public class CameraPanController : MonoBehaviour
{
    [SerializeField] private BoxCollider2D cameraBounds;
    [SerializeField] private float dragSpeed = 1f;

    private Camera cam;
    private Vector3 lastWorldPosition;
    private bool isDragging;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        HandleMouseInput();
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            lastWorldPosition = GetMouseWorldPosition();
            isDragging = true;
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector3 currentWorldPosition = GetMouseWorldPosition();
            Vector3 difference = lastWorldPosition - currentWorldPosition;

            transform.position += difference * dragSpeed;

            ClampCamera();

            lastWorldPosition = GetMouseWorldPosition();
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Mathf.Abs(cam.transform.position.z);

        return cam.ScreenToWorldPoint(mousePosition);
    }

    private void ClampCamera()
    {
        Bounds bounds = cameraBounds.bounds;

        float cameraHeight = cam.orthographicSize;
        float cameraWidth = cameraHeight * cam.aspect;

        float minX = bounds.min.x + cameraWidth;
        float maxX = bounds.max.x - cameraWidth;

        float minY = bounds.min.y + cameraHeight;
        float maxY = bounds.max.y - cameraHeight;

        Vector3 position = transform.position;

        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);

        transform.position = position;
    }
}