using UnityEngine;

public class ARInteractionObject : MonoBehaviour
{
    [Header("Configuración Básica")]
    [SerializeField] private bool m_canMove;
    [SerializeField] private bool m_canRotate;
    [SerializeField] private bool m_canDelete;
    [SerializeField] private TypeOfBone m_typeOfBone;

    [Header("Debug System - Asignación Automática")]
    [SerializeField] private bool m_canDebug = true;
    [SerializeField] private string m_horizontalBarName = "flechas_horizontal";
    [SerializeField] private string m_verticalBarName = "flecha_vertical";
    [SerializeField] private string m_rotationBarName = "flecha_rotacion";

    private GameObject m_horizontalBar;
    private GameObject m_verticalBar;
    private GameObject m_rotationBar;

    private void Awake()
    {
        if (m_canDebug)
        {
            // Buscar los ejes por nombre en los hijos
            m_horizontalBar = FindChildByName(m_horizontalBarName);
            m_verticalBar = FindChildByName(m_verticalBarName);
            m_rotationBar = FindChildByName(m_rotationBarName);

            // Desactivar todos al inicio
            DesactivateDebugger();
        }
    }

    private GameObject FindChildByName(string name)
    {
        Transform child = transform.Find(name);
        if (child == null)
        {
            Debug.LogError($"No se encontró el objeto hijo '{name}' en {gameObject.name}");
            return null;
        }
        return child.gameObject;
    }

    public void DesactivateDebugger()
    {
        if (!m_canDebug) return;

        SetDebugObjectsState(false, false, false);
    }

    public void setDebugSystem(bool debug, bool moveDebug)
    {
        if (!m_canDebug) return;

        if (debug)
        {
            SetDebugObjectsState(false, false, true); // Solo rotación
        }
        else
        {
            MoveSystem(moveDebug);
        }
    }

    private void MoveSystem(bool orientation)
    {
        SetDebugObjectsState(!orientation, orientation, false);
    }

    private void SetDebugObjectsState(bool horizontal, bool vertical, bool rotation)
    {
        if (m_horizontalBar != null) m_horizontalBar.SetActive(horizontal);
        if (m_verticalBar != null) m_verticalBar.SetActive(vertical);
        if (m_rotationBar != null) m_rotationBar.SetActive(rotation);
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
