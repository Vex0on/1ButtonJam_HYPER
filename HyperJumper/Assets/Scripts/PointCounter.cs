using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;

public class PointCounter : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Vector2 _startingPoint;
    [SerializeField] private Vector2 _highestPoint;
    // Start is called before the first frame update
    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _startingPoint = _playerController.gameObject.transform.position; 
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 currentPoint = new Vector2(0, _playerController.gameObject.transform.position.y - _startingPoint.y);
        if(currentPoint.y > _highestPoint.y)
        {
            _highestPoint = currentPoint;
        }
        float pointsToConvert = Mathf.Floor(_highestPoint.y * 100);
        _text.text = "Points: "+pointsToConvert.ToString();
    }
}
