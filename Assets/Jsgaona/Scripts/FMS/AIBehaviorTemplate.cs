using Jsgaona;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBehaviorTemple", menuName = "Juego/Behavior/Behavior Temple")]
public class AIBehaviorTemplate : ScriptableObject
{

    [Header("Target detection")]
    // Periodos de tiempo para realizar busquedas 
    [Range(0.1f, 3.0f)] public float CheckInterval = 0.5f;

    [Header("Approach Distance")]
    // Distancia minima a la que se puede acercar al objetivo 
    public float MaxDistanceFromTarget = 5;

    // Distancia maxima a la que se puede acercar al objetivo 
    public float MinDistanceFromTarget = 3;

    [Header("Setting States")]
    // Referencia del estado por defecto 
    public AIStateIdle DefaultState;

    // Referencia de los datos del estado por defecto 
    public AIStateIdleTemplate DefaultStateTemplate;

    // Referencia del estado de persecucion 
    public AIStateChase ChaseState;

    // Referencia de los datos del estado de persecucion 
    public AIStateChaseTemplate ChaseTemplate;
 

    // Referencia del estado de ataque basico 

    // Referencia del estado de hackeo 
    public AIStateHack HackState;
    public AIStateHackTemplate HackTemplate;
    public AIStateBasicAttack AttackState;
    public AIStateBasicAttackTemplate AttackStateTemplate;


}
