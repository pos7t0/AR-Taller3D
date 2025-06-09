using UnityEngine;

public class ARInteractionObject : MonoBehaviour
{
    [SerializeField] private bool m_canMove;
    [SerializeField] private bool m_canRotate;
    [SerializeField] private bool m_canDelete;
    [SerializeField] private TypeOfBone m_typeOfBone;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public bool CanMove()
    {
        return m_canMove;
    }
    public bool CanRotate()
    {
        return m_canRotate;
    }
    public bool CanDelete()
    {
        return m_canDelete;
    }
    public TypeOfBone GetTypeOfBone()
    {
        return m_typeOfBone;
    }
}
