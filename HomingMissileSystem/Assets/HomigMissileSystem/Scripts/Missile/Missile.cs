using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissileStatus
{
    LAUNCH,
    FIND_TARGET,
    FOLLOWING_TARGET
}

public class Missile : MonoBehaviour
{
    [Header("References")]
    public Rigidbody mRb;
    public PlayerController target;
    public GameObject explosionPrfb;
    public GameObject audioSPrfb;
    public AudioClip explosionSound;
    public float upTime;
    private MissileStatus missileStatus;

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

        missileStatus = MissileStatus.LAUNCH;
        StartCoroutine(MissileLife());
    }

    IEnumerator MissileLife()
    {
        switch (missileStatus)
        {
            case MissileStatus.LAUNCH:
                Debug.Log("Missile launched");

                Vector3 lookUp = new Vector3(0f, 90f, 0f);
                mRb.velocity = transform.up * mSpeed;
                mRb.rotation = Quaternion.LookRotation(lookUp);
                missileStatus = MissileStatus.FIND_TARGET;
                break;

            case MissileStatus.FIND_TARGET:
                yield return new WaitForSeconds(upTime);

                Debug.Log("Searching target");

                if (target == null)
                {
                    target = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
                }

                missileStatus = MissileStatus.FOLLOWING_TARGET;                
                break;

            case MissileStatus.FOLLOWING_TARGET:
                Debug.Log("Following target");

                mRb.velocity = transform.forward * mSpeed;
                var leadTimePercentage = Mathf.InverseLerp(minDistance, maxDistance, Vector3.Distance(transform.position, target.transform.position));

                PredictionMovementTarget(leadTimePercentage);
                AddDeviation(leadTimePercentage);
                Rotation();
                break;
        }
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