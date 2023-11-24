using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using NUnit.Framework;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject buttons;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private TMP_Dropdown resolutionPicker;
    [SerializeField] private TMP_InputField playerNickname;

    private Resolution[] _resolutions;
    private List<Resolution> _filteredResolutions;
    private int _currentResolutionIdx = 0;
    private float _currentRefreshRate;
    public bool isGameStarted = false;

    [System.Obsolete]
    private void Start()
    {
        Time.timeScale = 0f;

        _resolutions = Screen.resolutions;
        _filteredResolutions = new List<Resolution>();

        resolutionPicker.ClearOptions();
        _currentRefreshRate = Screen.currentResolution.refreshRate;

        Debug.Log("RefreshRate: " + _currentRefreshRate);

        for (int i = 0; i < _resolutions.Length; i++)
        {
            Debug.Log("Resolution: " + _resolutions[i]);
            if (_resolutions[i].refreshRate == _currentRefreshRate)
            {
                _filteredResolutions.Add(_resolutions[i]);
            }
        }

        List<string> options = new List<string>();
        for (int i = 0; i < _filteredResolutions.Count; i++)
        {
            string resolutionOptions = _filteredResolutions[i].width + "x" + _filteredResolutions[i].height + " " + _filteredResolutions[i].refreshRate + "Hz";
            options.Add(resolutionOptions);
            if (_filteredResolutions[i].width == Screen.width && _filteredResolutions[i].height == Screen.height)
            {
                _currentResolutionIdx = i;
            }
        }

        resolutionPicker.AddOptions(options);
        resolutionPicker.value = _currentResolutionIdx;
        resolutionPicker.RefreshShownValue();

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

    public void SetPlayerName()
    {
        string playerName = playerNickname.text;
        Debug.Log("Player Name: " + playerName);
    }

    public void SetResolution(int resolutionIdx)
    {
        Resolution resolution = _filteredResolutions[resolutionIdx];
        Screen.SetResolution(resolution.width, resolution.height, true);
    }

    public void ToggleFullscreen()
    {
        Screen.fullScreen = fullscreenToggle.isOn;
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
    }
}
