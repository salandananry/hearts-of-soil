using UnityEngine;
using System;
[RequireComponent (typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    public event Action <Vector2> OnPlayerInteract;
    /*
    [SerializeField] private float interactDistance;
    private PlayerMovement movement;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
    }
    */
    private void Update()
    {
        if (Time.timeScale == 0) return;

        //MovementInput();

        if(Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //if (!IsWithinInteractRange(mousePos)) return;
            OnPlayerInteract?.Invoke(mousePos);
        }
    }
    /*
    private void MovementInput()
    {
        Vector2 input = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;

        movement.Move(input);
    }
    
    private bool IsWithinInteractRange(Vector2 targetPos)
    {
        return Vector2.Distance(transform.position, targetPos)
               <= interactDistance;
    }
    */
}