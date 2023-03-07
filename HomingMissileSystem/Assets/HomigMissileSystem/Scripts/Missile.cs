using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [Header("References")]
    public Rigidbody mRb;
    public PlayerController target;
    public GameObject explosionPrfb;
    public GameObject audioSPrfb;
    public AudioClip explosionSound;
    public float upTime;

    private bool isLaunching = false;
    private bool searchingTarget = false;
    private bool isFollowing = false;

    [Header("Movement")]
    public float mSpeed;
    public float mRotateSpeed;

    [Header("Prediction")]
    public float maxDistance;
    public float minDistance;
    public float maxTime;
    private Vector3 standardPredict, deviatedPredict;

    [Header("Deviation")]
    public float deviationAmount;
    public float deviationSpeed;

    private void Start()
    {
        if(mRb == null)
        {
            mRb = GetComponent<Rigidbody>();
        }       
        
        StartCoroutine(MissileLife());
    }

    private void FixedUpdate()
    {
        if (isLaunching)
        { 
            Vector3 lookUp = new Vector3(0f, 90f, 0f);
            mRb.rotation = Quaternion.LookRotation(lookUp);
            mRb.velocity = transform.forward * mSpeed;            
        }
        else if (searchingTarget)
        {
            if (target == null)
            {
                target = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            }
        }
        else if (isFollowing)
        {
            mRb.velocity = transform.forward * mSpeed;
            var leadTimePercentage = Mathf.InverseLerp(minDistance, maxDistance, Vector3.Distance(transform.position, target.transform.position));

            PredictionMovementTarget(leadTimePercentage);
            AddDeviation(leadTimePercentage);
            Rotation();
        }
    }

    IEnumerator MissileLife()
    {
        Debug.Log("Missile launched");
        isLaunching = true;
        yield return new WaitForSeconds(upTime);
        Debug.Log("Searching target");
        isLaunching = false;
        searchingTarget = true;
        yield return new WaitForSeconds(0.1f);
        Debug.Log("Following target");
        searchingTarget = false;
        isFollowing = true;
    }

    private void PredictionMovementTarget(float leadTP)
    {
        var predictionTime = Mathf.Lerp(0, maxTime, leadTP);
        standardPredict = target.tRb.position + target.tRb.velocity * predictionTime;
    }

    private void AddDeviation(float leadTP)
    {
        var deviation = new Vector3(Mathf.Cos(Time.time * deviationSpeed), 0, 0);
        var predictionOffset = transform.TransformDirection(deviation) * deviationAmount * leadTP;
        deviatedPredict = standardPredict + predictionOffset;
    }

    private void Rotation()
    {
        var heading = deviatedPredict - transform.position;
        var rotation = Quaternion.LookRotation(heading);
        mRb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, mRotateSpeed * Time.deltaTime));
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Touched target");

        audioSPrfb.GetComponent<AudioSource>().clip = explosionSound;
        Instantiate(explosionPrfb, transform.position, Quaternion.identity);
        Instantiate(audioSPrfb, transform.position, Quaternion.identity);

        if(collision.gameObject.tag == "Player")
        {
            Destroy(collision.gameObject);
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        if(target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, target.gameObject.transform.position);
        }        
    }
}