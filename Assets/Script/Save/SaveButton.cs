using UnityEngine;

public class SaveButton : MonoBehaviour
{
    public void SaveGame()
    {
        SaveManager.instance.SaveGame();
    }
}
