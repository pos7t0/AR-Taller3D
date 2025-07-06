using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SetupARScene : MonoBehaviour
{
    public ARSession arSession;
    public Camera arCamera;

    void Awake()
    {
        if (arCamera == null)
        {
            Debug.LogError("Cámara AR no asignada.");
            return;
        }

        if (arSession == null)
        {
            Debug.LogError("AR Session no asignada.");
            return;
        }

        // Activar sesión y cámara si están desactivadas
        if (!arSession.enabled)
            arSession.enabled = true;

        if (!arCamera.gameObject.activeSelf)
            arCamera.gameObject.SetActive(true);
    }
}