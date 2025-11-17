using System.Collections.Generic;

using System.Text;
using Unity.VisualScripting;
using UnityEditor;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace Jsgaona {

    public class ActiveState {

        // Almacena el estado actual a ejecutar
        public AIState State;

        // Almacena los datos del estado actual a ejecutar
        public AIStateTemplate StateTemplate;
    }
  
// ...



    public class EnemyAi : MonoBehaviour, IHackeable
    {

        [Header("Adjust movement")]
        [SerializeField] private float speedMovement = 2.5f;
        [SerializeField] private float speedRotation = 120;
        [SerializeField] private LayerMask includeLayer;
        [SerializeField] private AIState currentState;
        [SerializeField] private AIBehaviorTemplate behaviorTemplate;
        [Header("Anim Controller")]
        [SerializeField] private Animator animController;
        private float nextCheck = 0.25f;
        public Transform player;
        private readonly Dictionary<string, ActiveState> ActiveStates = new();
        private Vector3 initialPoint;
        private Quaternion initialRotation;

        public bool inRange = false;
        public NavMeshAgent agent;

        // Implementación de la interfaz IHackeable
        public float TimeStopMotion { get; set; }
        public bool ItsHacked { get; set; }

        public void Hack(float timeHack)
        {
            Debug.Log("Inicio hackeo de enemigo");
            TimeStopMotion = timeHack;
            ChangeState(GetHackState());
        }

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.speed = speedMovement;
            agent.angularSpeed = speedRotation;
            agent.updateRotation = true;
        }
        public AIState GetDefaultState()
        {
            return ActiveStates[behaviorTemplate.DefaultState.name].State;
        }
   

        // Se obtiene el estado de persecucion: CHASE 
        public AIState GetChaseState()
        {
            // Se verifica si existe un estado de persecucion 
           if (behaviorTemplate.ChaseState == null) return null;
            return ActiveStates[behaviorTemplate.ChaseState.name].State;
        }
        public AIState GetAttackState()
        {
            // Se verifica si existe un estado de persecucion 
            if (behaviorTemplate.ChaseState == null) return null;
            return ActiveStates[behaviorTemplate.AttackState.name].State;
        }
        // Se obtiene el estado de combat basico: BASICATTACK 

        // Se obtiene el estado de hackeo: HACK 
        public AIState GetHackState()
        {
            // Se verifica si existe un estado de persecucion 
            if (behaviorTemplate.HackState == null) return null;
            return ActiveStates[behaviorTemplate.HackState.name].State;
        }
    
        private void Start()
        {
            // ... después de AddState(...) de todos
            PrintActiveStates("Estados registrados");
            agent.speed = speedMovement;
            initialPoint = transform.position;
            initialRotation = transform.rotation;

            // El pj dispone de un estado de reposo 
            if (behaviorTemplate.DefaultState != null)
            {
                AddState(behaviorTemplate.DefaultState, behaviorTemplate.DefaultStateTemplate);
            }
            // El pj dispone de un estado de persecucion 
            if (behaviorTemplate.ChaseState != null)
            {
                AddState(behaviorTemplate.ChaseState, behaviorTemplate.ChaseTemplate);
            }

            // El pj dispone de un estado de hack 
            if (behaviorTemplate.HackState != null)
            {
                AddState(behaviorTemplate.HackState, behaviorTemplate.HackTemplate);
            }
            if (behaviorTemplate.AttackState != null)
            {
                AddState(behaviorTemplate.AttackState, behaviorTemplate.AttackStateTemplate);
            }
            // Se recorre todos los estados y se los inicializa 
            foreach (ActiveState activeState in ActiveStates.Values)
            {
                activeState.State.Initialize(this, activeState.StateTemplate);
            }
            // Se establece el estado por defecto 
            ChangeState(GetDefaultState());
        }
        private void PrintActiveStates(string header = "Dump ActiveStates")
        {
            if (ActiveStates == null)
            {
                Debug.LogError("ActiveStates es null.", this);
                return;
            }
            if (ActiveStates.Count == 0)
            {
                Debug.LogWarning("ActiveStates está vacío.", this);
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine($"{header}  (count={ActiveStates.Count})");
            foreach (var kv in ActiveStates)
            {
                var state = kv.Value?.State;
                var template = kv.Value?.StateTemplate;
                string sName = state ? state.GetType().Name : "null";
                string tName = template ? template.GetType().Name : "null";
                string sAsset = state ? state.name : "null";
                sb.AppendLine($"- key='{kv.Key}' | stateAsset='{sAsset}' ({sName}) | template=({tName})");
            }
            Debug.Log(sb.ToString(), this);
        }
        public bool HackingTime()
        {
            TimeStopMotion -= Time.deltaTime;
            // Se valida si el contador a llegado a cero o menos 
            return TimeStopMotion < 0;
        }
        public void Update()
        {
            if (currentState != null)
            {
                currentState.Update();
            }
          
            animController.SetFloat("speed", agent.speed);
            if (!inRange) return;
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;
            if (direction != Vector3.zero) { 
            Quaternion lookRotation = Quaternion.LookRotation(direction);
                float newSpeedRot = speedRotation * Time.deltaTime;
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, newSpeedRot);
            }
        }
        public void UpdateSpeed(float newspeed)
        { if (newspeed != 0)
            {
                agent.speed = speedMovement * newspeed;
            } else
            {
                agent.speed = newspeed;
             }
            agent.stoppingDistance = behaviorTemplate.MinDistanceFromTarget;
                
        }
         

        private void AddState(AIState state, AIStateTemplate template)
        {
            ActiveState activeState;
          
            if (template == null)
            {
                activeState = new ActiveState
                {
                    State = Instantiate(state),
                };
            }
            else {
                activeState = new ActiveState
                {
                    State = Instantiate(state),
                    StateTemplate = Instantiate(template)
                };
            }
           ActiveStates.Add(state.name, activeState);

           
        }



        // Metodo que sirve para detectar la fecuencia de actualizacion de cada estado
        public bool CheckInterval()
        {
            if (Time.timeSinceLevelLoad <= nextCheck) return false;
            // Se justa el tiempo para determinar el proximo chequeo
            nextCheck = Time.timeSinceLevelLoad + behaviorTemplate.CheckInterval;
            return true;
        }


        // Se emplea este metodo para dteerminar la distancia entre en enemigo y el jugador
        public float GetDistance()
        {
            return Vector3.Distance(transform.position, player.position);
        }


        public void AdjustAgent(float speed, bool increase)
        {
            if (increase)
            {
                agent.speed *= speed;
                agent.stoppingDistance = behaviorTemplate.MinDistanceFromTarget;
            }
            else
            {
                agent.speed /= speed;
                agent.stoppingDistance = 0;
            }
        }

        // Se emplea este metodo para gestionar el estado de persecucion
        public bool Chase(float distance)
        {
         //inRange  = false;
            float min = behaviorTemplate.MinDistanceFromTarget;
            float max = behaviorTemplate.MaxDistanceFromTarget;
            Debug.Log(DetectedObstacle(distance));
            if (DetectedObstacle(distance))
            {
                agent.SetDestination(player.position);
                return true;

            }
            if (distance <= min)
            {
                //inRange = true;
                agent.ResetPath();
                return false;
            }
            if (distance > max)
            {
                agent.SetDestination(player.position);
                return true;
            }
            if (agent.hasPath) agent.ResetPath();
            return false;
        }
        public bool DetectedObstacle(float distance)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            return Physics.Raycast(transform.position, directionToPlayer, distance, includeLayer,
QueryTriggerInteraction.Ignore);
        }


        // Permite verificar si el ataque se encuentra al alcance del objetivo
        public bool ValidateView(AIStateIdleTemplate template, float distance)
        {
            // Se calcula el angulo de vision
            Vector3 directionToTarget = player.position - transform.position;
            if (distance < template.AutoAggroDistance)
            {
                return !Physics.Raycast(transform.position, directionToTarget, distance, includeLayer, QueryTriggerInteraction.Ignore);
            }
            float angleToTarget = Vector3.Angle(directionToTarget, transform.forward);
            // Si el angulo hacia el objetivo esta dentro del area de vision y el alcance
            if (angleToTarget <= template.ViewAngle && distance >= 0 && distance <= template.DetectionDistance)
            {
                // Realizar un Raycast para verificar que no exista obstaculos
                return !Physics.Raycast(transform.position, directionToTarget, distance, includeLayer, QueryTriggerInteraction.Ignore);

            }

            return false;
        }
        public bool CanChase()
        {
            return agent.pathStatus == NavMeshPathStatus.PathComplete;
        }

        // Metodo que permite cambiar el estado del enemigo
        public void ChangeState(AIState newState)
        {
            // Se valida que el estado actual no sea nulo para dejarlo
            if (currentState != null)
            {
                currentState.Exit();
            }
            // Se fija el nuevo estado
            currentState = newState;
            if (currentState != null)
            {
                currentState.Enter();
            }
        }
        public void ResetPositionEnemyAi()
        {
            agent.SetDestination(initialPoint);
        }

        // Se emplea este metodo para poder obtener si el estado de Reset se a completado 
        public bool GetResetEnemyAi()
        {
            // Espera a que se haya calculado el camino 
            if (!agent.pathPending)
            {
                // Esta dentro del rango de parada 
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    // No hay mas camino o el agente se detuvo por completo 
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        transform.rotation = initialRotation;
                        return true;
                    }
                }
            }
            return false;
        }
#if UNITY_EDITOR
        // Metodo de llamada de Unity, permite dibujar sobre la ventana de escena una esfera
        // que brinda la visualizacion del rango de area sobre el cual patrulla en enemigo aleatoriamente
        private void OnDrawGizmos()
        {
            if (behaviorTemplate == null || behaviorTemplate.DefaultStateTemplate == null) return;
            Handles.color = new Color(1f, 1f, 0f, 0.1f); // Amarillo semi-transparente
            Handles.DrawSolidArc(
            transform.position,
            transform.up,
            Quaternion.Euler(0f, -behaviorTemplate.DefaultStateTemplate.ViewAngle, 0f) * transform.forward,
            behaviorTemplate.DefaultStateTemplate.ViewAngle * 2.0f,
            behaviorTemplate.DefaultStateTemplate.DetectionDistance
            );

            Handles.color = new Color(1f, 0f, 0f, 0.1f); // Rojo semi-transparente
            Handles.DrawSolidDisc(transform.position, transform.up, behaviorTemplate.DefaultStateTemplate.AutoAggroDistance);
        }
#endif

    }
}
