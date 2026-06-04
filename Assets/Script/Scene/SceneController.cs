using UnityEngine;

public class SceneController : MonoBehaviour
{
   public void ChangeScene(string sceneName)
    {
        SceneLoader.instance.SceneChange(sceneName);
    }
}
