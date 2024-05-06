using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy.ScriptableObject
{
    [CreateAssetMenu(fileName = "Stage", menuName = "ScriptableObjects/Stage")]
    public class StageScriptableObject : UnityEngine.ScriptableObject
    {
        //ステージ中で出現する可能性のあるwaveのリスト
        [SerializeField]
        private WaveScriptableObject[] waveScriptableObjects;
        public WaveScriptableObject[] WaveScriptableObjects => waveScriptableObjects;
        
        //ステージが終わる評価値
        [SerializeField]
        private int stageEvaluation;
        public int StageEvaluation => stageEvaluation;
        
        //ステージ内でのwaveのintervalの基準値
        //インスペクタ上で整数値で設定できるように、0.001倍している
        [SerializeField][Range(1, 1000)]
        private float waveInterval;
        public float WaveInterval => waveInterval * 0.001f;
    }
}