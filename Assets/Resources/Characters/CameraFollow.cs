using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // el jugador
    public Vector3 offset = new Vector3(0, 0, -10); // para mantener la cámara detrás
    public float smoothSpeed = 0.125f; // suavizado
    
    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
