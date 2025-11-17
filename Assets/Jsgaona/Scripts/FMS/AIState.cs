using UnityEngine;

namespace Jsgaona {

    public abstract class AIState : ScriptableObject {
        
        public EnemyAi EnemyAi { protected set; get; }

        // Cuando el estado se inicializa
        public abstract void Initialize(EnemyAi enemyAi, AIStateTemplate template);

        // Cuando el estado entra
        public abstract void Enter();

        // Cuando el estado se actualiza
        public abstract void Update();

        // Cuando el estado sale
        public abstract void Exit();
    }
}