using System.Collections.Generic;
using Player.ScriptableObject;
using UnityEngine;

namespace UI.ActionScene
{
    [CreateAssetMenu(fileName = "SelectableCards", menuName = "ScriptableObjects/SelectableCards")]
    public class SelectableSOs : ScriptableObject
    {
        [SerializeField] private List<CardSO> selectableSO;
        public List<CardSO> SelectableSOList => selectableSO;
    }
}