// Se emplea este scriptable object para generar el FMS del estado de reposo 
using Jsgaona;
using UnityEngine;


// Se emplea este scriptable object para generar el FMS del estado de reposo 
[CreateAssetMenu(fileName = "HackState", menuName = "Juego/State/Hack State")]
public class AIStateHack : AIState
{
    private AIStateHackTemplate hackTemplate;


    // Cuando el estado se inicializa 
    public override void Initialize(EnemyAi enemyMove, AIStateTemplate template)
    {
        EnemyAi = enemyMove;
        hackTemplate = (AIStateHackTemplate)template;
    }

    // Cuando el estado Entra 
    public override void Enter()
    {
        Debug.Log("entrando estado hackeo");
        EnemyAi.ItsHacked = true;
        // Se activa el efecto de particulas 
    }

    // Cuando el estado se Ejecuta 
    public override void Update()
    {
        // Se valida la duracion de hackeo 
        if (EnemyAi.HackingTime())
        {
            EnemyAi.ChangeState(EnemyAi.GetChaseState());
        }
    }

    // Cuando el estado se Sale 
    public override void Exit()
    {
        Debug.Log("saliendo  estado hackeo");

        // Se desactiva el efecto de particulas 
        EnemyAi.ItsHacked = false;
    }
}