using UnityEngine;
using UnityEngine.Events;
public class PuzzlePiece : MonoBehaviour
{
    [SerializeField] private TypeOfBone m_typeOfBone;
    [SerializeField] private Material m_changeMaterial;
    [SerializeField] private Material m_originalMaterial;
    [SerializeField] private Material m_errorMaterial;
    
    private GameObject m_piece;
    private bool m_inPlace=false;
    private float m_marginError=5f;

    private void Update()
    {
        VerifyPiece();
    }
    public bool IsInPlace()
    {
        return m_inPlace;
    }

    private void ChangeMaterial(Material m)
    {
        GetComponent<MeshRenderer>().material=m;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ARInteractionObject _))
        {
            Debug.Log("pieza encontrada");
            m_piece =other.gameObject;
        }
    }

    private void VerifyPiece()
    {
        if (m_piece==null)
        {
            if(!IsInPlace())
            ChangeMaterial(m_originalMaterial);
            return;
        }
        
        bool samePiece = m_piece.GetComponent<ARInteractionObject>().GetTypeOfBone() == m_typeOfBone;
        if (!samePiece)
        {
            return;
        }
        if (RotationCheck())
        {
            m_inPlace = true;
            ChangeMaterial(m_changeMaterial);
            GetComponentInParent<PuzzleSystem>().CompletePuzzle();
            Destroy(m_piece);
        }
        else 
        {
            ChangeMaterial(m_errorMaterial);
        }
       
    }

    private bool RotationCheck()
    {
        float rotY_A = gameObject.transform.parent.gameObject.transform.eulerAngles.y;
        float rotY_B = m_piece.transform.eulerAngles.y;

        float difference = Mathf.DeltaAngle(rotY_A, rotY_B); // Maneja correctamente los 0°-360°

        

        return Mathf.Abs(difference) <= m_marginError;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ARInteractionObject piece))
        {
            m_piece = null;
        }
    }

}
