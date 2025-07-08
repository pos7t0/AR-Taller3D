using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class KillMe : MonoBehaviour
{
    private DateTime expirationDateUtc = new DateTime(2025, 9, 6, 0, 0, 0, DateTimeKind.Utc); // Expira el 6 de septiembre de 2025 (UTC)
    private const string installDateKey = "InstallDate";
    private int fallbackDays = 30;

    void Start()
    {
        CheckHardwareTime();
        CheckLocalInstallDate();
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
                DateTime serverUtcDate = DateTime.Parse(dateHeader).ToUniversalTime();
                Debug.Log("Fecha UTC del servidor: " + serverUtcDate);

                if (serverUtcDate > expirationDateUtc)
                {
                    KillApp("Expiró por fecha del servidor.");
                }
                else
                {
                    Debug.Log("Aplicación válida según la fecha del servidor.");
                }

                yield break;
            }
        }

        Debug.LogWarning("No se pudo obtener la fecha del servidor.");
    }

    void CheckLocalInstallDate()
    {
        DateTime installDate;

        if (!PlayerPrefs.HasKey(installDateKey))
        {
            installDate = DateTime.UtcNow;
            PlayerPrefs.SetString(installDateKey, installDate.ToString("O"));
            PlayerPrefs.Save();
            Debug.Log("Guardando fecha de instalación local: " + installDate);
        }
        else
        {
            installDate = DateTime.Parse(PlayerPrefs.GetString(installDateKey));
            Debug.Log("Fecha de instalación recuperada: " + installDate);
        }

        TimeSpan elapsed = DateTime.UtcNow - installDate;

        if (elapsed.TotalDays > fallbackDays)
        {
            KillApp("Expiró por fecha de instalación local.");
        }
        else
        {
            Debug.Log("Aplicación válida según fallback local.");
        }
    }

    void CheckHardwareTime()
    {
        DateTime nowUtc = DateTime.UtcNow;

        if (nowUtc > expirationDateUtc)
        {
            KillApp("Expiró por fecha del sistema.");
        }
        else
        {
            Debug.Log("Aplicación válida según la hora del sistema.");
        }
    }

    void KillApp(string reason)
    {
        Debug.LogWarning(reason);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_ANDROID
        try
        {
            AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer")
                .GetStatic<AndroidJavaObject>("currentActivity");
            activity.Call<bool>("moveTaskToBack", true); // Mueve la app al fondo
        }
        catch (Exception e)
        {
            Debug.LogError("Error al cerrar app en Android: " + e.Message);
        }
#else
        Application.Quit();
#endif
    }
}