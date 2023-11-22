using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject buttons;
    public bool isGameStarted = false;


    private void Start()
    {
        Time.timeScale = 0f;
    }

    public void PlayGame()
    {
        if (buttons != null)
        {
            buttons.SetActive(false);
        }

        Time.timeScale = 1f;
        isGameStarted = true;
    }

    public void Options()
    {
        Debug.Log("Options");
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
