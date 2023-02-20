using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraController : MonoBehaviour
{
    public float rotationSpeed = 5f;
    [Range(0f, 1f)]
    public float smoothness = 0.1f;

    private bool rotationActive;
    private bool lookAtPlayer;
    private Vector3 offset;
    private GameObject target;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        offset = transform.position - target.transform.position;
    }
    private void FixedUpdate()
    {
        if (rotationActive)
        {
            Quaternion camRotation = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotationSpeed, Vector3.up);
            offset = camRotation * offset;
        }
        Vector3 newPos = target.transform.position + offset;
        transform.position = Vector3.Slerp(transform.position, newPos, smoothness);
        if(lookAtPlayer || rotationActive)
        {
            transform.LookAt(target.transform);
        }
        if (Input.GetButton("Fire1"))
        {
            rotationActive = true;
        }
        else
        {
            rotationActive = false;
            transform.LookAt(target.transform);
        }
    }
}
