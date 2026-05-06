using UnityEngine;

[RequireComponent (typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private TileCheck tileCheck;
    private PlayerMovement movement;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        MovementInput();

        if(Input.GetKeyDown(KeyCode.E))
        {
            InspectTile();
        }
    }
    private void MovementInput()
    {
        Vector2 input = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;

        movement.Move(input);
    }
    private void InspectTile()
    {
        tileCheck.ShowTileStatusAt(transform.position);
    }
}