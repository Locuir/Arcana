using UnityEngine;

namespace KevinIglesias
{
    public enum BowConditions
    {
        OnEnter,
        OnExit
    }

    public enum BowActions
    {
        Pull,
        Release,
        Cancel
    }

    public class HumanArcherBowSMB : StateMachineBehaviour
    {
        public BowConditions condition;
        public BowActions bowAction;
        public float delay;
        public float duration;

        private HumanArcherController hAC;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (condition == BowConditions.OnEnter)
            {
                if (!hAC)
                {
                    hAC = animator.GetComponentInChildren<HumanArcherController>();
                    if (!hAC)
                    {
                        hAC = animator.GetComponent<HumanArcherController>();
                    }
                }

                if (bowAction == BowActions.Pull)
                {
                    hAC.LoadBow(delay, duration);
                }
                else
                {
                    hAC.ShootArrow(delay, duration);
                }
            }
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (condition == BowConditions.OnExit)
            {
                if (!hAC)
                {
                    hAC = animator.GetComponentInChildren<HumanArcherController>();
                    if (!hAC)
                    {
                        hAC = animator.GetComponent<HumanArcherController>();
                    }
                }

                if (bowAction == BowActions.Pull)
                {
                    hAC.LoadBow(delay, duration);
                }
                else
                {
                    hAC.ShootArrow(delay, duration);
                }
            }
        }
    }
}