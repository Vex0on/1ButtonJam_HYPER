using UnityEngine;

public abstract class Block : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D _playerRB;


    public virtual void OnTouch(PlayerController player) { }
    public virtual void OnUpdate(PlayerController player) { }
    public virtual void OnDetach(PlayerController player) { }
}
