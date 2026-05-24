using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] private Sprite cursorSprite;

    private void Start()
    {
        Texture2D texture = cursorSprite.texture;

        Cursor.SetCursor(texture, Vector2.zero, CursorMode.Auto);
    }
}
