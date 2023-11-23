using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject buttons;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private TMP_Dropdown resolutionPicker;
    [SerializeField] private TMP_InputField playerNickname;
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
        optionsPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void ToggleFullscreen()
    {
        Screen.fullScreen = fullscreenToggle.isOn;
    }

    public void ChangeResolution()
    {
        Resolution resolution = Screen.resolutions[resolutionPicker.value];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetPlayerName()
    {
        string playerName = playerNickname.text;
        Debug.Log("Player Name: " + playerName);
    }
}
