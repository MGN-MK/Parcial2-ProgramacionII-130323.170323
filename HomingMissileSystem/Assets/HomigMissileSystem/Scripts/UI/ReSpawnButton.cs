using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Adminstrador del boton para crear un nuevo target al presionar el boton
public class ReSpawnButton : MonoBehaviour
{
    //Prefab del target
    public GameObject objectToReSpawn;

    //Almacenamiento del ultimo target en escena
    public GameObject target
    {
        set => currentTarget = value;
    }

    private GameObject currentTarget;

    //Funcion que se ejecuta al hacer click en el boton, genera un nuevo target en la misma posicion del ultimo target en escena
    public void ReSpawnObject()
    {
        Debug.Log("New target spawned");
        Instantiate(objectToReSpawn, Vector3.zero, Quaternion.identity);        
    }
}
