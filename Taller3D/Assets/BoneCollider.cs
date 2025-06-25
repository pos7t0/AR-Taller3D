using UnityEngine;

public class BoneCollider : MonoBehaviour
{
    [SerializeField] private TypeOfBone m_myBone;
    [SerializeField] private float m_damage;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            GetComponentInParent<SkullMovement>().TakeDamage(m_myBone, m_damage);
        }
    }

}
