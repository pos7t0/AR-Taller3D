using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;


public class ObjectManipulation : MonoBehaviour
{
    public GameObject m_arObject;
    private ARInteractionObject m_arInteractionObject;

    [SerializeField] private Camera m_arCamera;

    private bool m_isARObjectSelected;
    private Vector2 m_initialTouchPos;

    [SerializeField] private float m_speedMovement = 4.0f;

    [SerializeField] private float m_screenfactor;

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

                    ARMovement(pos);
                    break;

                case TouchPhase.Ended:
                    
                    //Debug.Log($"[New Input] Touch ended at: {pos}, fingerId: {touch.finger.index}");
                    break;
            }
        }

        //  ROTACIÓN (fuera del foreach)
        ARRotation();
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

            // Movimiento relativo a la vista
            Vector3 move = (camRight * diffPos.x + camForward * diffPos.y) * m_speedMovement;

            m_arObject.transform.position += move;

            //Debug.Log($"Moviendo según cámara: {move}");
            m_initialTouchPos = pos;
        }
    }

    private void ARRotation()
    {
        if (!m_isARObjectSelected || m_arObject == null)
        {
            return;
        }
        if (Touch.activeTouches.Count == 2 && m_arInteractionObject.CanRotate())
        {

            var touch0 = Touch.activeTouches[0];
            var touch1 = Touch.activeTouches[1];

            Vector2 prevTouch0 = touch0.screenPosition - touch0.delta;
            Vector2 prevTouch1 = touch1.screenPosition - touch1.delta;

            float prevAngle = Vector2.SignedAngle(prevTouch1 - prevTouch0, Vector2.right);
            float currAngle = Vector2.SignedAngle(touch1.screenPosition - touch0.screenPosition, Vector2.right);
            float angleDelta = currAngle - prevAngle;

            if (m_arObject != null)
            {
                m_arObject.transform.Rotate(Vector3.up, angleDelta, Space.World);
                Debug.Log($"Rotando: {angleDelta}°");
            }
        }
    }

    
    private bool CheckTouchOnARObject(Vector2 touchPosition)
    {
        Ray ray = m_arCamera.ScreenPointToRay(touchPosition);

        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 2f);

        if (Physics.Raycast(ray, out RaycastHit hitARObject))
        {
            
            if (hitARObject.collider.transform.TryGetComponent(out ARInteractionObject _))
            {
                
                m_arObject = hitARObject.transform.gameObject;
                m_arInteractionObject = hitARObject.transform.gameObject.GetComponent<ARInteractionObject>();
                return true;
            }
            Debug.Log(hitARObject.collider.gameObject.name);

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






}
