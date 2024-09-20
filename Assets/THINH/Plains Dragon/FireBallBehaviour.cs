using UnityEngine;

public class FireBallBehaviour : StateMachineBehaviour
{
    bool isFire = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isFire = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!isFire && stateInfo.normalizedTime >= 0.5f)
        {
            isFire = true;
            DragonController dragon = animator.GetComponent<DragonController>();
            dragon.Fire();
        }

        //if (stateInfo.normalizedTime >= 1F)
        //{
        //    DragonController dragon = animator.GetComponent<DragonController>();
        //    dragon.FireCount += 1;
        //    Debug.Log("oke2");
        //}
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("oke");
        DragonController dragon = animator.GetComponent<DragonController>();
        dragon.FireCount += 1;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
