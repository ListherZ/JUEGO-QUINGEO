using UnityEngine;

namespace Jsgaona {

    // Se emplea este scriptable object para almacenar el comportamiento en persecucion del enemigo
    [CreateAssetMenu(fileName = "NewBasicAttackTemple", menuName = "Juego/Template/Basic Attack Temple")]
    public class AIStateBasicAttackTemplate : AIStateTemplate {
        
        // Velocidad de ataque
        [Range(0.1f, 5.0f)] public float SpeedAttack = 1.5f;

        // Nombres de animaciones que realiza durante el ataque
        public string[] NameAnim = {"Attack01", "Attack02"};
    }
}