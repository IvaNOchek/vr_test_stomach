using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class DraggableTooth : MonoBehaviour
{
    private Vector3 offset;
    private float zCoordinate;
    private bool isHolding = false;
    private bool isRotating = false;
    public float rotationSpeed = 10.0f;
    public Transform originalParent;
    public GameObject toothHolder;
    public float zoomSpeed = 10.0f;

    private InputDevice leftController;
    private InputDevice rightController;

    private void Start()
    {
        originalParent = transform.parent;
        var inputDevices = new List<InputDevice>();
        InputDevices.GetDevices(inputDevices);

        foreach (var device in inputDevices)
        {
            if (device.characteristics.HasFlag(InputDeviceCharacteristics.Left))
            {
                leftController = device;
            }
            else if (device.characteristics.HasFlag(InputDeviceCharacteristics.Right))
            {
                rightController = device;
            }
        }
    }

    private void OnMouseDown()
    {
        zCoordinate = Camera.main.WorldToScreenPoint(transform.position).z;
        offset = transform.position - GetMouseWorldPos();
        isHolding = true;
        transform.parent = toothHolder.transform;
    }

    private void OnMouseDrag()
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
            Vector3 newPosition = GetMouseWorldPos() + offset;
            transform.position = newPosition;
        }
    }

    private void OnMouseUp()
    {
        isHolding = false;
        isRotating = false;
    }

    private void Update()
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

        if (IsVR() && leftController.TryGetFeatureValue(CommonUsages.triggerButton, out bool leftTriggerValue) && leftTriggerValue)
        {
            if (!isHolding)
            {
                zCoordinate = Camera.main.WorldToScreenPoint(transform.position).z;
                offset = transform.position - GetMouseWorldPos();
                isHolding = true;
                transform.parent = toothHolder.transform;
            }
        }

        if (IsVR() && rightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 rightAxisValue) && rightAxisValue.magnitude > 0.1f)
        {
            if (isHolding)
            {
                Vector3 newPosition = GetMouseWorldPos() + offset;
                transform.position = newPosition;
            }
            else if (isRotating)
            {
                float mouseX = rightAxisValue.x;
                float mouseY = rightAxisValue.y;

                transform.Rotate(Vector3.up, mouseX * rotationSpeed);
                transform.Rotate(Vector3.right, mouseY * rotationSpeed);
            }
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoordinate;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    public void ResetPosition()
    {
        transform.position = originalParent.position;
        transform.rotation = Quaternion.Euler(-90, 0, 0);
        transform.SetParent(originalParent);
    }

    public bool IsVR()
    {
        return XRSettings.enabled;
    }
}