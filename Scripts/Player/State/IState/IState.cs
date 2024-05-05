namespace Player.State.IState
{
    public interface IState
    {
        // このクラスの状態を取得する
        State State { get; }

        // 状態開始時に最初に実行される
        void Entry();

        // フレームごとに実行される
        void Update();

        // 固定フレームごとに実行される
        void FixedUpdate();

        // 状態終了時に実行される
        void Exit();
    }

    // プレイヤーの状態
    public enum State
    {
        Idle,
        Shot,
        Reload,
        Skill
    }
}
