using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;

public class SkullMovement : MonoBehaviour
{
    public float velocidad = 2f;
    private ARPlane planoBase;

    void Start()
    {
        // Buscar el plano donde fue colocado el objeto
        planoBase = FindObjectOfType<ARFloorDetector>()?.GetPlaneDetected();

        //if (planoBase == null)
            
    }

    void Update()
    {
        // este sistema era para probar que se mueve el objeto en 3D, 
        // trata de ver como es que se mueva el personaje
        Vector3 direccion = Vector3.zero;

        if (Keyboard.current.wKey.isPressed)
            direccion += Vector3.forward;
        if (Keyboard.current.sKey.isPressed)
            direccion += Vector3.back;
        if (Keyboard.current.aKey.isPressed)
            direccion += Vector3.left;
        if (Keyboard.current.dKey.isPressed)
            direccion += Vector3.right;

        if (direccion == Vector3.zero) return;

        Vector3 nuevaPosicion = transform.position + direccion * velocidad * Time.deltaTime;

        if (EstaDentroDelPlano(nuevaPosicion))
        {
            transform.position = nuevaPosicion;
        }
        else
        {
            
        }
    }

    bool EstaDentroDelPlano(Vector3 posicionMundo)
    {
        if (planoBase == null) return true;

        Vector3 posLocal = planoBase.transform.InverseTransformPoint(posicionMundo);
        var boundary = planoBase.boundary;

        int n = boundary.Length;
        bool dentro = false;

        for (int i = 0, j = n - 1; i < n; j = i++)
        {
            Vector2 pi = boundary[i];
            Vector2 pj = boundary[j];

            if (((pi.y > posLocal.z) != (pj.y > posLocal.z)) &&
                (posLocal.x < (pj.x - pi.x) * (posLocal.z - pi.y) / (pj.y - pi.y) + pi.x))
            {
                dentro = !dentro;
            }
        }

        return dentro;
    }
}
