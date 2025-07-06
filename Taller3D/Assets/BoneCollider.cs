using UnityEngine;

public class BoneCollider : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] private SkinnedMeshRenderer m_meshRenderer;
    [Header("Colores")]
    [SerializeField] private Color m_originalMaterial;
    [SerializeField] private Color m_wrongMaterial;
    [SerializeField] private Color m_goodMaterial;
    [SerializeField] private float m_durationColor=2f;
    private Color m_index;
    private float m_timer=0f; 
    private bool m_isOriginalMaterial=true;
    [Header("atributos")]
    [SerializeField] private TypeOfBone m_boneName;
    [SerializeField] private float damage=1f;

    private void Start()
    {
        m_originalMaterial = m_meshRenderer.material.color;
    }
    private void Update()
    {
        ReturnColor();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            m_timer = 0;
            if (GetComponentInParent<SkullMovement>().GetWeakZone()==m_boneName)
            {
                Debug.Log("bueno");
                ChangeColor(m_goodMaterial);
                GetComponentInParent<SkullMovement>().TakeDamage(m_boneName, damage);
                Destroy(collision.gameObject);
            }
            else
            {
                Debug.Log("malo");
                ChangeColor(m_wrongMaterial);
                Destroy(collision.gameObject);
            }
            
        }
    }

    private void ReturnColor()
    {
        if (!m_isOriginalMaterial)
        {
            m_timer += Time.deltaTime;
            float t = m_timer / m_durationColor;

            // Lerp hacia el color original
            m_meshRenderer.material.color = Color.Lerp(m_index, m_originalMaterial, t);

            if (t >= 1f)
            {
                m_isOriginalMaterial = true; // Termina la transición
                
            }
            
        }
    }
    private void ChangeColor(Color color)
    {
        m_isOriginalMaterial = false;
        m_index = color;
        m_meshRenderer.material.color =m_index;
    }



}
