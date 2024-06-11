using UnityEngine;

public class JawRotation : MonoBehaviour
{
    public float rotationSpeed = 10.0f;
    public float translateSpeed = 5.0f;
    private bool isHolding = false;
    private bool isRotating = false;
    private Vector3 offset;
    private float zCoordinate;
    public float zoomSpeed = 10.0f;

    void OnMouseDown()
    {
        zCoordinate = Camera.main.WorldToScreenPoint(transform.position).z;
        offset = transform.position - GetMouseWorldPos();
        isHolding = true;
    }

    void OnMouseDrag()
    {
        if (isRotating)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            transform.Rotate(Vector3.up, mouseX * rotationSpeed);
            transform.Rotate(Vector3.right, mouseY * rotationSpeed);
        }
        else
        {
            transform.position = GetMouseWorldPos() + offset;
        }
    }

    void OnMouseUp()
    {
        isHolding = false;
        isRotating = false;
    }

    void Update()
    {
        if (Input.GetMouseButton(2) && isHolding)
        {
            isRotating = true;

        }

        float scroll = Input.mouseScrollDelta.y;
        if (scroll != 0 && isHolding)
        {
            float zoom = scroll * zoomSpeed * Time.deltaTime;
            transform.localPosition += Vector3.forward * zoom;
        }
    }

    Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoordinate;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}