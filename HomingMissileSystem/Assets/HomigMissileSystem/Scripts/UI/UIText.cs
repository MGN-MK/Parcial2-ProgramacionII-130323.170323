using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Administrador de los textos de estatus de los misiles, asi como del contador de los misiles activos
public class UIText : MonoBehaviour
{
    //Textos de cada tipo de dato
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI accelerationText;
    public TextMeshProUGUI angleText;
    public TextMeshProUGUI positionText;
    public TextMeshProUGUI powerText;
    public TextMeshProUGUI directionText;
    public TextMeshProUGUI missilesText;
    public TextMeshProUGUI indexText;
    public TextMeshProUGUI indexTarget;
    public TextMeshProUGUI positionTarget;

    //Variables para los calculos de cada tipo de dato, accesar al spawner de los misiles y a ellos mismos.
    private Rigidbody missile;
    private float speed;
    private float lastSpeed = 0f;
    private float acceleration;
    private float angleX;
    private float angleY;
    private float angleZ;
    private Vector3 position;
    private float power;
    private Vector3 direction;
    private bool active = true;
    private MissileSpawn missileSpawn;
    private int missilesActive;
    private string targetName;
    private Vector3 targetPos;

    //Listado de todos los misiles activos
    private Missile[] missilesNearby;

    //Declaracion del administrador de los misiles
    private void Start()
    {
        missileSpawn = FindObjectOfType<MissileSpawn>();
    }

    private void FixedUpdate()
    {
        //Al presionar la tecla F, se acrivan o desactivan los textos
        if (Input.GetKeyDown(KeyCode.F))
        {
            active = !active;
            Debug.Log(active);

            speedText.gameObject.SetActive(active);
            accelerationText.gameObject.SetActive(active);
            angleText.gameObject.SetActive(active);
            positionText.gameObject.SetActive(active);
            powerText.gameObject.SetActive(active);
            directionText.gameObject.SetActive(active);
            indexText.gameObject.SetActive(active);
            indexTarget.gameObject.SetActive(active);
            positionTarget.gameObject.SetActive(active);
        }

        //Si estan los textos activados y el objeto misil no esta vacio. se calcula el estatus de dicho misil
        if (active && missile != null)
        {
            CalculateStats();
        }

        //Sin importar los demas textos, el contador de misiles activos se muestra y acutaliza siempre
        missilesActive = missileSpawn.missilesCount.Count;
        missilesText.text = "Misiles: " + missilesActive;

        //Si el objeto de misil esta vacio, se llama a la funcion que revisa y busca un nuevo misil
        if (missile == null)
        {
            GetNearbyMissile();
        }
    }

    //Funcion que calcula el estatus del misil
    private void CalculateStats()
    {
        //Se obtiene la velocidad del riggidbody del misil
        speed = missile.velocity.magnitude;
        speedText.text = "Velocidad: " + Mathf.RoundToInt(speed) + "m/s";

        //Se llama al numerador que calcula la aceleracion (es un numerador porque se calcula despues de cierto tiempo para obtener los datos correctos)
        StartCoroutine(AcelerationCalculation());

        //Se obtienen los angulos de rotacion del riggidbody del misil
        angleX = missile.rotation.x;
        angleY = missile.rotation.y;
        angleZ = missile.rotation.z;
        angleText.text = "Ángulo: " + angleX.ToString("F2") + "°, " + angleY.ToString("F2") + "°, " + angleZ.ToString("F2") + "°";
        
        //Se obtiene la posicion del riggidbody del misil
        position = missile.position;
        positionText.text = "Posición: " + position.ToString("F2");
        
        //Se calcula la fuerza con la formula Fuerza = masa * aceleracion
        power = missile.mass * acceleration;
        powerText.text = "Fuerza: " + Mathf.RoundToInt(power) + "N";
        
        //Se obtiene la direccion del script del misil, que la calcula previamente
        direction = missile.gameObject.GetComponent<Missile>().looking;
        directionText.text = "Dirección: " + direction.ToString("F2");

        if (missile.gameObject.GetComponent<Missile>().target != null)
        {
            //Obtiene el nombre del target
            targetName = missile.gameObject.GetComponent<Missile>().target.GetComponent<PlayerController>().nameID;
            indexTarget.text = "Objetivo Actual: " + targetName;

            //Obtiene la posicion del target
            targetPos = missile.gameObject.GetComponent<Missile>().positionOfTarget;
            positionTarget.text = "Posición del Objetivo: " + targetPos;
        }
    }

    //Calcula la aceleracion guardando dos velocidades con una diferencia de 1 segundo, para ejecutar la formula Aceleracion = velocidad final - velocidad inicial / tiempo (este ultimo se omite al ser de 1 segundo)
    IEnumerator AcelerationCalculation()
    {
        lastSpeed = missile.velocity.magnitude;
        yield return new WaitForSeconds(1f);
        
        if(missile != null)
        {
            acceleration = missile.velocity.magnitude - lastSpeed;
        }

        accelerationText.text = "Aceleración: " + acceleration.ToString("F3") + "m/s²";
    }

    //Registra todos los objetos que tengan el script de Missile, verifica que no sean nulos, obtienen su componente riggidbody y su numero index para mostratlo juntos con su estatus
    private void GetNearbyMissile()
    {
        missilesNearby = FindObjectsOfType<Missile>();

        foreach (var thisMissile in missilesNearby)
        {
            if (thisMissile != null && missile == null)
            {
                missile = thisMissile.GetComponent<Rigidbody>();
                indexText.text = "Misil Actual: " + thisMissile.missileIndex + " tipo " + thisMissile.followingType;
            }
        }
    }
}
