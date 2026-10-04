using UnityEngine;
using System;
using UnityEngine.EventSystems;

[RequireComponent (typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    public event Action <Vector2> OnPlayerInteract;
    private void Update()
    {
        if (Time.timeScale == 0) return;
        if (!Input.GetMouseButtonDown(0)) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        OnPlayerInteract?.Invoke(mousePos);
    }
}