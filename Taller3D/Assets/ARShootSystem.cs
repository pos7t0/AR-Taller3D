using UnityEngine;
using UnityEngine.InputSystem;

public class ARShootSystem : MonoBehaviour
{

    [SerializeField] private GameObject proyectilPrefab;
    [SerializeField] private float velocidadDisparo = 10f;
    [SerializeField] private float m_intervalShoot = 2f;
    private bool m_istouch=false;
    private float m_timer=0;
    void Update()
    {
        if (m_istouch && m_timer > m_intervalShoot)
        {
            Disparar();
            m_timer = 0;
        }
        else
            m_timer += Time.deltaTime;
        
        
    }

    private void Disparar()
    {
        // Crear proyectil desde la posici�n de la c�mara
        Vector3 origen = Camera.main.transform.position;
        Quaternion rotacion = Camera.main.transform.rotation;

        GameObject proyectil = Instantiate(proyectilPrefab, origen, rotacion);

        // Aplicar fuerza en la direcci�n de la c�mara
        Rigidbody rb = proyectil.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Camera.main.transform.forward * velocidadDisparo;
        }
    }

    public void Touched()
    {
        m_istouch = true;
    }
    public void IsNotTouched()
    {
        m_istouch = false;
    }
}
