using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public Transform cameraTransform;
    [Range(0f, 0.5f)]
    public float parallaxFactor = 0.1f;

    private Vector3 lastCamPos;

    void Start()
    {
        lastCamPos = cameraTransform.position;
    }

    void LateUpdate()
    {
        Vector3 delta = cameraTransform.position - lastCamPos;
        transform.position += new Vector3(delta.x * parallaxFactor, delta.y * parallaxFactor, 0);
        lastCamPos = cameraTransform.position;
    }
}
