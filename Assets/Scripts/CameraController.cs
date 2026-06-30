using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float distance = 8f;
    public float height = 3f;
    public float mouseWheel;
    public float rotateSpeed = 120f;
    public float angle;
    public float upRange = 1f;
    public LayerMask collisionMask = ~0;
    public float collisionBuffer = 0.3f;
    public float returnSpeed = 10f;
    private float currentDistance;
    void Start()
    {
        currentDistance = distance;
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

        Vector3 pivot = target.position + Vector3.up * height;
        Vector3 dir = Quaternion.Euler(0f, angle, 0f) * new Vector3(0f, 0f, -1f);

        float desiredDistance = distance;

        RaycastHit hit;
        if (Physics.Raycast(pivot, dir, out hit, distance, collisionMask, QueryTriggerInteraction.Ignore))
        {
            desiredDistance = hit.distance - collisionBuffer;
        }

        
        if (desiredDistance < currentDistance)
            currentDistance = desiredDistance;
        else
            currentDistance = Mathf.Lerp(currentDistance, desiredDistance, returnSpeed * Time.deltaTime);

        currentDistance = Mathf.Max(currentDistance, 0.1f);

        transform.position = pivot + dir * currentDistance;
        transform.LookAt(pivot);
    }
}
