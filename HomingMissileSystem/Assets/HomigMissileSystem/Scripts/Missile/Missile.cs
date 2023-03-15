using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FollowingType
{
    CONSTANT, ONEPOSITION
}

//Controlador de los movimientos del misil, prediccion y destruccion
public class Missile : MonoBehaviour
{
    //Establece cómo el misil va a seguir al objetivo
    public FollowingType followingType;

    //Referencias a objetos publicos
    [Header("References")]
    public GameObject explosionPrfb;
    public GameObject audioSPrfb;
    public AudioClip explosionSound;
    public float soundDuration;

    //Variables protegidas a las que pueden accesar otros scripts
    public GameObject target
    {
        get => playerGameObject;
        set => playerGameObject = value;
    }

    public Vector3 positionOfTarget
    {
        get => targetPosition;
    }

    public int missileIndex
    {
        get => index + 1;
    }

    public Vector3 looking
    {
        get => heading;
    }

    //Variabeles privadas para las operacione sinternas del propio script
    private MissileSpawn missileSpawn;
    private Rigidbody mRb;
    private GameObject playerGameObject;
    private Vector3 heading;
    private int index;    

    //Variable spublicas que rigen los movimientos del misil
    [Header("Movement")]
    public float upTime;
    public float lifeTime;
    public float mSpeed;
    public float mRotateSpeed;

    private Vector3 targetPosition;
    private bool settedTargetPosition = false;

    //Variables para la aceleracion constante del misil asi como el retraso que tiene para evitar que suba exponencialmente
    [Range(0f, 1f)]
    public float accelerationPercentage;
    public float accelerationDelay = 250f;

    //Vector y booleanos para administrar las fases de la vida del misil
    private Vector3 lookUp = new Vector3(0f, 90f, 0f);
    private bool isLaunching = false;
    private bool isFollowing = false;
    private bool recalculate = false;
    
    //Variables que rigen la prediccion de movimiento
    [Header("Prediction")]
    public float maxDistance;
    public float minDistance;
    public float maxTime;
    public float recalculateTime;
    public float timeUpRecalculate;

    private Vector3 standardPredict, deviatedPredict;
    private float timer = 0f;

    //Variables que ayudan al misil a tener uan desviacion para girar mas facilmente
    [Header("Deviation")]
    public float deviationAmount;
    public float deviationSpeed;

    //Se declaran variables y comienza la vida inicial del misil
    private void Start()
    {
        if (mRb == null)
        {
            mRb = GetComponent<Rigidbody>();
        }

        missileSpawn = FindObjectOfType<MissileSpawn>();
        index = missileSpawn.missilesCount.Count;
        StartCoroutine(MissileLife());
    }

