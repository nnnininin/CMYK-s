using AttributeScripts;
using UnityEngine;

namespace Enemy.ScriptableObject
{
    [CreateAssetMenu(fileName = "Wave", menuName = "ScriptableObjects/Wave")]
    public class WaveScriptableObject : UnityEngine.ScriptableObject
    {
        //敵の出現位置は、画面端をいくつかに区切ったマス目を基準に考えている
        
        //1つのWaveの中でGroup単位で敵がいくつも出現する
        [SerializeField][Tooltip("通常の敵")]
        private EnemyScriptableObject enemyScriptableObject;
        public EnemyScriptableObject EnemyScriptableObject => enemyScriptableObject;
        
        [SerializeField][Range(1, 16)][Tooltip("1グループの敵の数")]
        private int enemyNumberInGroup;
        public int EnemyNumberInGroup => enemyNumberInGroup;

        //一度に出現する敵のグループの数
        [SerializeField][Range(1, 8)][Tooltip("1Waveの中で出現するグループの数")]
        private int groupNumber;
        public int GroupNumber => groupNumber;
        
        //グループ内の最小の要素数
        [SerializeField][Range(1, 3)][Tooltip("グループの中で、敵同士の間に最低限存在するマスの数")]
        private int minElementNumberInGroup;
        public int MinElementNumberInGroup => minElementNumberInGroup;
        
        //敵の出現する方向の数
        [SerializeField][Range(1, 4)][Tooltip("敵の出現方向の数1~4方向")]
        private int directionNumber;
        public int DirectionNumber => directionNumber;
        
        //ある方向で出現する敵の列の数
        [SerializeField][Range(1, 3)][Tooltip("敵が出現する列の数1~3ライン")]
        private int lineNumber;
        public int LineNumber => lineNumber;

        //合計の敵の数(インスペクタから確認用)
        [field: SerializeField]
        [field: Immutable]
        public int AllEnemyNumber { get; private set; }
        
        //Waveの評価値(インスペクタから確認用)
        [field:SerializeField]
        [field:Immutable]
        public int WaveEvaluation { get; private set; }
        private void OnValidate()
        {
            AllEnemyNumber = enemyNumberInGroup * groupNumber * directionNumber * lineNumber;
            WaveEvaluation = GetEvaluation();
        }
        
        //次のWaveが出現するまでの時間
        private float nextSpawnIntervalSeconds;
        public float GetNextSpawnIntervalSeconds()
        {
            return nextSpawnIntervalSeconds;
        }
        public void SetNextSpawnIntervalSeconds(float seconds)
        {
            nextSpawnIntervalSeconds = seconds;
        }
        
        //Waveの評価値を計算する
        public int GetEvaluation()
        {
            var waveEvaluation 
                = enemyScriptableObject.Evaluation 
                  * enemyNumberInGroup 
                  * groupNumber 
                  * directionNumber 
                  * lineNumber;
            return waveEvaluation;
        }
    }
}