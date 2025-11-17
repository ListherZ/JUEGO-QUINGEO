using UnityEngine;

namespace Assets.DanDanDan.Scripts
{
    public class Plataform : MonoBehaviour
    {
        //Variables visibles desde el inspector de Unity
        [SerializeField] private float speed = 2;
        [SerializeField] private float minApproach = 0.1f;
        [SerializeField] private Transform[] wayPoint;

        //Variables ocultas desde el inspector de Unity
        private int currentIndex;
        private Vector3 nextPosition;

        //Metodo de llamada de Unity, se llama una unica vez al iniciar el app se llama despues de
        //Awake, se realiza la configuracion previa al inicio de la logica del juego
        private void Start()
        {
            //Se valida los puntos de control
            if (wayPoint.Length > 2)
            {
                transform.position = wayPoint[0].position;

                //Se asigna el siguiente indice y posicioon del punto de control
                currentIndex = 1;
                nextPosition = wayPoint[currentIndex].position;
            }
            else
            {
                enabled = false;
            }
        }

        //Metodo de llamada de Unity, se llama en cada actualizacion constante 0.02 seg
        //Se realiza la logica de gestion de fisicas del motor
        private void FixedUpdate()
        {
            //float distance = Vector3.Distance(transform.position, nextPosition);
            Vector3 toTarget = (nextPosition - transform.position).normalized;

            //se valida si la plataforma ha llegado a su destino, con margen de tolerancia
            if (toTarget.magnitude < minApproach)
            {
                currentIndex++;
            }
                //Se valida s el indice ha superado al tamano del arreglo
                if (currentIndex >= wayPoint.Length)
                { 
                    currentIndex = 0;    
                }

                nextPosition = wayPoint[currentIndex].position;

            //Mover la plataforma hacia el destino actual
            float timer = speed * Time.fixedDeltaTime;
            transform.position = Vector3.MoveTowards(transform.position, nextPosition, timer);
        }

        //Metodo de llama de Unity, se llama en el momento que una consolision marcada como Trigger
        //Se superpone a otro collider
        private void OnTriggerEnter(Collider other)
        {
            //Verifica si es el jugador y moverlo con la plataforma
            if(other.CompareTag("Player"))
            {
                other.transform.SetParent(transform, true);
            }
        }

        //Metodo de llama de Unity, se llama en el momento que una consolision marcada como Trigger
        //Se superpone a otro collider
        private void OnTriggerExit(Collider other)
        {
            //Liberar al jugador al salir de la plataforma
            if(other.CompareTag("Player"))
            {
                other.transform.SetParent(null);
            }
        }
    }
}