using System;
using UnityEngine;

namespace Scene
{
    public class SaveLoadManager : MonoBehaviour
    {
        [SerializeField]
        private TemporaryGameDataSo temporaryGameData;

        private const string SaveDataKey = "saveData";

        //temporarySaveDataの内容をES3に保存
        //temporarySaveDataにデータをセットしゲーム内で使用している
        
        public void SaveGameData()
        {
            try
            {
                // SaveDataKeyにtemporarySaveDataを保存
                ES3.Save(SaveDataKey, temporaryGameData);
                Debug.Log("Game data saved successfully.");
            }
            catch (Exception e)
            {
                // 保存中にエラーが発生した場合
                Debug.LogError($"Failed to save game data: {e.Message}");
            }
        }

        public bool LoadGameData()
        {
            try
            {
                // SaveDataKeyに保存されているデータをtemporarySaveDataにロード
                if (ES3.KeyExists(SaveDataKey))
                {
                    temporaryGameData = ES3.Load<TemporaryGameDataSo>(SaveDataKey);
                    Debug.Log("Game data loaded successfully.");
                    return true;
                }
                Debug.LogWarning($"No save data found at key {SaveDataKey}.");
                return false;
            }
            catch (Exception e)
            {
                // ロード中にエラーが発生した場合
                Debug.LogError($"Failed to load game data: {e.Message}");
                return false;
            }
        }
    }
}