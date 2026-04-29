using UnityEngine;

[RequireComponent(typeof(PlayerAnimation))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Vector2 lastDirection;
    private PlayerAnimation playerAnimation;
    private bool isMoving;

    private void Start()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    public void Move(Vector2 input)
    {
        transform.Translate(input * speed * Time.deltaTime);

        if (input != Vector2.zero)
            lastDirection = input;

        isMoving = input != Vector2.zero;

        playerAnimation.UpdateAnimation(input, lastDirection, isMoving);
    }
}