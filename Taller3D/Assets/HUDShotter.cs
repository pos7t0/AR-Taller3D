using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class HUDShotter : MonoBehaviour
{
    [SerializeField] private TMP_Text m_weakText;
    [SerializeField] private Slider m_healthVar;
    [SerializeField] private TMP_Text m_debugger;

    #region Nombre de huesos
    private Dictionary<TypeOfBone, string> boneNames = new Dictionary<TypeOfBone, string>()
    {
        // Zona superior
        { TypeOfBone.Craneo, "Cráneo" },
        { TypeOfBone.Mandibula, "Mandíbula" },
    
        // Zona media
        { TypeOfBone.Columna, "Columna" },
        { TypeOfBone.Costillas, "Costillas" },
        { TypeOfBone.Humero_derecho, "Húmero derecho" },
        { TypeOfBone.Humero_izquierdo, "Húmero izquierdo" },
        { TypeOfBone.Mano_derecha, "Mano derecha" },
        { TypeOfBone.Mano_izquierda, "Mano izquierda" },
        { TypeOfBone.Omoplato_derecho, "Omóplato derecho" },
        { TypeOfBone.Omoplato_izquierdo, "Omóplato izquierdo" },
        { TypeOfBone.Radio_cubito_derecho, "Radio y cúbito derechos" },
        { TypeOfBone.Radio_cubito_izquierdo, "Radio y cúbito izquierdos" },
    
        // Zona baja
        { TypeOfBone.Femur_derecho, "Fémur derecho" },
        { TypeOfBone.Femur_izquierdo, "Fémur izquierdo" },
        { TypeOfBone.Pelvis, "Pelvis" },
        { TypeOfBone.Perone_tibia_derecha, "Peroné y tibia derechos" },
        { TypeOfBone.perone_tibia_izquierdo, "Peroné y tibia izquierdos" },
        { TypeOfBone.pie_derecho, "Pie derecho" },
        { TypeOfBone.pie_izquierdo, "Pie izquierdo" }
    };
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void ShowWeakText(TypeOfBone weakBone)
    {
        
        if (boneNames.TryGetValue(weakBone, out string boneName))
        {
            m_weakText.text = boneName;
        }
    }
    public void SetSlider(float health)
    {
        m_healthVar.maxValue =health;
        m_healthVar.value = health;
    }
    public void ChangeSlider(float health)
    {
        m_healthVar.value=health;
        if (m_healthVar.value<=0)
        {
            m_healthVar.fillRect.GetComponent<CanvasRenderer>().SetAlpha(0);

        }
    }

    public void Debug(string a)
    {
        m_debugger.text =a;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
