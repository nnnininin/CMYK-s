using UniRx;

namespace Enemy.Util
{
    public class EventManager
    {
        // ダメージを受けたときのイベント
        public Subject<int> OnDamageReceived { get; } = new();

        // 死亡時のイベント
        public Subject<Unit> OnDeath { get; } = new();
        
        //ボスの死亡時のイベント
        public Subject<Unit> OnBossDeath { get; } = new();
        
        // 召喚時のイベント
        public Subject<Unit> OnSummon { get; } = new();
    }
}