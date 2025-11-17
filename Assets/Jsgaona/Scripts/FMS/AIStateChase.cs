using UnityEngine;

namespace Jsgaona {


    [CreateAssetMenu(fileName = "NewChaseState", menuName = "Juego/State/Chase State")]
    public class AIStateChase : AIState {
        
        private AIStateChaseTemplate chaseTemple;
    

        public override void Initialize(EnemyAi enemyAi, AIStateTemplate template) {
            EnemyAi = enemyAi;
            chaseTemple = (AIStateChaseTemplate) template;
        }

        public override void Enter() {
            Debug.Log("Entrando a Chase");
            EnemyAi.inRange = true;

            EnemyAi.UpdateSpeed(chaseTemple.MoveSpeedModifier);
        }

        public override void Update() {
         
            Debug.Log("Updating Chase State");
            if(!EnemyAi.CheckInterval()) return;
            Debug.Log("Checking Chase State");
            float distance = EnemyAi.GetDistance();
            if(distance > chaseTemple.LeaveDistance) {
                EnemyAi.ChangeState(EnemyAi.GetDefaultState());
                return;
            }
            // Cuando el enemigo alcanza al personaje
            if (!EnemyAi.Chase(distance) && EnemyAi.GetAttackState()!= null) {
                EnemyAi.ChangeState(EnemyAi.GetAttackState());
            }
            if (!EnemyAi.CanChase())
            {
                EnemyAi.ChangeState(EnemyAi.GetDefaultState() );
                return; 
            }
        }

        public override void Exit() {
            Debug.Log("Exiting Chase State");
            EnemyAi.inRange = false;

        }
    }
}