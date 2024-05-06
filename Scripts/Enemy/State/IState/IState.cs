namespace Enemy.State.IState
{
    public interface IState
    {
        // このクラスの状態を取得する
        State State { get; }

        // 状態開始時に最初に実行される
        void Entry();

        // フレームごとに実行される
        void Update();

        // 物理演算のフレームごとに実行される
        void FixedUpdate();

        // 状態終了時に実行される
        void Exit();
    }

    //*******Stateを追加したい場合、ここにStateを追加***********
    public enum State
    {
        Idle,
        Death,
        Chase,
        GoStraight,
        Explode,
        Summon
    }
    //**********************************
}
