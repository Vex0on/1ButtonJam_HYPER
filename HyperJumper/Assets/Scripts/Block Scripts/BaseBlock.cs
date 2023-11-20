using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBlock : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D _playerRB;
    [SerializeField] protected PlayerController _playerController;
}
