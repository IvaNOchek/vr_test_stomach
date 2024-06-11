using UnityEngine;

public class CameraControllerVR : MonoBehaviour
{
    public float speed = 10.0f;
    public float sensitivity = 100.0f;
    public Transform playerTransform;

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            rotationX += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            rotationY -= Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
            rotationY = Mathf.Clamp(rotationY, -90, 90);

            transform.localEulerAngles = new Vector3(rotationY, rotationX, 0.0f);

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = playerTransform.right * moveHorizontal + playerTransform.forward * moveVertical;
        movement.y = 0;

        playerTransform.Translate(speed * Time.deltaTime * movement, Space.World);


        }


    }
}
