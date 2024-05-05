// ReSharper disable once RedundantUsingDirective

using UnityEngine.InputSystem;

namespace Util.Input
{
    public class PreventMashInteraction : IInputInteraction
    {
        // 最小の入力間隔[s]（押された後、入力を受け付けない時間[s]）
        public float MinInputDuration;

        // 入力判定の閾値(0でデフォルト値)
        public float PressPoint;

        // 設定値かデフォルト値の値を格納するフィールド
        private float PressPointOrDefault => PressPoint > 0 ? PressPoint : InputSystem.settings.defaultButtonPressPoint;
        private float ReleasePointOrDefault => PressPointOrDefault * InputSystem.settings.buttonReleaseThreshold;

        // 直近のPerformed状態に遷移した時刻
        private double _lastPerformedTime;

        /// <summary>
        /// 初期化
        /// </summary>
#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
//#else
    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
#endif
        public static void Initialize()
        {
            // 初回にInteractionを登録する必要がある
            InputSystem.RegisterInteraction<PreventMashInteraction>();
        }

        public void Process(ref InputInteractionContext context)
        {
            if (context.isWaiting)
            {
                // Waiting状態

                // 入力が０以外かどうか
                if (context.ControlIsActuated())
                {
                    // ０以外ならStarted状態に遷移
                    context.Started();
                }
            }

            if (context.isStarted)
            {
                // Started状態

                // 入力がPress以上
                //     かつ
                // 前回のPerformed状態遷移から「minInputDuration」以上経過 したかどうか
                if (context.ControlIsActuated(PressPointOrDefault) && context.time >= _lastPerformedTime + MinInputDuration)
                {
                    // Performed状態に遷移
                    context.PerformedAndStayPerformed();

                    // Performed状態遷移時の時刻を保持
                    _lastPerformedTime = context.time;
                }
                // 入力が０かどうか
                else if (!context.ControlIsActuated())
                {
                    // ０ならCanceled状態に遷移
                    context.Canceled();
                }
            }

            if (context.phase != InputActionPhase.Performed) return;
            // Performed状態

            // 入力がRelease以下かどうか
            if (!context.ControlIsActuated(ReleasePointOrDefault))
            {
                // Canceled状態に遷移
                context.Canceled();
            }
        }

        public void Reset()
        {
        }
    }
}