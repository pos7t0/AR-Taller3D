using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;


public class ObjectManipulation : MonoBehaviour
{
    public GameObject m_arObject;

    [SerializeField] private Camera m_arCamera;

    private bool m_isARObjectSelected;
    private string m_tagARObjects ="ARObject";
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
                    if (!m_isARObjectSelected)
                    {
                        Debug.Log("No se seleccionó un objeto AR");
                    }
                    break;
                case TouchPhase.Moved:
                    if (m_isARObjectSelected)
                    {
                        Vector2 diffPos = (pos - m_initialTouchPos) * m_screenfactor;
                        m_arObject.transform.position = m_arObject.transform.position +
                            new Vector3(diffPos.x * m_speedMovement, diffPos.y * m_speedMovement, 0);
                        m_initialTouchPos = pos;
                    }
                    else
                    {
                        Debug.Log("falso");
                    }
                        break;
                case TouchPhase.Ended:
                    Debug.Log($"[New Input] Touch ended at: {pos}, fingerId: {touch.finger.index}");
                    break;
            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Update()
    //{
    //    if (Input.touchCount>0)
    //    {
    //        UnityEngine.Touch touchOne = Input.GetTouch(0);
    //        if (Input.touchCount == 1)
    //        {


    //            if (touchOne.phase == TouchPhase.Began)
    //            {
    //                m_initialTouchPos = touchOne.position;
    //                m_isARObjectSelected = CheckTouchOnARObject(m_initialTouchPos);
    //            }

    //            if (touchOne.phase == TouchPhase.Moved && m_isARObjectSelected)
    //            {
    //                Vector2 diffPos = (touchOne.position - m_initialTouchPos) * m_screenfactor;
    //                m_arObject.transform.position = m_arObject.transform.position +
    //                    new Vector3(diffPos.x * m_speedMovement, diffPos.y * m_speedMovement, 0);
    //                m_initialTouchPos = touchOne.position;
    //            }

    //        }
    //    }
    //}


    private bool CheckTouchOnARObject(Vector2 touchPosition)
    {
        Ray ray = m_arCamera.ScreenPointToRay(touchPosition);

        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 2f);

        if (Physics.Raycast(ray, out RaycastHit hitARObject))
        {
            Debug.Log("choco con algo");
            if (hitARObject.collider.CompareTag(m_tagARObjects))
            {
                Debug.Log("choco con el cubo");
                m_arObject = hitARObject.transform.gameObject;

                return true;
            }
            Debug.Log(hitARObject.collider.gameObject.name);

        }
        Debug.Log("pal lobby");

        return false;
    }
}
