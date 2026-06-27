using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float distance = 8f;
    public float height = 3f;
    public float mouseWheel;
    public float rotateSpeed = 120f;
    public float angle;
    void Start()
    {

    }
    private void Update()
    {
        mouseWheel = Input.GetAxis("Mouse ScrollWheel") * 2;
        if (mouseWheel > 0 && distance < 12)
        {
            distance += mouseWheel;
        }
        else if (mouseWheel < 0 && distance > 3)
        {
            distance += mouseWheel;
        }
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (target == null) return;

        if (Input.GetMouseButton(1))
        {
            angle += Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
        }
        Vector3 offset = Quaternion.Euler(0f, angle, 0f) * new Vector3(0f, height, -distance);
        transform.position = target.position + offset;
        transform.LookAt(target.position);
    }
}
