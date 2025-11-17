using UnityEngine;

namespace Jsgaona {

    public class Operator : StateMachineBehaviour {

        public string nameParameter;
        public bool value = false;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            Debug.Log("Entro anim");
            animator.SetBool(nameParameter, value);
        }

        /*
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            animator.SetBool(nameParameter, value);
        }
        */

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            animator.SetBool(nameParameter, !value);
            Debug.Log("Salio anim");
        }
    }
}