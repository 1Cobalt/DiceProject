using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private const string KeyVsAI = "LastGame_VsAI";
    private const string KeyPoints1 = "LastGame_Points1";
    private const string KeyPoints2 = "LastGame_Points2";

    // Сохраняем данные
    public static void SaveGame(bool vsAI, int points1, int points2)
    {
        PlayerPrefs.SetInt(KeyVsAI, vsAI ? 1 : 0);
        PlayerPrefs.SetInt(KeyPoints1, points1);
        PlayerPrefs.SetInt(KeyPoints2, points2);
        PlayerPrefs.Save();
    }

    // Загружаем данные
    public static (bool vsAI, int points1, int points2) LoadGame()
    {
        if (!PlayerPrefs.HasKey(KeyPoints1))
            return (false, 0, 0); // если сохранения нет

        bool vsAI = PlayerPrefs.GetInt(KeyVsAI, 0) == 1;
        int points1 = PlayerPrefs.GetInt(KeyPoints1, 0);
        int points2 = PlayerPrefs.GetInt(KeyPoints2, 0);

        return (vsAI, points1, points2);
    }

    // Очистить сохранение (по желанию)
    public static void ClearSave()
    {
        PlayerPrefs.DeleteKey(KeyVsAI);
        PlayerPrefs.DeleteKey(KeyPoints1);
        PlayerPrefs.DeleteKey(KeyPoints2);
    }
}
