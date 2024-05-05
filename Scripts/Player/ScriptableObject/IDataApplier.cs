using Scene;

namespace Player.ScriptableObject
{
    public interface IDataApplier
    {
        //temporaryGameDataにデータを適用する処理
        void ApplyToGameData(TemporaryGameDataSo temporaryGameData);
    }
}