using System;
using Player.ScriptableObject;
using Scene;
using UniRx;
using UnityEngine;

namespace UI.ActionScene
{
    public class CardSelector : MonoBehaviour
    {
        [SerializeField] private TemporaryGameDataSo temporaryGameData;
        //選択可能なカードのリスト
        [SerializeField] private SelectableSOs selectableSOs;
        
        //他のカード
        [SerializeField] private CardSelector[] otherCardSelectors;
        
        private CardSO selectedCardSO;
        
        public Subject<CardSO> OnAssetLoaded { get; } = new();
        
        public Subject<Unit> OnCardClicked { get; } = new();

        private void Awake()
        {
            SelectCard();
            foreach (var otherCardSelector in otherCardSelectors)
            {
                otherCardSelector.OnCardClicked.Subscribe(_ => SelectCard()).AddTo(this);
            }
        }
        
        //クリックされると実行
        public void OnClickLoad()
        {
            if (selectedCardSO is IDataApplier dataApplier)
            {
                dataApplier.ApplyToGameData(temporaryGameData);
            }
            OnCardClicked.OnNext(Unit.Default);
            
            //再び選択可能なカードのリストからランダムに選ぶ
            SelectCard();
        }
        
        private static CardSO GetRandomSelectedSO(SelectableSOs selectableSOsValue)
        {
            Debug.Log("CardSelector.GetRandomSelectedSO");
            if (selectableSOsValue == null || selectableSOsValue.SelectableSOList.Count == 0)
            {
                throw new InvalidOperationException("Selectable list is empty or null.");
            }
            var index = UnityEngine.Random.Range(0, selectableSOsValue.SelectableSOList.Count);
            return selectableSOsValue.SelectableSOList[index];
        }
        
        private void SelectCard()
        {
            //選択可能なカードのリストからランダムに選ぶ
            selectedCardSO = GetRandomSelectedSO(selectableSOs);
            //選ばれたカードの情報をランダムボタンコントローラーに送る
            OnAssetLoaded.OnNext(selectedCardSO);
        }
    }
}