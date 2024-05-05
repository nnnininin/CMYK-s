using System.Collections.Generic;
using UnityEngine;

namespace Enemy.State
{
    public class StateContext
    {
        private IState.IState CurrentState { get; set; } // 現在の状態
        public IState.IState PreviousState { get; private set; } // 直前の状態

        // 状態のテーブル
        private readonly Dictionary<IState.State, IState.IState> _stateTable;
        
        //ステートマシンを初期化
        public StateContext()
        {
            // 状態テーブルを初期化
            Dictionary<IState.State, IState.IState> table = new();
            _stateTable = table;
        }

        //状態を追加する
        public void AddState(IState.State state, IState.IState stateClass)
        {
            if (_stateTable.ContainsKey(state))
                return;
            _stateTable.Add(state, stateClass);
            Debug.Log("State added: " + state);
        }

        // 別の状態に変更する
        public void ChangeState(IState.State next)
        {
            if (_stateTable == null)
            {
                return;
            }
            if (CurrentState == null)
            {
                CurrentState = _stateTable[next];
                PreviousState = CurrentState;
                CurrentState.Entry();
            }
            if (CurrentState.State == next)
            {
               return;
            }
            // 退場 → 現在状態変更 → 入場
            var nextState = _stateTable[next];
            PreviousState = CurrentState;
            PreviousState?.Exit();
            CurrentState = nextState;
            Debug.Log("CurrentState.State: " + CurrentState.State);
            CurrentState.Entry();
        }

        // 現在の状態を更新する
        public void Update() => CurrentState.Update();

        // 現在の状態を物理演算のフレームごとに更新する
        public void FixedUpdate()
        {
            CurrentState.FixedUpdate();
        }
    }
}
