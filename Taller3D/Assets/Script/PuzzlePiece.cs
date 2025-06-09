using UnityEngine;
using UnityEngine.Events;
public class PuzzlePiece : MonoBehaviour
{
    [SerializeField] private GameObject m_piece;
    [SerializeField] private Material m_changeMaterial;
    private bool m_inPlace=false;
    
    public bool IsInPlace()
    {
        return m_inPlace;
    }

    private void ChangeMaterial()
    {
        GetComponent<MeshRenderer>().material=m_changeMaterial;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other!=null&& m_piece != null)
        {
            Debug.Log("no esta");
            if (other.gameObject.name == m_piece.name)
            {
                m_inPlace = true;
                ChangeMaterial();
                GetComponentInParent<PuzzleSystem>().CompletePuzzle();
                Destroy(m_piece);
            }
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("choco con alguna pieza");
    }

}
