using UnityEngine;

namespace Jsgaona
{
    [CreateAssetMenu(fileName = "NewHackTemplate", menuName = "Juego/Template/Hack Template")]
    public class AIStateHackTemplate : AIStateTemplate
    {
        [Min(0.1f)] public float TiempoHack = 3f;
        public bool StopAgent = true;
        public bool FaceTarget = true;

    }
}
