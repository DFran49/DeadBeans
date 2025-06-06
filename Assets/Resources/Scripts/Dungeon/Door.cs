using System;
using UnityEngine;

public class Door : MonoBehaviour
{
    public event Action<Collider2D> OnHitboxTriggerEnter;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        OnHitboxTriggerEnter?.Invoke(other);
    }
}
