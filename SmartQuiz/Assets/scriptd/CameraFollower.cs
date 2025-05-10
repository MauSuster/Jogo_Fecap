using UnityEngine;

public class CameraFollower : MonoBehaviour
{

    public Transform target; // A cápsula a ser seguida
    public Vector3 offset = new Vector3(0f, 1f, 1f); // Posição da câmera em relação ao alvo
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        }
    }

    // Permite trocar o alvo em tempo de execução
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}