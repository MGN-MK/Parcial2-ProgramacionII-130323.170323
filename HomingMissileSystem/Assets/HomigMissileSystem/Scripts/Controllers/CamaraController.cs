using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controlador basico de la camara para que esta siga al jugaddor y con el click del mouse esta rote a su alrededor
public class CamaraController : MonoBehaviour
{
    //Variables para la velocidad de rotacion y la suavidad del seguimiento
    public float rotationSpeed = 5f;
    [Range(0f, 1f)]
    public float smoothness = 0.1f;

    public GameObject SetTarget
    {
        set => target = value;
    }
    //Variables para conrolar los momentos en que se puede rotar la camara, el objetivo y posiciones
    private bool rotationActive;
    private Vector3 targetLastPosition;
    private Vector3 offset;
    private Vector3 newPos;
    private GameObject target;

    //Declaracion del objetivo y de la distacia entre este y la camara
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        offset = transform.position - target.transform.position;
    }

    private void FixedUpdate()
    {
        //Si la rotacion se activa, la camara se mueve conforme a los movimientos del mouse con el click sostenido en los ejes X y Y
        if (rotationActive)
        {
            Quaternion camRotationX = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotationSpeed, Vector3.up);
            Quaternion camRotationY = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * rotationSpeed, Vector3.right);
            offset = camRotationX * camRotationY * offset;
            transform.LookAt(targetLastPosition);
        }

        //Si el objetivo no esta vacio, se actualiza su ultima posicion por la actual
        if(target != null)
        {
            targetLastPosition = target.transform.position;  
        }

        //La posicion de la camara se actualiza a la ultima posicion del objetivo sumando la distancia que mantienen
        newPos = targetLastPosition + offset;

        //Se hace este cambio de posicion de forma paulatina siguiendo la posicion actual de la camara, al nueva posicion, y la suavidad del movimiento
        transform.position = Vector3.Slerp(transform.position, newPos, smoothness);

        //En caso de que se haga click izquierdo, la rotacion se activa, en caso contrario, la roytacion se desactiva y la camara mira hacia la ultima posicion del objetivo
        if (Input.GetButton("Fire1"))
        {
            rotationActive = true;
        }
        else
        {
            rotationActive = false;
            transform.LookAt(targetLastPosition);
        }      
    }
}
