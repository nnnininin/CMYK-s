using System;
using UniRx;
using UnityEngine;

namespace Player.Util
{
    public class EventManager
    {
        // イベントの定義
        private readonly Subject<Unit> _onShotStart = new();
        private readonly Subject<(Vector3 Position, float Percentage)> _onShotComplete = new();
        private readonly Subject<Unit> _onEntryShotState = new();
        private readonly Subject<float> _onEntryReloadState = new();
        private readonly Subject<Unit> _onEntrySkillState = new();
        private readonly Subject<Unit> _onExitShotState = new();
        private readonly Subject<Unit> _onInitializeData = new();
        private readonly Subject<Unit> _onDamageHitPoint = new();
        private readonly Subject<Vector3> _onSkillFieldInstantiatePosition = new();
        private readonly Subject<Unit> _onSkillAble = new();
        private readonly Subject<Unit> _onDeath = new();
        
        
        // イベントの公開プロパティ
        public IObservable<Unit> OnShotStart => _onShotStart.AsObservable();
        public IObservable<(Vector3 Position, float Percentage)> OnShotComplete => _onShotComplete.AsObservable();
        public IObservable<Unit> OnEntryShotState => _onEntryShotState.AsObservable();
        public IObservable<float> OnEntryReloadState => _onEntryReloadState.AsObservable();
        public IObservable<Unit> OnEntrySkillState => _onEntrySkillState.AsObservable();
        public IObservable<Unit> OnExitShotState => _onExitShotState.AsObservable();
        public IObservable<Unit> OnInitializeData => _onInitializeData.AsObservable();
        public IObservable<Unit> OnDamageHitPoint => _onDamageHitPoint.AsObservable();
        public IObservable<Vector3> OnSkillFieldInstantiatePosition => _onSkillFieldInstantiatePosition.AsObservable();
        public IObservable<Unit> OnSkillAble => _onSkillAble.AsObservable();
        public IObservable<Unit> OnDeath => _onDeath.AsObservable();
        
        // イベントのトリガーメソッド
        public void TriggerOnShotStart() => _onShotStart.OnNext(Unit.Default);
        public void TriggerOnShotComplete(Vector3 position, float percentage) => _onShotComplete.OnNext((position, percentage));
        public void TriggerOnEntryShotState() => _onEntryShotState.OnNext(Unit.Default);
        public void TriggerOnEntryReloadState(float reloadTime) => _onEntryReloadState.OnNext(reloadTime);
        public void TriggerOnExitShotState() => _onExitShotState.OnNext(Unit.Default);
        public void TriggerOnEnterSkillState() => _onEntrySkillState.OnNext(Unit.Default);
        public void TriggerOnInitializeData() => _onInitializeData.OnNext(Unit.Default);
        public void TriggerOnDamageHitPoint() => _onDamageHitPoint.OnNext(Unit.Default);
        public void TriggerOnSkillFieldInstantiatePosition(Vector3 position) => _onSkillFieldInstantiatePosition.OnNext(position);
        public void TriggerOnSkillAble() => _onSkillAble.OnNext(Unit.Default);
        public void TriggerOnDeath() => _onDeath.OnNext(Unit.Default);
    }
}
