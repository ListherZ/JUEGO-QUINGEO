using UnityEngine;

namespace Jsgaona {
    
    
    [CreateAssetMenu(fileName = "NewIdleState", menuName = "Juego/State/Idle State")]
    public class AIStateIdle : AIState {
        
        private AIStateIdleTemplate idleTemplate;

        public override void Initialize(EnemyAi enemyAi, AIStateTemplate template) {
            EnemyAi = enemyAi;
            idleTemplate = (AIStateIdleTemplate) template;
        }



        public override void Enter() {
            EnemyAi.UpdateSpeed(0f);
            Debug.Log("Entrando a Idle");
        }


        public override void Update() {
            if(!EnemyAi.CheckInterval()) return;
            float distance = EnemyAi.GetDistance();
            if(distance <= idleTemplate.DetectionDistance) {
                if(EnemyAi.ValidateView(idleTemplate, distance)){
                    if (EnemyAi.GetChaseState() != null)
                    {
                                           EnemyAi.ChangeState(EnemyAi.GetChaseState());

                    }else
                    {
                        EnemyAi.ChangeState(EnemyAi.GetAttackState());
                    }

                }
                
            }
        }

        public override void Exit() {
            Debug.Log("Exiting Idle State");
        }
    }
}