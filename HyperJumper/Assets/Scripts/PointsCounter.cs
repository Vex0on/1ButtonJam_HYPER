using TMPro;
using UnityEngine;

public class PointsCounter : MonoBehaviour
{
    public PlayerController player;

    [SerializeField] private TextMeshProUGUI _textCurrent;
    [SerializeField] private TextMeshProUGUI _textHighest;
    [SerializeField] private float _currentPoints;
    [SerializeField] private float _highestPoints;

    private float startPoint;


    private void Start()
    {
        startPoint = player.GetCurrentHeight();
        UpdateBestHeightPoint(PlayerPrefs.GetFloat("Height", 0));
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat("Height", _highestPoints);
        PlayerPrefs.Save();
    }
    private void OnDestroy()
    {
        PlayerPrefs.SetFloat("Height", _highestPoints);
        PlayerPrefs.Save();
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("Height", _highestPoints);
        PlayerPrefs.Save();
    }

    private void Update()
    {
        float currentPoints = Mathf.Floor((player.GetCurrentHeight() - startPoint) * 100);
        if (currentPoints > _highestPoints)
            UpdateBestHeightPoint(currentPoints);

        UpdateHeightPoint(currentPoints);
    }

    private void UpdateBestHeightPoint(float value)
    {
        _highestPoints = value;
        _textHighest.SetText($"Best: {_highestPoints}");
    }

    private void UpdateHeightPoint(float value)
    {
        _currentPoints = value;
        _textCurrent.SetText($"Height: {_currentPoints}");
    }
}
