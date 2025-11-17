using UnityEngine;

namespace Jsgaona {

    // Se emplea este scriptable object para generar el FMS del estado de ataque basico
    [CreateAssetMenu(fileName = "NewRangeAttackState", menuName = "Juego/State/RangeAttack State")]
    public class AIStateRangeAttack : AIStateBasicAttack {

        protected override void Attack() {
            base.Attack();
            combatEnemy.UseWeapon();
        }
    }
}