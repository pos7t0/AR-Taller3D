using UnityEngine;
using System.IO;
using System.Linq;

public static class CSVReader
{
    public static void LoadBoneInfo(string csvFileName, BoneInfoDisplay.BoneInfo[] boneInfoList)
    {
        TextAsset csvData = Resources.Load<TextAsset>(csvFileName);
        if (csvData == null)
        {
            Debug.LogError("No se encontró el archivo CSV: " + csvFileName);
            return;
        }

        string[] lines = csvData.text.Split('\n');

        // Saltar la primera línea (encabezados)
        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] values = SplitCSVLine(lines[i]);

            if (values.Length >= 3)
            {
                string boneName = values[0].Trim();
                string info = values[1].Trim();
                string funFact = values[2].Trim();

                // Buscar el tipo de hueso correspondiente
                foreach (var boneInfo in boneInfoList)
                {
                    if (boneInfo.boneType.ToString().ToLower() == boneName.ToLower())
                    {
                        boneInfo.boneName = boneName;
                        boneInfo.boneDescription = $"{info}\n\nDato curioso: {funFact}";
                        break;
                    }
                }
            }
        }
    }

    private static string[] SplitCSVLine(string line)
    {
        // Maneja campos entre comillas que contienen comas
        return line.Split(';').Select(s => s.Trim('"')).ToArray();
    }
}