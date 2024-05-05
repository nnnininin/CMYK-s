using Scene;
using UnityEngine;

namespace Player.ScriptableObject
{
    public abstract class CardSO : UnityEngine.ScriptableObject, IDataApplier
    {
        [SerializeField]
        private string displayName;
        public string DisplayName => displayName;
        
        [SerializeField][Multiline]
        private string description;
        public string Description => description;
        
        [SerializeField]
        private Sprite buttonSprite;
        public Sprite ButtonSprite => buttonSprite;
        
        public abstract void ApplyToGameData(TemporaryGameDataSo temporaryGameData);
    }
}