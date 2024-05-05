using Scene;
using UnityEngine;

namespace Player.ScriptableObject
{
    [CreateAssetMenu(fileName = "CharaSO", menuName = "ScriptableObjects/CharaSO")]
    public class CharaSO : CardSO
    {
        [SerializeField]
        private string charaName;
        public string CharaName => charaName;
        [SerializeField]
        private string charaDescription;
        public string CharaDescription => charaDescription;
        [SerializeField]
        private int hitPoint;
        public int HitPoint => hitPoint;
        [SerializeField]
        private int moveSpeed;
        public int MoveSpeed => moveSpeed;
        
        public override void ApplyToGameData(TemporaryGameDataSo temporaryGameData)
        {
            temporaryGameData.SetCharaSO(this);
        }
    }
}