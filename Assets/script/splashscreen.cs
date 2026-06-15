using UnityEngine;
using UnityEngine.SceneManagement;

public class splashscreen : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
