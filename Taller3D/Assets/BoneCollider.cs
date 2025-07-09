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

    private HUDShotter m_hud;

    private void Start()
    {
        m_hud=FindAnyObjectByType<HUDShotter>();
        
        m_originalMaterial = m_meshRenderer.material.color;
    }
    private void Update()
    {
        ReturnColor();
    }


    

    public void ReceiveHit(BulletSystem bullet)
    {
        m_timer = 0;

        SkullMovement skull = GetComponentInParent<SkullMovement>();
        if (skull == null)
        {
            Debug.LogWarning("No se encontró SkullMovement");
            Destroy(bullet.gameObject);
            return;
        }

        if (skull.GetWeakZone() == m_boneName)
        {
            m_hud.GoodAudio();
            Debug.Log("Impacto bueno");
            ChangeColor(m_goodMaterial);
            skull.TakeDamage(m_boneName, damage);
        }
        else
        {
            m_hud.BadAudio();
            Debug.Log("Impacto malo");
            ChangeColor(m_wrongMaterial);
        }

        Destroy(bullet.gameObject);
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
