using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class ARFloorDetector : MonoBehaviour
{
    [Header("Referencias")]
    public ARRaycastManager m_raycastManager;
    public GameObject m_objectPrefab;

    //no se si es necesario xd
    private GameObject objetoInstanciado;
    private bool m_floorDetected = false;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    // Guardamos el plano detectado
    private ARPlane m_planeDetected;

    void Update()
    {
        foreach (Touch touch in Touch.activeTouches)
        {
            Vector2 pos = touch.screenPosition;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // Solo en primer toque y si aún no se ha instanciado
                    if (!m_floorDetected && m_raycastManager.Raycast(pos, hits, TrackableType.PlaneWithinPolygon))
                    {
                        Pose pose = hits[0].pose;

                        // Instanciar objeto
                        objetoInstanciado = Instantiate(m_objectPrefab, pose.position, pose.rotation);

                        // Guardar el plano AR donde se colocó el objeto
                        m_planeDetected = hits[0].trackable as ARPlane;

                        m_floorDetected = true;
                        Debug.Log("Piso detectado y objeto colocado.");
                    }
                    break;

                case TouchPhase.Moved:
                    break;

                case TouchPhase.Ended:
                    break;
            }
        }
    }

    // Permitir que otros scripts accedan al plano
    public ARPlane GetPlaneDetected()
    {
        return m_planeDetected;
    }
}