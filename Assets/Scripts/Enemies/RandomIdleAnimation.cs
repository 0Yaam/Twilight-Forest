using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Animator myAnimator;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (!myAnimator) { return; }

        AnimatorStateInfo state = myAnimator.GetCurrentAnimatorStateInfo(0);
        myAnimator.Play(state.fullPathHash, -1, Random.Range(0f, 1f));
    }
}
