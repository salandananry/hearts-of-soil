using UnityEngine;

[RequireComponent (typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private SeedPlanter seedPlanter;
    [SerializeField] private TileWaterer tileWaterer;
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
            TryPlanting();
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            TryWatering();
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
    private void TryPlanting()
    {
        seedPlanter.PlantSeed(transform.position);
    }
    private void TryWatering()
    {
        tileWaterer.WaterSoil(transform.position);
    }
}