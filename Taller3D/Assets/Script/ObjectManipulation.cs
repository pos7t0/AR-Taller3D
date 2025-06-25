using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;


public class ObjectManipulation : MonoBehaviour
{
    public GameObject m_arObject;
    private ARInteractionObject m_arInteractionObject;

    [SerializeField] private Camera m_arCamera;

    private bool m_isARObjectSelected;
    private Vector2 m_initialTouchPos;

    [Header("UI References")]
    [SerializeField] public Image m_axisToggleButtonImage; // Referencia a la imagen del botón
    [SerializeField] public Sprite m_xzAxisSprite; // Sprite para el modo XZ
    [SerializeField] public Sprite m_yAxisSprite;  // Sprite para el modo Y

    [SerializeField] public Image transformToggleButtonImage; // Referencia a la imagen del botón
    [SerializeField] public Sprite transform;  // Sprite para el modo Y
    [SerializeField] public Sprite rotate;  // Sprite para el modo Y


    [SerializeField] private bool m_isRotationMode = false;

    [SerializeField] private float m_speedMovement = 4.0f;

    [SerializeField] private float m_screenfactor;

    [SerializeField] private bool m_changeAxis;

    private void Start()
    {
        m_arCamera = Camera.main;
    }
    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    void Update()
    {
        foreach (Touch touch in Touch.activeTouches)
        {
            Vector2 pos = touch.screenPosition;
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    m_initialTouchPos = pos;
                    m_isARObjectSelected = CheckTouchOnARObject(m_initialTouchPos);
                    break;

                case TouchPhase.Moved:
                    if (m_isRotationMode)
                        ARRotation(pos);
                    else
                        ARMovement(pos);
                    break;

                case TouchPhase.Ended:
                    break;
            }
        }
    }

    private void ARMovement(Vector2 pos)
    {
        
        if (!m_isARObjectSelected || m_arObject==null)
        {
            return;
        }
        if (m_arInteractionObject.CanMove() && Touch.activeTouches.Count == 1)
        {
            Vector2 diffPos = (pos - m_initialTouchPos) * m_screenfactor;

            // Direcciones de cámara (solo en plano XZ)
            Vector3 camForward = m_arCamera.transform.forward;
            camForward.y = 0;
            camForward.Normalize();

            Vector3 camRight = m_arCamera.transform.right;
            camRight.y = 0;
            camRight.Normalize();

            // Movimiento relativo a la vista (XZ)
            Vector3 moveXZ = (camRight * diffPos.x + camForward * diffPos.y) * m_speedMovement;

            // Movimiento vertical (Y)
            Vector3 moveY = Vector3.up * diffPos.y * m_speedMovement;

            // Elegir el tipo de movimiento según el booleano
            Vector3 move = m_changeAxis ? moveY : moveXZ;

            m_arObject.transform.position += move;

            // Guardamos la nueva posición inicial
            m_initialTouchPos = pos;
        }
    }

    private void ARRotation(Vector2 pos)
    {
        if (!m_isARObjectSelected || m_arObject == null)
            return;

        if (m_arInteractionObject.CanRotate())
        {
            float rotationDelta = (pos.x - m_initialTouchPos.x) * 0.2f; // Puedes ajustar el factor
            m_arObject.transform.Rotate(Vector3.up, rotationDelta, Space.World);
            m_initialTouchPos = pos;
        }
    }


    private bool CheckTouchOnARObject(Vector2 touchPosition)
    {
        Ray ray = m_arCamera.ScreenPointToRay(touchPosition);

        if (Physics.Raycast(ray, out RaycastHit hitARObject))
        {
            // Primero verifica si es un hueso con información
            BoneInfoDisplay infoDisplay = hitARObject.collider.GetComponent<BoneInfoDisplay>();
            if (infoDisplay != null && infoDisplay.IsShowingInfo())
            {
                infoDisplay.HideBoneInfo();
                return false;
            }
            else if (infoDisplay != null)
            {
                infoDisplay.ShowBoneInfo(hitARObject.collider.GetComponent<ARInteractionObject>());
                return false;
            }

            // Lógica original para objetos interactivos
            if (hitARObject.collider.transform.TryGetComponent(out ARInteractionObject _))
            {
                m_arObject = hitARObject.transform.gameObject;
                m_arInteractionObject = hitARObject.transform.gameObject.GetComponent<ARInteractionObject>();
                return true;
            }
        }

        return false;
    }

    public void DeleteModel()
    {
        
        if (m_arInteractionObject==null)
        {
            
            return;
        }
        Debug.Log(m_arInteractionObject.CanDelete());
        if (m_arInteractionObject.CanDelete())
        {
            FindAnyObjectByType<HUDPuzzle>().AddElement(m_arInteractionObject.GetTypeOfBone());
            Destroy(m_arObject);
            m_arObject = null;
            m_arInteractionObject = null;
        }
        
    }

    public void ChangeAxis()
    {
        m_changeAxis = !m_changeAxis;
        UpdateAxisButtonImage(); // Actualiza la imagen al cambiar el eje
    }

    private void UpdateAxisButtonImage()
    {
        if (m_axisToggleButtonImage != null)
        {
            m_axisToggleButtonImage.sprite = m_changeAxis ? m_yAxisSprite : m_xzAxisSprite;
        }
    }
    private void UpdateTransformButtonImage()
    {
        if (transformToggleButtonImage != null)
        {
            transformToggleButtonImage.sprite = m_isRotationMode ?  rotate : transform;
        }
    }

    public void ToggleRotationMode()
    {
        m_isRotationMode = !m_isRotationMode;
        UpdateTransformButtonImage();
    }





}
