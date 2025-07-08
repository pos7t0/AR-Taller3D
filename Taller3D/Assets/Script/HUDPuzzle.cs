using UnityEngine;
using UnityEngine.UI;
public class HUDPuzzle : MonoBehaviour
{


    
    [SerializeField] private Animator m_piecesPanel;

    [SerializeField] private SpawnObject m_spawnObject;
    [SerializeField] private GameObject[] m_piecesToPut;
    [SerializeField] private Transform m_contentTransform;
    [SerializeField] private GameObject m_winPanel;

    public void AddElement(TypeOfBone typeOfBone)
    {
        foreach (GameObject a in m_piecesToPut)
        {
            Debug.Log("");
            if (a.GetComponent<TypeOfPiece>().GetTypeBone()==typeOfBone)
            {
            
                a.SetActive(true);
                return;
            }
            
        }
    }

    
    public void HideMenu()
    {
        m_piecesPanel.SetTrigger("Hide");
    }
    public void ShowMenu()
    {
        m_piecesPanel.SetTrigger("Show");
    }

    public void HideButton(GameObject button)
    {
        button.SetActive(false);
        m_piecesPanel.SetTrigger("Hide");
    }




}
