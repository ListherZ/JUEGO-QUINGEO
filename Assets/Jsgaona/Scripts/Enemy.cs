//Este script se emplea para gestionar la logica de los enemigos
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    //Variables visibles desde el inspector de Unity
    [SerializeField] private float speedRotation = 120;
    [SerializeField] private float chaseDistance = 3.5f;
    [SerializeField] private float approachDistance = 0.5f;
    [SerializeField] private float updateInterval = 0.25f;

    [SerializeField] private LayerMask includeLayer;

    //Variables ocultas desde el Inspector de Unity
    public bool isPlayerInRange = false;
    public bool inRange = false;
    private NavMeshAgent agent;
    public Transform Player { set; get; }

    //Metodo de llamada de Unity, se llama una unica vez al iniciar el app, es el primer
    //metodo en ejecutarse, se realiza la asignacion de componentes
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    //Metodo de llamada de Unity, se llama una unica vez al iniciar el app
    //de Awake, se realiza la asignacion de variables y configuracion del script
    private void Start()
    {
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.MedQualityObstacleAvoidance;
        if (Player != null) StartCoroutine(UpdateEnemyBehaviour());
    }

    //Metodo de llamada de Unity, se llama en el momento de que el GameObject es destruido
    private void OnDestroy()
    {
        if (Player != null) StopCoroutine(UpdateEnemyBehaviour());
    }

    //Metodo de llamada de Unity, se activa cuando el renderizador del objeto entra en el campo
    // de vision de la camara activa
    private void OnBecameInvisible()
    {
        enabled = false;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
    }

    //Metodo de llamada de Unit, se llama en cada frame del computador
    //Se reaiza la logica de control del enemigo
    private void Update()
    {
        //Se valida que exista una referencia valida del personaje a seguir y este esta a rango
        if (isPlayerInRange)
        {
            //Mira al jugador suavemente
            Vector3 direction = (Player.position - transform.position).normalized;
            direction.y = 0; //para evitar rotacion vertical
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, speedRotation * Time.deltaTime);
            }

            //El enemigo esta en rango de alcance del personaje
            if (inRange)
            {
                agent.ResetPath();
            }
            else
            {
                agent.SetDestination(Player.position);
            }
        }
    }

    //Coroutine para optimizar las actualizaciones
    private IEnumerator UpdateEnemyBehaviour()
    {
        //Mientras ea verdad, se ejecuta indefinidamente
        while (true)
        {
            float distance = Vector3.Distance(transform.position, Player.position);
            bool playerDetected = distance <= chaseDistance;
            inRange = distance <= approachDistance;

            //El jugador a ingresado al radio de persecucion
            if (playerDetected)
            {
                Vector3 directionToPlayer = (Player.position - transform.position).normalized;
                isPlayerInRange = !Physics.Raycast(transform.position, directionToPlayer, distance, includeLayer);
            }

            yield return new WaitForSeconds(updateInterval);
        }
    }
}