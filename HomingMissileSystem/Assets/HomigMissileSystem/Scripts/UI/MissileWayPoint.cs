using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Script encargado del movimiento del marcador de la posicion de un misil con respecto a la camara del objetivo, asi como el calculo e impresion en pantalla de la distancia entre dicho misil y el objetivo
public class MissileWayPoint : MonoBehaviour
{
    //Variables publicas para que la posicion del icono con respecto al misil sea correcta (Ya que el canvas es muy grande en comparacion a lo que recorre el misil al principio), y el espacio de margen que mantendra el icono con los bordes del canvas
    public float scale = 100;
    public float offset = 10;

    public bool setActive
    {
        get => isActive;
        set => isActive = value;
    }

    //Variables privadas que son parte del icono y el texto de la distancia, asi como la lista de misiles y si el objeto se encuentra activo o no
    private Image icon;
    private Transform missile;
    private Transform missileTarget;
    private TextMeshProUGUI meter;
    private Missile[] missilesActive;
    private bool isActive = true;

    //Declaracion de las variables que conforman al medidor
    private void Start()
    {
        icon = GetComponent<Image>();
        meter = GetComponentInChildren<TextMeshProUGUI>();
    }

    //Comprobacion constante de las medidas de la ventana, posicion de la camara con respecto al mundo y establecimiento de uno de los misiles en cuestion
    private void FixedUpdate()
    {
        float minX = (icon.GetPixelAdjustedRect().width / 2) + offset;
        float maxX = Screen.width - minX;
        float minY = (icon.GetPixelAdjustedRect().height / 2) + offset;
        float maxY = Screen.height - minY;

        Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);
        GetNearbyMissile();

        //En caso de tener un misil, se activa el objeto y, con base al objetivo, se calcula la posicion del icono en la pantalla y la distancia expresada en metros
        if (missile != null)
        {          
            if(missile.gameObject.GetComponent<Missile>().target != null)
            {
                missileTarget = missile.gameObject.GetComponent<Missile>().target.transform;
                Vector3 direction = missile.position * scale - missileTarget.position * scale;

                if (Vector3.Dot(direction, missileTarget.forward) < 0)
                {
                    if (pos.x < Screen.width / 2 && (pos.y > minY || pos.y < maxY))
                    {
                        pos.x = maxX;
                    }
                    else
                    {
                        pos.x = minX;
                    }

                    if (pos.y < Screen.height / 2 && (pos.x > minX || pos.x < maxX))
                    {
                        pos.y = maxY;
                    }
                    else
                    {
                        pos.y = minY;
                    }
                }

                pos.x = Mathf.Clamp(direction.x, minX, maxX);
                pos.y = Mathf.Clamp(direction.y, minY, maxY);

                icon.transform.position = pos;
                meter.text = ((int)Vector3.Distance(missile.position, missileTarget.position)).ToString() + " m";
            }            
        } //Si no hay misil, el objeto se desactiva
        
        if(missileTarget == null && isActive)
        {
            isActive = false;
            gameObject.SetActive(isActive);
        }
    }

    //Funcion que hace un listado de todos los misiles activos y declara uno como misil objetivo
    private void GetNearbyMissile()
    {
        missilesActive = FindObjectsOfType<Missile>();

        foreach (var thisMissile in missilesActive)
        {
            if(thisMissile != null && missile == null)
            {
                missile = thisMissile.transform;
            }
        }   
    }
}
