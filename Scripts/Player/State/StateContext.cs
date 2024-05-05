using System.Collections.Generic;
using Player.State.IState.BaseState.UtilState;
using UnityEngine;

namespace Player.State
{
    public class StateContext
    {
        public IState.IState CurrentState { get; private set; } // 現在の状態
        private IState.IState PreviousState { get; set; } //直前の状態

        // 状態のテーブル
        private Dictionary<IState.State, IState.IState> _stateTable;

        public void Init(IPlayer player, IState.State initState)
        {
            Debug.Log("Init called");
            if (_stateTable != null)
                return;

            // 各状態クラスの初期化（BaseState を継承したクラスを使用）
            _stateTable = new Dictionary<IState.State, IState.IState>
            {
                { IState.State.Idle, new IdleState(player) },
                { IState.State.Shot, new ShotState(player) },
                { IState.State.Reload, new ReloadState(player) },
                { IState.State.Skill, new SkillState(player) }
            };

            ChangeState(initState);
        }

        // 別の状態に変更する
        public void ChangeState(IState.State next)
        {
            //Debug.Log("ChangeState called: " + next.ToString());
            if (_stateTable == null)
            {
                Debug.Log("StateTable is null");
                return;
            }
            if (CurrentState == null)
            {
                Debug.Log("CurrentState is null");
                CurrentState = _stateTable[next];
                CurrentState.Entry();
            }
            if (CurrentState.State == next)
            {
                Debug.Log("Already in the same state: " + next);
                return;
            }
            Debug.Log("ChangeState called: " + next.ToString());
            // 退場 → 現在状態変更 → 入場
            var nextState = _stateTable[next];
            PreviousState = CurrentState;
            PreviousState?.Exit();
            CurrentState = nextState;
            CurrentState.Entry();
        }

        // 現在の状態をUpdateする
        public void Update() => CurrentState?.Update();

        public void FixedUpdate() => CurrentState?.FixedUpdate();
    }
}
