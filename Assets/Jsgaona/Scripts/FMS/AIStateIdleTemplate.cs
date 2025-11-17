using UnityEngine;

namespace Jsgaona {
    
    [CreateAssetMenu(fileName = "NewIdleTemple", menuName = "Juego/Template/Idle Template")]
    public class AIStateIdleTemplate : AIStateTemplate {
        
        [Range(30, 120)] public float ViewAngle = 60.0f;
        public float DetectionDistance = 15.0f;
        public float AutoAggroDistance = 5.0f;
    }
}