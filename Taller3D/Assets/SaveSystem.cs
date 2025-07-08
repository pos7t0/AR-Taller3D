using UnityEngine;
using UnityEngine.UI;

public class SaveSystem : MonoBehaviour
{
    [Header("Checkmarks de Niveles")]
    [SerializeField] private GameObject m_checkLvl1;
    [SerializeField] private GameObject m_checkLvl2;
    [SerializeField] private GameObject m_checkLvl3;

    [Header("Botón de Desbloqueo")]
    [SerializeField] private GameObject m_specialButton; // Botón que se habilitará
    [SerializeField] private GameObject m_lockIcon;  // Icono de candado opcional


    [Header("Debug")]
    [SerializeField] private bool m_debugUnlockAll;
    [SerializeField] private bool m_debuglockAll;


    void Start()
    {
       

        if (m_debugUnlockAll)
        {
            PlayerPrefs.SetInt("lvl1_completado", 1);
            PlayerPrefs.SetInt("lvl2_completado", 1);
            PlayerPrefs.SetInt("lvl3_completado", 1);
            PlayerPrefs.Save();
        }
        if (m_debuglockAll)
        {
            PlayerPrefs.SetInt("lvl1_completado", 0);
            PlayerPrefs.SetInt("lvl2_completado", 0);
            PlayerPrefs.SetInt("lvl3_completado", 0);
            PlayerPrefs.Save();
        }

        // Verificar niveles completados
        bool lvl1Complete = PlayerPrefs.GetInt("lvl1_completado", 0) == 1;
        bool lvl2Complete = PlayerPrefs.GetInt("lvl2_completado", 0) == 1;
        bool lvl3Complete = PlayerPrefs.GetInt("lvl3_completado", 0) == 1;
        // Mostrar checkmarks
        m_checkLvl1.SetActive(lvl1Complete);
        m_checkLvl2.SetActive(lvl2Complete);
        m_checkLvl3.SetActive(lvl3Complete);

        // Habilitar botón especial si todos están completados
        if (m_specialButton != null)
        {
            bool allLevelsComplete = lvl1Complete || lvl2Complete || lvl3Complete;
            m_specialButton.SetActive(allLevelsComplete);

            // Opcional: Ocultar icono de candado
            if (m_lockIcon != null)
            {
                m_lockIcon.SetActive(!allLevelsComplete);
            }
        }

    }

    public void DebugUnlockAllLevels()
    {
        PlayerPrefs.SetInt("lvl1_completado", 1);
        PlayerPrefs.SetInt("lvl2_completado", 1);
        PlayerPrefs.SetInt("lvl3_completado", 1);
        PlayerPrefs.Save();

        // Actualiza visualmente
        m_checkLvl1.SetActive(true);
        m_checkLvl2.SetActive(true);
        m_checkLvl3.SetActive(true);
        m_specialButton.SetActive(true);

        Debug.Log("¡Todos los niveles desbloqueados en debug!");
    }
}