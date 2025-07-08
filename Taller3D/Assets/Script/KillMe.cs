using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class KillMe : MonoBehaviour
{
    private DateTime expirationDate = new DateTime(2025, 9, 6); // Fecha limite
    private const string installDateKey = "InstallDate";
    private int fallbackDays = 30;

    void Start()
    {
        CheckTimeFromHardware(); 
        CheckFallbackLocalDate();
        StartCoroutine(CheckDateFromWeb());
    }

    IEnumerator CheckDateFromWeb()
    {
        UnityWebRequest request = UnityWebRequest.Head("https://www.google.cl");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string dateHeader = request.GetResponseHeader("date");

            if (!string.IsNullOrEmpty(dateHeader))
            {
                // Convertir la cabecera del servidor a DateTime UTC
                DateTime serverUtcDate = DateTime.Parse(dateHeader).ToUniversalTime();
                Debug.Log("Fecha UTC del servidor: " + serverUtcDate);

                // Convertir a hora de Chile
                TimeZoneInfo chileTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific SA Standard Time");
                DateTime chileDate = TimeZoneInfo.ConvertTimeFromUtc(serverUtcDate, chileTimeZone);
                Debug.Log("Fecha del servidor en hora de Chile: " + chileDate);

                // Fecha l�mite (hora de Chile)
                DateTime expirationDateChile = new DateTime(2025, 7, 6, 0, 0, 0);

                if (chileDate > expirationDateChile)
                {
                    KillApp("Expir� por hora del servidor en Chile.");
                }
                else
                {
                    Debug.Log("Aplicaci�n v�lida seg�n hora de Chile.");
                }

                yield break;
            }
        }

        Debug.LogWarning("Fallo al obtener la fecha del servidor.");
    }

    void CheckFallbackLocalDate()
    {
        DateTime installDate;

        if (!PlayerPrefs.HasKey(installDateKey))
        {
            installDate = DateTime.UtcNow;
            PlayerPrefs.SetString(installDateKey, installDate.ToString("O"));
            PlayerPrefs.Save();
            Debug.Log("Guardando fecha de instalaci�n: " + installDate);
        }
        else
        {
            installDate = DateTime.Parse(PlayerPrefs.GetString(installDateKey));
            Debug.Log("Fecha de instalaci�n recuperada: " + installDate);
        }

        TimeSpan elapsed = DateTime.UtcNow - installDate;

        if (elapsed.TotalDays > fallbackDays)
        {
            KillApp("Expir� por fecha de instalaci�n local.");
        }
        else
        {
            Debug.Log("Aplicaci�n v�lida seg�n fallback local.");
        }
    }

    void CheckTimeFromHardware()
    {
        DateTime now = DateTime.Now;
        if (now > expirationDate)
        {
            KillApp("Expir� por fecha del sistema.");
        }
        else
        {
            Debug.Log("Aplicaci�n v�lida seg�n hora del sistema.");
        }
    }

    void KillApp(string reason)
    {
        Debug.LogWarning(reason);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
