using UnityEngine;

[RequireComponent (typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private CheckSoil checkSoil;
    private PlayerMovement movement;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        Vector2 input = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;

        movement.Move(input);

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPlant();
        }
    }
    private void TryPlant()
    {
        Vector2 adjustedPos = (Vector2)transform.position + new Vector2(0, -0.5f);
        checkSoil.TileCheck(adjustedPos);
    }
}