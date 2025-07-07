using UnityEngine;

public class BulletSystem : MonoBehaviour
{
    [SerializeField] private float m_durationAlive=2f;
    public bool m_collision=false;
    private void Update()
    {
        if (m_durationAlive<=0)
        {
            Destroy(gameObject);
        }
        m_durationAlive -= Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (m_collision) return;

        // Solo reaccionar si el otro tiene un BoneCollider
        if (collision.gameObject.TryGetComponent(out BoneCollider bone))
        {
            m_collision = true;
            bone.ReceiveHit(this);
        }
    }

}
