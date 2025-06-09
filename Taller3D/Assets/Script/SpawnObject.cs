using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SpawnObject : MonoBehaviour
{
    

    void Start()
    {
        
    }

    public void SpawnArObject(GameObject game)
    {
        if (Camera.main != null)
        {
            Vector3 posicion = Camera.main.transform.position + Camera.main.transform.forward * 1.5f;

            Quaternion rotacion = Quaternion.identity;

            Instantiate(game, posicion, rotacion);
        }
    }
}
