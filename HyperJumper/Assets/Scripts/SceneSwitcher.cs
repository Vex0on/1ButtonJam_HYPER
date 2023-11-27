using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    void Start()
    {
        Invoke("SwitchToGameScene", 5f);
    }

    void SwitchToGameScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
