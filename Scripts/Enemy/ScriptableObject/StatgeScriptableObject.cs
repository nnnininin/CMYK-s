using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy.ScriptableObject
{
    [CreateAssetMenu(fileName = "Stage", menuName = "ScriptableObjects/Stage")]
    public class StageScriptableObject : UnityEngine.ScriptableObject
    {
        [SerializeField]
        private WaveScriptableObject[] waveScriptableObjects;
        public WaveScriptableObject[] WaveScriptableObjects => waveScriptableObjects;
        
        //ステージが終わる評価値
        [SerializeField]
        private int stageEvaluation;
        public int StageEvaluation => stageEvaluation;
        
        //ステージ内でのwaveのintervalの基準値
        [FormerlySerializedAs("waveIntervalSeconds")] [SerializeField][Range(1, 1000)]
        private float waveInterval;
        public float WaveInterval => waveInterval * 0.001f;
    }
}