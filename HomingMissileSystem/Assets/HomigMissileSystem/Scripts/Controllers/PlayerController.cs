using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controlador basico del jugador, no utiliza fisicas
public class PlayerController : MonoBehaviour
{
    //VAriable spublicas que rigen el movimiento del jugador
    public float speed = 5f;
    public float rotationSpeed = 3f;
    public Rigidbody tRb;

    //Declaracion del riggidBofy del jugador en caso de que no se haya declarado antes
    private void Start()
    {
        if(tRb == null)
        {
            tRb = GetComponent<Rigidbody>();
        }
    }

    //Verificacion constante de las teclas presionadas para mover al jugador en esas direcciones, tanto en posicion como en rotacion
    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movementDirection = new Vector3(horizontalInput, 0f, verticalInput);
        movementDirection.Normalize();
        transform.position = transform.position + movementDirection * speed * Time.deltaTime;


        if(movementDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movementDirection), rotationSpeed * Time.deltaTime);
        }
    }
}
