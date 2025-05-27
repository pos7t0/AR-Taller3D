using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SpawnObject : MonoBehaviour
{
    public GameObject objeto; // Asigna tu modelo 3D en el Inspector
    private bool yaInstanciado = false;

    void Start()
    {
        if (!yaInstanciado && Camera.main != null)
        {
            // Posición: al frente de la cámara (1 metro)
            Vector3 posicion = new Vector3(0,0,1);

            // Rotación: que mire en la misma dirección que la cámara
            Quaternion rotacion = Quaternion.LookRotation(Camera.main.transform.forward);

            // Instanciar objeto
            Instantiate(objeto, posicion, rotacion);
            yaInstanciado = true;
        }
    }
}