    //Se verifica en que fase se encuentra la vida del misil, asi como el contador del tiempo de vida del misil
    private void FixedUpdate()
    {
        mRb.velocity = transform.forward * mSpeed;
        timer += Time.deltaTime;

        if (isLaunching)
        {
            mRb.MoveRotation(Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lookUp), mRotateSpeed * Time.deltaTime));
        }
        else if (isFollowing && playerGameObject != null)
        {
            mSpeed += mSpeed * accelerationPercentage / accelerationDelay / 2;
            mRotateSpeed += mRotateSpeed * accelerationPercentage / accelerationDelay;

            if(followingType == FollowingType.CONSTANT)
            {
                targetPosition = playerGameObject.transform.position;

                if (recalculate)
                {
                    mRb.MoveRotation(Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lookUp), mRotateSpeed * Time.deltaTime));
                }
                else
                {
                    var leadTimePercentage = Mathf.InverseLerp(minDistance, maxDistance, Vector3.Distance(transform.position, playerGameObject.transform.position));

                    PredictionMovementTarget(leadTimePercentage);
                    AddDeviation(leadTimePercentage);
                    Rotation();
                }
            }
            else if(followingType == FollowingType.ONEPOSITION)
            {
                if (settedTargetPosition)
                {
                    heading = targetPosition - transform.position;
                    var rotation = Quaternion.LookRotation(heading);
                    mRb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, mRotateSpeed * Time.deltaTime));
                }
                else
                {
                    Debug.Log("Set target position");
                    targetPosition = playerGameObject.transform.position;
                    settedTargetPosition = true;
                }
            }
            
        }

        //Si se termina su tiempo de visa, el misil de destruye
        if( timer > lifeTime)
        {
            SelfDestroy();
        }

        //Si se queda sin objetivo, empieza la secuencia para destruirse en tal caso
        if(playerGameObject == null)
        {
            StartCoroutine(TargetDeadTime());
        }
    }

    //Numerado que administra las fases de vida del misil con base a los tiempos dados en las variables publicas
    IEnumerator MissileLife()
    {
        Debug.Log("Missile launched");
        isLaunching = true;
        yield return new WaitForSeconds(upTime);
        isLaunching = false;
        Debug.Log("Following target");
        isFollowing = true;
        while (isFollowing)
        {
            yield return new WaitForSeconds(recalculateTime);
            recalculate = true;
            Debug.Log("Recalculating...");
            yield return new WaitForSeconds(timeUpRecalculate);
            recalculate = false;
            Debug.Log("Following target again");
        }
    }

    //Numerador que administra las acciones para autodestruccion del misil cuando este se queda sin obejtivo
    IEnumerator TargetDeadTime()
    {
        yield return new WaitForSeconds(missileSpawn.targetDestroyedLifeTime + index);
        SelfDestroy();
    }

    //Funcion que predice el movimiento del objetivo segun su posicion, velocidad y el tiempo que puede predecir a futuro
    private void PredictionMovementTarget(float leadTP)
    {
        var predictionTime = Mathf.Lerp(0, maxTime, leadTP);
        standardPredict = playerGameObject.GetComponent<Rigidbody>().position + playerGameObject.GetComponent<Rigidbody>().velocity * predictionTime;
    }

    //Funcion que agrega la desviacion del misil para que le sea mas facil girar ante cambios reprentinos de la direccion del objetivo
    private void AddDeviation(float leadTP)
    {
        var deviation = new Vector3(Mathf.Cos(Time.time * deviationSpeed), 0, 0);
        var predictionOffset = transform.TransformDirection(deviation) * deviationAmount * leadTP;
        deviatedPredict = standardPredict + predictionOffset;
    }

    //Funcion que se encarga de rotar al misil en direccion al objetivo
    private void Rotation()
    {
        heading = deviatedPredict - transform.position;
        var rotation = Quaternion.LookRotation(heading);
        mRb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, mRotateSpeed * Time.deltaTime));
    }

    //Funcion que, al chocar con un objeto solido, se auto destruye y verifica si este es el jugador para destruirlo tambien
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Touched target");
            Destroy(collision.gameObject);
        }

        SelfDestroy();
    }

    //Funcion que dibuja una linea roja en la escena conectando al misil con el objetivo para que el desarrollador pueda ver claramente hacia quien se esta dirigiendo
    private void OnDrawGizmos()
    {
        if(playerGameObject != null && isFollowing)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, playerGameObject.gameObject.transform.position);
        }        
    }

    //Funcion de la secuencia de auto destruccion, que coloca las animaciones y sonidos, para despues destruirlos y al objeto mismo, eliminandose tambien de la lista de misiles activos del script MissileSpawn
    public void SelfDestroy()
    {        
        var explosion = Instantiate(explosionPrfb, transform.position, Quaternion.identity);
        var sound = Instantiate(audioSPrfb, transform.position, Quaternion.identity);
        sound.GetComponent<AudioSource>().clip = explosionSound;
        var exAnim = explosion.GetComponent<ParticleSystem>().main.duration;

        gameObject.SetActive(false);
        gameObject.GetComponent<AudioSource>().Stop();
        missileSpawn.missilesCount.Remove(gameObject);

        Destroy(explosion, exAnim);
        Destroy(sound, soundDuration);
        Destroy(gameObject);
    }
}