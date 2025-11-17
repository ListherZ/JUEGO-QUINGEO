using UnityEngine;

namespace Jsgaona {

    [CreateAssetMenu(fileName = "NewChaseTemple", menuName = "Juego/Template/Chase Template")]
    public class AIStateChaseTemplate : AIStateTemplate {
        
        public float LeaveDistance = 10.0f;
        [Range(1, 3)] public float MoveSpeedModifier = 1.5f;
    }
}