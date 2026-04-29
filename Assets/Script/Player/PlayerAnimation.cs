using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void UpdateAnimation(Vector2 input, Vector2 lastDirection, bool isMoving)
    {
        Vector2 dir = input == Vector2.zero ? lastDirection : input;

        animator.SetFloat("X", dir.x);
        animator.SetFloat("Y", dir.y);
        animator.SetBool("IsMoving", isMoving);
    }
}