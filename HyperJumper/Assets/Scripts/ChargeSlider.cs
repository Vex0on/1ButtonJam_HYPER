using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeSlider : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    private Scrollbar _scrollbar;
    
    // Start is called before the first frame update
    void Start()
    {
        _scrollbar = GetComponent<Scrollbar>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((float)_playerController.GetSpaceHoldTime() <= _playerController.GetShortPressTime()) return;

        _scrollbar.size = (float)_playerController.GetSpaceHoldTime() / _playerController.GetMaxJumpHigh();
    }
}
