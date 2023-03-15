using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Administrador de los puntos de spawn de los misiles, asi como de la cantidad de ellos en escena y los intervalos para sus apariciones
public class MissileSpawn : MonoBehaviour
{
    //Variables publicas para los prefabs y administracion de tiempos
    public GameObject[] missilesPrfbs;
    private GameObject launchedMissile;

    public int missilesLaunchedReduceTime;
    public float missilesSpawningTime;
    public float targetDestroyedLifeTime;

    //Variables que guardan los nombres de los tags
    [Header("Tag Names")]
    public string targetTag;
    public string SpawnPointsTag;
    public string CameraTag;

    //Variable publica paraa que otros scripts puedan acceder al numero de misiles activos y estos mismos puedan eliminarse de dich lista al ser destruidos
    public List<GameObject> missilesCount
    {
        get => missilesBuffer;
        set => missilesBuffer = value;
    }

    //Variables privadas para el spawn de los misiles, asi como de las variables que administran si se ha eliminado al jugador o no
    private GameObject[] spawnPoints;
    private Transform spawnPoint;
    private GameObject target;
    private GameObject newMissile;
    private GameObject cameraPlayer;
    private MissileWayPoint wayPoint;
    private ReSpawnButton reSpawnButton;
    private int missilesLaunched = 0;
    private float timer;
    protected bool targetEliminated;

    //Lista interna de los misiles activos
    private List<GameObject> missilesBuffer = new List<GameObject>();

    //Declaracion de variables y lanzamiento del primer misil
    private void Start()
    {        
        spawnPoints = GameObject.FindGameObjectsWithTag(SpawnPointsTag);
        cameraPlayer = GameObject.FindGameObjectWithTag(CameraTag);
        wayPoint = FindObjectOfType<MissileWayPoint>();
        reSpawnButton = FindObjectOfType<ReSpawnButton>();
        SearchNewTarget();
        NewMissileLaunched();
    }

    //Se ejecuta un cronometro si es que el objetivo no ha sido elimindao, y cada cierto tiempo se lanza un nuevo misil y se reinicia el temporixador
    private void FixedUpdate()
    {   
        if(timer > missilesSpawningTime)
        {
            timer = 0f;
            Debug.Log("new missile");
            NewMissileLaunched();
        }

        //Si la cantidad de misiles lanzados supera el numero de misisles establecidos, se reduce el intervalo entre misil y misil
        if(missilesLaunched > missilesLaunchedReduceTime)
        {
            missilesSpawningTime -= 0.5f;
            missilesLaunched = 0;
        }

        if(target == null)
        {
            timer = 0f;
            reSpawnButton.target = null;
            reSpawnButton.gameObject.SetActive(true);
            SearchNewTarget();
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    //Funcion que busca un nuevo objetivo
    private void SearchNewTarget()
    {
        target = GameObject.FindGameObjectWithTag(targetTag);

        if (target != null)
        {
            if(cameraPlayer != null)
            {
                cameraPlayer.GetComponent<CamaraController>().SetTarget = target;
            }

            if(reSpawnButton != null)
            {
                reSpawnButton.target = target;
                reSpawnButton.gameObject.SetActive(false);
            }

            foreach(var missileActive in missilesBuffer)
            {
                if(missileActive != null)
                {
                    missileActive.GetComponent<Missile>().target = target;
                }
            }
        }
    }

    //Funcion que lanza un nuevo misil eligiendo de forma aleatoria el punto de spawn. Se le asigna al nuevo misil el objetivo y se agrega a la lista de misiles activos
    private void NewMissileLaunched()
    {
        launchedMissile = missilesPrfbs[Random.Range(0, missilesPrfbs.Length)];
        spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform;
        newMissile = Instantiate(launchedMissile, spawnPoint.position, Quaternion.identity);
        newMissile.GetComponent<Missile>().target = target;

        missilesBuffer.Add(newMissile);
        missilesLaunched++;

        //Activa el WayPoint al crear un misil nuevo si es que el WayPoint no se encontraba activo
        if(wayPoint.setActive == false)
        {
            wayPoint.setActive = true;
            wayPoint.gameObject.SetActive(true);
        }
    }
}
