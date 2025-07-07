using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    [SerializeField] private GameObject m_checkLvl1;
    [SerializeField] private GameObject m_checkLvl2;
    [SerializeField] private GameObject m_checkLvl3;

    void Start()
    {
        m_checkLvl1.SetActive(PlayerPrefs.GetInt("lvl1_completado", 0) == 1);
        m_checkLvl2.SetActive(PlayerPrefs.GetInt("lvl2_completado", 0) == 1);
        m_checkLvl3.SetActive(PlayerPrefs.GetInt("lvl3_completado", 0) == 1);
    }

}
