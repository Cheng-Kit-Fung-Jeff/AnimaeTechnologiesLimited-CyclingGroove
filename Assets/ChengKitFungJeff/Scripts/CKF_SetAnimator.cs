using UnityEngine;

public class CKF_SetAnimator : MonoBehaviour
{
    private Animator animator;
    public bool keepAnimatorStateOnDisable = true;

    public void Awake()
    {
        animator = GetComponent<Animator>();
        animator.keepAnimatorStateOnDisable = keepAnimatorStateOnDisable;
    }

    public void SetBoolTrue(string key)
    {
        animator.SetBool(key, true);
        
    }
    public void SetBoolFalse(string key)
    {
        animator.SetBool(key, false);
    }
}
