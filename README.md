# Homing Missile System

Sistema de Misiles Auto - Dirigidos // Montserrat Godinez Núñez - Parcial 02

Este es un sistema de para misiles auto-dirigidos hacia un objetivo. Estos misiles se lanzan desde un objeto (”MissileBase”) con puntos de spawn que se escogen aleatoria mente para cada nuevo lanzamiento; una vez eliminado el objetivo los misiles se destruyen después de cierto tiempo.

El proyecto está listo para solo agregar los scripts y prefabs en los objetos de tu proyecto y desde el mismo editor configurarlos para brindarte una mayor comodidad sin que tengas que sufrir por saber qué código hace qué cosa.

# Índice

- [Contenido](https://www.notion.so/Contenido-a565186e10184f84915d5f638dde31ea)
- [Instalación](https://www.notion.so/Instalaci-n-9772f87c374a4e81967f89b43225a09c)
    - [Configurar una Nueva Escena o Nuevos Objetos](https://www.notion.so/Configurar-una-Nueva-Escena-o-Nuevos-Objetos-b35e8e76e66a43e9b472b177cdf4f49b)
        - [Misiles](https://www.notion.so/Misiles-7ab470a238534a69915eaafb65333db9)
        - [Lanzador de Misiles](https://www.notion.so/Lanzador-de-Misiles-8a83a233b9ca4addbc38b9b82d49b04b)
        - [UI](https://www.notion.so/UI-e5690cda8681439e8c6dba9684bec8be)
            - [Estatus del Misil](https://www.notion.so/Estatus-del-Misil-3c07bd79acf342d59d673da1cf4b5388)
            - [Icono del Misil](https://www.notion.so/Icono-del-Misil-fa19c9fe3c73454d9621e0010ac8cfe4)
            - [Botón de ReSpawn](https://www.notion.so/Bot-n-de-ReSpawn-b08fb182a8ba45c6b7f5d364b50e392f)
- [Funcionalidades de Modificar el Código](https://www.notion.so/Funcionalidades-de-Modificar-el-C-digo-e153b7cbff314eb49b33c3ad49400e2e)
    - [MissileSpawn](https://www.notion.so/MissileSpawn-72f7452667304540a11e7b28a4e1ee4c)
    - [UIText](https://www.notion.so/UIText-04317543249a44b6b5af020e9e3dad0b)
    - [ReSpawnButton](https://www.notion.so/ReSpawnButton-0153079b53984bce9e2b9ccb9a1cd689)
- [Recomendaciones](https://www.notion.so/Recomendaciones-33de5995d98d41be9becbad03cbebd32)
    - [Usa traductor para los errores si no sabes inglés](https://www.notion.so/Usa-traductor-para-los-errores-si-no-sabes-ingl-s-e16e695cca5249bda1fa92de47ef0461)
    - [No modifiques los scripts!](https://www.notion.so/No-modifiques-los-scripts-dc68269bdb73400c80fadfd0c0786ecd)
- [Contacto](https://www.notion.so/Contacto-b4935d10047c4f4d8a5fa449eb89c6f1)

---

# Contenido

- Lanzador y Administrador de Misiles (MissileBase).
- Misiles Auto-Dirigidos con Predicción del Movimiento del Objetivo.
- Misiles lanzados cada cierto intervalo de tiempo (Se puede reducir con el paso del tiempo).
- Estatus del Primer Misil Lanzado que sigue Activo (Velocidad, Aceleración, Ángulo, Posición, Fuerza, Dirección, Index del Misil).
- Icono que muestra en pantalla la posición del misil al que se le calculan su estatus.
- Un jugador con un controlador básico para pruebas.
- Cámara que sigue al jugador y que permite rotarla en de forma vertical y horizontal alrededor del jugador.
- Botón para crear un nuevo jugador cuando el que esta en escena sea destruido.

[Índice](https://www.notion.so/ndice-8993490a2e864250b0c16bf0c8b89fe2) 

---

# Instalación

- Descarga el paquete de Assets “HomingMissileSystemAssets”, el cual lo encontrarás en el repositorio Parcial2-ProgramacionII-130323.170323.

https://github.com/MGN-MK/Parcial2-ProgramacionII-130323.170323

<aside>
⚠️ En caso de que el acceso directo no funcione, copia y pega el siguiente link en un buscador: https://github.com/MGN-MK/Parcial2-ProgramacionII-130323.170323

</aside>

- Dentro de tu proyecto importa el paquete de Assets “HomingMissileSystemAssets”, este generará los archivos necesarios para poder utilizarlo, incluyendo los scripts organizados en carpetas, algunos prefabs de prueba, y unos efectos de partículas de prueba.
    - Si no tienes un proyecto de unity, solo haz doble click sobre el archivo del paquete
    - Puedes simplemente arrastrar el archivo del paquete dentro de tu proyecto
    - Puedes hacer click derecho en la carpeta del proyecto y seleccionar (Import Package > Custom Package), para después elegir el archivo del paquete.
- Puedes utilizar y personalizar la escena “HomingMissileSystem” que viene precargada, o crea una nueva escena donde se dará lugar al el lanzamiento de los misiles.

[Índice](https://www.notion.so/ndice-8993490a2e864250b0c16bf0c8b89fe2) 

# Configurar una Nueva Escena o Nuevos Objetos

Si estás utilizando la escena que viene por defecto o los prefabs de prueba puedes saltarte estos pasos.

### Misiles

1. Con el prefab de prueba.
    1. Agrega el prefab “Missile” (Prefabs > Missile).
2. Configurando uno propio.
    1. Al objeto que será tu misil agregale el script “MissileSpawn” (Scripts > Missile > MissileSpawn), y rellena los campos con sus respectivos componentes. 
    - References (Referencias)
        - Explosion Prfb. Asigna el prefab de la explosiñon que se va a crear en la misma posición que el misil.
        - Audio S Prfb. Asigna el prefab del audioSource que se reproducirá cuando el misil se destruya.
        - Explosion Sound. Asigna el clip de sonido que sonará cuando el misil se destruya.
        - Sound Duration. Escribe el número de segundos que durará el sonido de explosión.
    - Movement (Movimiento)
        - Up Time. Escribe el número de segundos que el misil volará hacia arriba antes de perseguir al objetivo.
        - Life Time. Escribe el número de segundos que el misil estará activo antes de destruirse.
        - M Speed. Escribe la velocidad de desplazamiento que tendrá el misil.
        - M Rotation Speed. Escribe la velocidad de rotación que tendrá el misil.
        - Acceleration Percentage. Desliza el deslizador para definir el porcentaje en que se incrementarán las velocidades del misil (de desplazamiento y de rotación).
        - Acceleration Delay. Escribe cuánto se va a reducir en porcentaje la aceleración, mayor el número, menor aceleración. (Esto para contrarrestar que se incrementan las velocidades cada frame).
    - Prediction (Predicción)
        - Max Distance. Escribe la distancia máxima que el misil puede registrar entre él y el objetivo.
        - Min Distance. Escribe la distancia mínima que el misil puede registrar entre él y el objetivo.
        - Max Time. Escribe la cantidad de segundos que el misil puede predicir a futuro el movimiento del objetivo. **Procura que sea un número pequeño para que sea preciso y no se vaya a lugares muy apartados sin razón.**
        - Recalculate Time. Escribe el número de segundos que deben pasar mientras el misil persigue al jugador antes de volver a subir para recalcular la trayectoria.
        - Time Up Recalculate. Escribe el número de segundos que el misil volará hacia arriba antes de volver a perseguir al objetivo.
    - Deviation (Desviación)
        - Deviation Amount. Escribe la cantidad de desviación que tendrá el misil en su trayectoria para facilitarle el dar giros bruscos si el objetivo cambia de dirección repentinamente.
        - Deviation Speed. Escribe la velocidad con la que esta desviación se manifestará.
3. Vuelve el misil un Prefab, arrastrando el objeto a la ventana de Project.

[Índice](https://www.notion.so/ndice-8993490a2e864250b0c16bf0c8b89fe2) 

### Lanzador de Misiles

1. Con el prefab de prueba.
    1. Agrega el prefab “MissileBase” (Prefabs > MissileBase).
2. Configurando uno propio.
    1. Al objeto que será tu lanzador de misiles crea como hijos objetos vacíos que serán los *SpawnPoints* (puntos de aparición).
    2. Coloca los puntos de aparición en la posición que desees, y ponles un tag para ellos únicamente.
    3. Al objeto padre agregale el script “MissileSpawn” (Scripts > Missile > MissileSpawn), y rellena los campos con sus respectivos componentes.
    - Missile Prfb. Asigna el prefab del misil que se va a crear en la misma posición del punto de aparición elegido al azar.
    - Missiles Launched Reduce Time. Escribe el número de misiles que se tienen que lanzar antes de reducir el intervalo del lanzamiento entre cada uno.
    - Missiles Spawning Time. Escribe los segundos de intervalo entre el lanzamiento de cada misil.
    - Target Destroyed Life Time. Escribe el número de segundos que tienen que pasar después de que el objetivo sea eliminado para que los misiles se destruyan.
    
    **AL ESCRIBIR LOS TAGS CUIDA LA ORTOGRAFÍA, ESPACIOS Y MAYÚSCULAS**
    
    - Target Tag. Escribe el nombre del tag del objetivo.
    - Spawn Points Tag. Escribe el nombre del tag de los puntos de aparición.
    - Camera Tag. Escribe el nombre del tag de la cámara.

[Índice](https://www.notion.so/ndice-8993490a2e864250b0c16bf0c8b89fe2) 

## UI

Necesitas tener un canvas en Escena, utiliza el que tengas o crea uno nuevo.

### Estatus del Misil

1. Con los prefabs de prueba.
    1. Agrega al canvas el prefab “MissileStats” (Prefabs > MissileStats).
2. Configurando uno propio.
    1. Al objeto padre que contiene lo que muestran tus estadísticas agrégale el script “UIText” (Scripts > UI > UIText), y rellena los campos con sus respectivos componentes.
        - Speed Text. Toma el objeto TextMeshPro que corresponda a la velocidad y arrástralo para agregarlo. Se muestra en m/s.
        - Acceleration Text. Toma el objeto TextMeshPro que corresponda a la aceleración y arrástralo para agregarlo. Se muestra en m/s².
        - Angle Text. Toma el objeto TextMeshPro que corresponda al ángulo y arrástralo para agregarlo. Se muestra en x, y, z.
        - Position Text. Toma el objeto TextMeshPro que corresponda a la posición y arrástralo para agregarlo. Se muestra en x, y, z.
        - Direction Text. Toma el objeto TextMeshPro que corresponda a la dirección y arrástralo para agregarlo. Se muestra en x, y, z.
        - Missiles Text. Toma el objeto TextMeshPro que corresponda al número de misiles y arrástralo para agregarlo. Se muestra en números.
        - Index Text. Toma el objeto TextMeshPro que corresponda al número del misil del que se esta tomando registro y arrástralo para agregarlo. Se muestra en números.

[Índice](https://www.notion.so/ndice-8993490a2e864250b0c16bf0c8b89fe2) 

### Icono del Misil

1. Con los prefabs de prueba.
    1. Agrega al canvas el prefab “MissileIcon” (Prefabs > MissileIcon).
2. Configurando uno propio.
    1. Al objeto padre que contiene el icono y el texto TextMeshProm para la distancia agrégale el script “MissileWayPoint” (Scripts > UI > MissileWayPoint), y rellena los campos con sus respectivos componentes.
        - Scale. Para que la posición del icono con respecto al misil sea correcta (Ya que el canvas es muy grande en comparación a lo que recorre el misil al principio).
        - Offset. Espacio de margen que mantendrá el icono con los bordes del canvas.

[Índice](https://www.notion.so/ndice-8993490a2e864250b0c16bf0c8b89fe2) 

### Botón de ReSpawn

1. Con los prefabs de prueba.
    1. Agrega al canvas el prefab “ReSpawn” (Prefabs > ReSpawn).
2. Configurando uno propio.
    1. Al objeto padre que contiene el botón agrégale el script “ReSpawnButton” (Scripts > UI > ReSpawnButton), y rellena el campo con su respectivo componente.
        - Object To Re Spawn. Asigna el prefab del objeto que volverá a aparecer en el origen.

[Índice](https://www.notion.so/ndice-8993490a2e864250b0c16bf0c8b89fe2) 

---

# Funcionalidades de Modificar el Código

## MissileSpawn

Dentro del script “MissileSpawn” (Scripts > Missile > MissileSpawn), tal vez requieras cambiar el nombre del componente que se busca para administrar la cámara, en el siguiente bloque de código precisamente:

```csharp
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
```

Para ello, reemplaza la variable “CamaraController” por el nombre del script que controla tu cámara. También reemplaza la palabra “SetTarget” que viene justo después de lo anterior por el nombre de la variable que representa el objeto que persigue tu cámara.

```csharp
if(cameraPlayer != null)
            {
                cameraPlayer.GetComponent<CamaraController>().SetTarget = target;
            }
```

[Índice](https://www.notion.so/ndice-8993490a2e864250b0c16bf0c8b89fe2) 

## UIText

Dentro del script “UIText” (Scripts > UI > UIText), puedes acceder a las variables que almacenan el estatus del misil para utilizarlos en tus propios cálculos y representaciones, en el siguiente bloque de código precisamente:

```csharp
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
```

En caso de que quieras utilizar su contenido puedes reemplazar private por public, o generar una nueva variable antes de la funcion Start() para acceder a ellas desde otro script, como:

```csharp
public bool setActive //Cuida que el tipo de dato coincida con el de la variable a la que buscas acceder
    {
        get => isActive; //Esto es para solo leer los datos
        set => isActive = value; //Esto es para sobreescribir los datos
    }
```

[Índice](https://www.notion.so/ndice-8993490a2e864250b0c16bf0c8b89fe2) 

## ReSpawnButton

Puedes establecer el punto donde el objeto aparezca al darle click al botón dentro de esta función:

```csharp
public void ReSpawnObject()
    {
        Debug.Log("New target spawned");
        Instantiate(objectToReSpawn, Vector3.zero, Quaternion.identity);        
    }
```

Para ello reemplaza “Vector3.zero” por tu variable de tipo Vector3 donde almacenaste la posición deseada.

```csharp
public void ReSpawnObject()
    {
        Debug.Log("New target spawned");
        Instantiate(objectToReSpawn, posicionDeseada, Quaternion.identity);        
    }
```

[Índice](https://www.notion.so/ndice-8993490a2e864250b0c16bf0c8b89fe2) 

---

# Recomendaciones

## Usa traductor para los errores si no sabes inglés

Los códigos y mensajes de error se encuentran en inglés debido a que es el lenguaje universal para estos. Sin embargo, el contenido es fácil de entender incluso traduciéndolo con algún traductor en línea, úsalo si así lo requieres.

## No modifiques los scripts!

Esto debido a que estos están relacionados entre sí. El sistema de  misiles auto-dirigidos  está diseñado para solo agregar los scripts y prefabs en los objetos de tu proyecto y desde el mismo editor configurarlos para brindarte una mayor comodidad.

 Solo modifícalos para integrar el sistema a los sistemas que ya tienes en el proyecto (Sistemas de progresión, de movimiento, de inventario, etc.), o hacer las modificaciones sugeridas en [Funcionalidades de Modificar el Código](https://www.notion.so/Funcionalidades-de-Modificar-el-C-digo-e153b7cbff314eb49b33c3ad49400e2e) .

[Índice](https://www.notion.so/ndice-8993490a2e864250b0c16bf0c8b89fe2) 

# Contacto

Dudas, comentarios y sugerencias las puedes enviar al siguiente correo con el asunto “GitHub HomingMissileSystem”:

MontseG700@gmail.com

[Índice](https://www.notion.so/ndice-8993490a2e864250b0c16bf0c8b89fe2)