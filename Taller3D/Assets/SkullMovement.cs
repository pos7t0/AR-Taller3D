using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;

public class SkullMovement : MonoBehaviour
{
    [Header("Animaciones")]
    [SerializeField] private string[] m_waitTriggers;
    [SerializeField] private Animator m_animator;
    [Header("Atributos")]
    [SerializeField] private float m_health;
    [SerializeField] private TypeOfBone m_weakZone;
    [SerializeField] private float m_weakDuration;
    [SerializeField] private float m_speed = 2f;
    
    private Vector3 m_targetPosition;
    private float m_minDistanceFromPlayer = 1.5f; // Puedes ajustarlo
    private float m_reachDistance = 0.1f;
    private bool m_isWaiting = false;

    private ARPlane planoBase;
    private float m_timerWeakZone=0f;
    private HUDShotter m_hudShotter;

    void Start()
    {
        // Buscar el plano donde fue colocado el objeto
        planoBase = FindObjectOfType<ARFloorDetector>()?.GetPlaneDetected();
        m_hudShotter = FindAnyObjectByType<HUDShotter>();
        m_hudShotter.ShowWeakText(m_weakZone);
        m_hudShotter.SetSlider(m_health);
        //if (planoBase == null)
            
    }

    void Update()
    {
        ChangeWeakZone();
        if (!m_isWaiting)
        {
            Vector3 direction = m_targetPosition - transform.position;
            direction.y = 0f;

            if (direction.magnitude < m_reachDistance)
            {
                StartWaiting();
                return;
            }

            Vector3 moveDirection = direction.normalized;
            Vector3 newPosition = transform.position + moveDirection * m_speed * Time.deltaTime;

            if (EstaDentroDelPlano(newPosition))
            {
                transform.position = newPosition;
            }

            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }

            if (m_animator != null)
            {
                m_animator.SetBool("Walk", true);
            }
        }
        else
        {
            if (m_animator != null)
            {
                m_animator.SetBool("Walk", false);
            }
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
    private void ChangeWeakZone()
    {
        m_timerWeakZone += Time.deltaTime;
        Debug.Log("tiempo para cambiar "+m_timerWeakZone);
        if (m_timerWeakZone>m_weakDuration)
        {
            m_timerWeakZone = 0;
            RandomWeakZone();
            
        }
    }
    private void RandomWeakZone()
    {
        TypeOfBone randomWeakZone = (TypeOfBone)Random.Range(0, System.Enum.GetValues(typeof(TypeOfBone)).Length);
        m_weakZone = randomWeakZone;
        m_hudShotter.ShowWeakText(m_weakZone);
    }

    public void TakeDamage(TypeOfBone boneHit,float damage)
    {
        if (m_weakZone==boneHit)
        {
            m_health -= damage;
            m_hudShotter.ChangeSlider(m_health);
        }
    }
    public TypeOfBone GetWeakZone()
    {
        return m_weakZone;
    }

    private void SetRandomTargetPosition()
    {
        if (planoBase == null) return;

        Vector3 centroPlano = planoBase.transform.position;
        Vector3 nuevoDestino = centroPlano;
        int intentos = 0;
        bool destinoValido = false;

        float rango = 0.8f * Mathf.Min(planoBase.size.x, planoBase.size.y);

        while (intentos < 20)
        {
            float offsetX = Random.Range(-rango, rango);
            float offsetZ = Random.Range(-rango, rango);
            nuevoDestino = planoBase.transform.position
                         + planoBase.transform.right * offsetX
                         + planoBase.transform.forward * offsetZ;

            if (EstaDentroDelPlano(nuevoDestino) &&
                Vector3.Distance(nuevoDestino, Camera.main.transform.position) >= m_minDistanceFromPlayer)
            {
                destinoValido = true;
                break;
            }

            intentos++;
        }

        if (destinoValido)
        {
            m_targetPosition = nuevoDestino;
        }
        else
        {
            Debug.LogWarning("No se encontró una posición válida dentro del plano. Se reintentará en el siguiente ciclo.");
            m_isWaiting = true;
            Invoke(nameof(OnIdleAnimationComplete), 2f); // espera 2 segundos y reintenta
        }
    }

    private void StartWaiting()
    {
        m_isWaiting = true;

        if (m_animator != null && m_waitTriggers.Length > 0)
        {
            int randomIndex = Random.Range(0, m_waitTriggers.Length);
            string randomTrigger = m_waitTriggers[randomIndex];
            m_animator.SetTrigger(randomTrigger);
        }

        // Mirar al jugador al detenerse
        Vector3 lookDirection = Camera.main.transform.position - transform.position;
        lookDirection.y = 0f;

        if (lookDirection.sqrMagnitude > 0.001f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = lookRotation;
        }
    }

    public void OnIdleAnimationComplete()
    {
        m_isWaiting = false;
        SetRandomTargetPosition();
    }
}
