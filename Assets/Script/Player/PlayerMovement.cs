using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerAnimation))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private Vector2 input;
    private Vector2 lastDirection;

    private Rigidbody2D rigidBody2D;
    private PlayerAnimation playerAnimation;
    private bool isMoving;

    private void Awake()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 newInput)
    {
        input = newInput.normalized;

        if (newInput != Vector2.zero)
            lastDirection = newInput;

        isMoving = input.sqrMagnitude > 0.001f;

        playerAnimation.UpdateAnimation(newInput, lastDirection, isMoving);
    }
    private void FixedUpdate()
    {
        Vector2 isoAdjusted = new Vector2(
            input.x,
            input.y * 0.5f
        );

        rigidBody2D.MovePosition(
            rigidBody2D.position + isoAdjusted * speed * Time.fixedDeltaTime
        );
    }
    public Vector2 GetLastDirection()
    {
        return lastDirection;
    }
}