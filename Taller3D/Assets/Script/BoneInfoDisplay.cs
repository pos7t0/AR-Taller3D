using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoneInfoDisplay : MonoBehaviour
{
    [SerializeField] private GameObject m_infoPanel; // Panel UI que muestra la información
    [SerializeField] private TMP_Text m_boneNameText;    // Texto para el nombre del hueso
    [SerializeField] private TMP_Text m_boneInfoText;    // Texto para la información del hueso
    [SerializeField] private string m_csvFileName = "bone_info"; // Nombre del archivo CSV (sin extensión)
    [SerializeField] private Button m_closeButton; // Referencia al botón

    [System.Serializable]
    public class BoneInfo
    {
        public TypeOfBone boneType;
        public string boneName;
        [TextArea(3, 10)]
        public string boneDescription;
    }

    [SerializeField] private BoneInfo[] m_boneInfoList; // Lista de información para cada hueso

    private ARInteractionObject m_currentBone;

    private void Start()
    {
        m_infoPanel.SetActive(false);
        LoadBoneInfoFromCSV();
        // Configura el evento del botón
        if (m_closeButton != null)
        {
            m_closeButton.onClick.AddListener(HideBoneInfo);
        }
        else
        {
            Debug.LogError("CloseButton no asignado en BoneInfoDisplay");
        }
    }

    private void LoadBoneInfoFromCSV()
    {
        if (!string.IsNullOrEmpty(m_csvFileName))
        {
            CSVReader.LoadBoneInfo(m_csvFileName, m_boneInfoList);
        }
    }

    public void ShowBoneInfo(ARInteractionObject bone)
    {
        m_currentBone = bone;
        TypeOfBone boneType = bone.GetTypeOfBone();

        foreach (var info in m_boneInfoList)
        {
            if (info.boneType == boneType)
            {
                m_boneNameText.text = info.boneName;
                m_boneInfoText.text = info.boneDescription;
                m_infoPanel.SetActive(true);
                return;
            }
        }
    }

    public void HideBoneInfo()
    {
        m_infoPanel.SetActive(false);
    }

    public bool IsShowingInfo()
    {
        return m_infoPanel.activeSelf;
    }
}