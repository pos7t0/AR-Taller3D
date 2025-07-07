using UnityEngine;

public class ARInteractionObject : MonoBehaviour
{
    [SerializeField] private bool m_canMove;
    [SerializeField] private bool m_canRotate;
    [SerializeField] private bool m_canDelete;
    [SerializeField] private bool m_canDebug=false;
    [SerializeField] private GameObject m_horizontalBar;
    [SerializeField] private GameObject m_verticalBar;
    [SerializeField] private GameObject m_rotationBar;
    [SerializeField] private TypeOfBone m_typeOfBone;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Update()
    {

    }

    public void DesactivateDebugger()
    {
        m_horizontalBar.SetActive(false);
        m_verticalBar.SetActive(false);
        m_rotationBar.SetActive(false);

    }
    public void setDebugSystem(bool debug,bool moveDebug)
    {
        if (!m_canDebug)
            return;

        if (debug)
        {
            m_verticalBar.SetActive(false);
            m_horizontalBar.SetActive(false);
            m_rotationBar.SetActive(true);
        }
        else
        {
            MoveSystem(moveDebug);
            m_rotationBar.SetActive(false);
        }

    }
    private void MoveSystem(bool orientation)
    {
        if (orientation)
        {
            m_verticalBar.SetActive(true);
            m_horizontalBar.SetActive(false);
        }
        else
        {
            m_verticalBar.SetActive(false);
            m_horizontalBar.SetActive(true);
        }
    }
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
