using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Video;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private List<Button> buttonsMain;
    [SerializeField] private List<Button> buttonsOptions;
    [SerializeField] private GameObject buttonsPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private TMP_Dropdown resolutionPicker;
    [SerializeField] private TMP_InputField playerNickname;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private TextMeshProUGUI subtitleText;

    [SerializeField] private List<string> videoURLs;
    [SerializeField] private List<string> subtitles;
    private int currentVideoIndex = 0;

    [Header("Debug")]
    [SerializeField] private int chosenMainIndex;
    [SerializeField] private int chosenOptionIndex;
    [SerializeField] private float _shortPressTime = 0.2f;
    [SerializeField] private double _startSpaceHoldTime;
    [SerializeField] private double _spaceHoldTime;

    public bool isGameStarted = false;

    [System.Obsolete]
    private void Start()
    {
        Time.timeScale = 0f;

        chosenMainIndex = 1;
        buttonsMain[chosenMainIndex].Select();
    }

    public void OnUIControl(InputAction.CallbackContext context)
    {
        if (isGameStarted) return;

        switch (context.phase)
        {
            case InputActionPhase.Started:
                _startSpaceHoldTime = context.time;
                break;
            case InputActionPhase.Canceled:
                _spaceHoldTime = context.time - _startSpaceHoldTime;

                if (_spaceHoldTime < _shortPressTime)
                {
                    if (optionsPanel.activeSelf)
                    {
                        chosenOptionIndex = ++chosenOptionIndex % buttonsOptions.Count;
                        buttonsOptions[chosenOptionIndex].Select();
                    }
                    else
                    {
                        chosenMainIndex = ++chosenMainIndex % buttonsMain.Count;
                        buttonsMain[chosenMainIndex].Select();
                    }
                }
                else
                {
                    if (optionsPanel.activeSelf)
                    {
                        buttonsOptions[chosenOptionIndex].onClick.Invoke();
                    }
                    else
                    {
                        buttonsMain[chosenMainIndex].onClick.Invoke();
                    }
                }
                break;
        }
    }

    public void PlayGame()
    {
        buttonsPanel.SetActive(false);

        Time.timeScale = 1f;
        isGameStarted = true;
    }

    public void Options()
    {
        Debug.Log("Options");
        buttonsPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void SetPlayerName()
    {
        string playerName = playerNickname.text;
        Debug.Log("Player Name: " + playerName);
    }

    public void CloseOptions()
    {
        buttonsPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

    public void PlayNextVideo()
    {
        currentVideoIndex = (currentVideoIndex + 1) % videoURLs.Count;
        string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoURLs[currentVideoIndex]);
        Debug.Log("Trying to play video: " + videoPath);

        videoPlayer.url = videoPath;
        videoPlayer.Play();


        if (currentVideoIndex < subtitles.Count)
        {
            subtitleText.text = subtitles[currentVideoIndex];
        }
    }
}
