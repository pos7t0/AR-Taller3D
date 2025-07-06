using UnityEngine;

public class BulletSystem : MonoBehaviour
{
    [SerializeField] private float m_durationAlive=2f;

    private void Update()
    {
        if (m_durationAlive<=0)
        {
            Destroy(gameObject);
        }
        m_durationAlive -= Time.deltaTime;
    }
}
