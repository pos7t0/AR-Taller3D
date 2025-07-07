using UnityEngine;
using UnityEngine.UI;

public class PuzzleSystem : MonoBehaviour
{
    [SerializeField]private PuzzlePiece[] m_pieces;
    [SerializeField] private Slider m_sliderProgress;



    private void Start()
    {
        ShowInFrontOfCamera(2);
        m_sliderProgress.value = 0;
        m_sliderProgress.maxValue = m_pieces.Length;
        ChangeSlider();
    }

    private void ChangeSlider()
    {
        if (m_sliderProgress.value <= 0)
        {
            m_sliderProgress.fillRect.GetComponent<CanvasRenderer>().SetAlpha(0);

        }
        else
        {
            m_sliderProgress.fillRect.GetComponent<CanvasRenderer>().SetAlpha(1);
        }
    }
    public void CompletePuzzle()
    {
        m_sliderProgress.value +=1;
        ChangeSlider();
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


    public void ShowInFrontOfCamera(float distance = 1.5f)
    {
        Camera cam = Camera.main;
        if (cam != null)
        {
            // Calcula una posición frente a la cámara
            Vector3 forward = cam.transform.forward;
            forward.y = 0; // Elimina la inclinación vertical
            forward.Normalize();

            Vector3 positionInFront = cam.transform.position + forward * distance;
            transform.position = positionInFront;

            // Gira solo en el eje Y para mirar hacia la cámara
            Vector3 directionToCamera = cam.transform.position - transform.position;
            directionToCamera.y = 0; // Solo rotación en Y
            transform.rotation = Quaternion.LookRotation(directionToCamera);
        }
        else
        {
            Debug.LogWarning("No se encontró la cámara principal.");
        }
    }


}
