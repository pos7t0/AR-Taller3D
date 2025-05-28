using UnityEngine;
using UnityEngine.UI;

public class PuzzleSystem : MonoBehaviour
{
    [SerializeField]private PuzzlePiece[] m_pieces;
    [SerializeField] private Slider m_sliderProgress;

    private void Start()
    {
        m_sliderProgress.value = 0;
        m_sliderProgress.maxValue = m_pieces.Length;
    }
    public void CompletePuzzle()
    {
        m_sliderProgress.value +=1;
        if (PuzzleIsReady())
        {

        }
    }

    private bool PuzzleIsReady()
    {
        foreach (PuzzlePiece a in m_pieces)
        {
            if (!a.IsInPlace())
            {
                return false;
            }
        }
        return true;
    }


}
