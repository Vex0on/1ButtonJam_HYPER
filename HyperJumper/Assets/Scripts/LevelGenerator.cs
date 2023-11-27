using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public List<GameObject> easyRooms = new();
    public List<GameObject> mediumRooms = new();
    public List<GameObject> hardRooms = new();

    [Header("difficulty levels (14.862 - first level + 12 for every next)")]
    [SerializeField] private float easyLevel;
    [SerializeField] private float mediumLevel;
    [SerializeField] private float hardLevel;
    [SerializeField] private float finishLevel;

    [Header("Objects")]
    public PlayerController player;

    [Header("Values")]
    public int distanceToNextLevel;

    [Header("Debug")]
    [SerializeField] private bool _isGenerating;
    [SerializeField] private float _currentLevelPositionToSpawn;
    [SerializeField] private float _currentLevel;
    [SerializeField] private float _levelHeightToGenerate;
    [SerializeField] private float _heightLevelPoint;
    [SerializeField] private GameObject _currentGenerateRoom;


    private void Update()
    {
        if (!_isGenerating) return;

        _currentLevel = player.GetCurrentHeight();
        _levelHeightToGenerate = _currentLevel + distanceToNextLevel;

        if (_levelHeightToGenerate > _heightLevelPoint)
        {
            if (_levelHeightToGenerate > finishLevel)
            {
                _isGenerating = false;
                return;
            }
            else if (_levelHeightToGenerate > hardLevel)
                _currentGenerateRoom = hardRooms[Random.Range(0, hardRooms.Count)];
            else if (_levelHeightToGenerate > mediumLevel)
                _currentGenerateRoom = mediumRooms[Random.Range(0, mediumRooms.Count)];
            else if (_levelHeightToGenerate > easyLevel)
                _currentGenerateRoom = easyRooms[Random.Range(0, easyRooms.Count)];

            CreateRoom();
            _heightLevelPoint = _currentLevelPositionToSpawn;
        }
    }

    private void CreateRoom()
    {
        if (_currentGenerateRoom == null) return;

        Instantiate(_currentGenerateRoom, new Vector2(0, _currentLevelPositionToSpawn + 1.767f), Quaternion.identity);
        _currentLevelPositionToSpawn += 12;
    }
}
