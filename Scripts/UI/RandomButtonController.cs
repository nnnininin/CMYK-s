using Player.ScriptableObject;
using TMPro;
using UI.ActionScene;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    //ランダムに選ばれたカードがセットされるボタン
    public class RandomButtonController : ButtonController
    {
        [SerializeField]
        private CardSelector cardSelector;

        [SerializeField]
        private TextMeshProUGUI buttonName;
        
        [SerializeField]
        private TextMeshProUGUI buttonDescription;
        
        private string descriptionData;
        
        private new void Awake()
        {
            cardSelector.OnAssetLoaded.Subscribe(SetSelectableData).AddTo(this);
        }

        //選ばれたカードの情報をボタンにセット
        private void SetSelectableData(CardSO cardSO)
        {
            //button名をセット
            var displayName = cardSO.DisplayName;
            Debug.Log($"RandomButtonController.SetSelectableData: {displayName}");
            if (displayName == null) Debug.LogError("SelectableSO.DisplayName is null");
            // \nを改行に変換
            if (displayName != null && displayName.Contains("\\n"))
            {
                displayName = displayName.Replace("\\n", "\n");
            }
            buttonName.text = displayName;

            //button説明をセット
            var description = cardSO.Description;
            if (description == null) Debug.LogError("SelectableSO.Description is null");
            // \nを改行に変換
            if (description != null && description.Contains("\\n"))
            {
                description = description.Replace("\\n", "\n");
            }
            descriptionData = description;
            buttonDescription.text = "";
            
            //スプライトをセット
            var sprite = cardSO.ButtonSprite;
            if (sprite == null) Debug.LogError("SelectableSO.ButtonSprite is null");
            GetComponent<Image>().sprite = sprite;
            
            base.Awake();
        }
        
        //ボタンにマウスオーバーしたときの処理
        protected override void OverrideOnButtonEnter()
        {
            //説明を表示
            buttonDescription.text = descriptionData;
        }
        
        //ボタンにマウスオーバーがなくなったときの処理
        protected override void OverrideOnButtonExit()
        {
            //説明を消す
            buttonDescription.text = "";
        }
    }
}