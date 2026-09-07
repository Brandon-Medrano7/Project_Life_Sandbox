using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Objetivo que seguirá la cámara")]
    public Transform target;

    [Header("Configuración")]
    public float smoothSpeed = 8f;
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}