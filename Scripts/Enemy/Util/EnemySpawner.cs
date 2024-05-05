using System;
using System.Collections.Generic;
using System.Threading;
using Enemy.ScriptableObject;
using Manager.DIContainer;
using UniRx;
using UnityEngine;
using Util.RayCaster;
using Zenject;
using Random = UnityEngine.Random;

namespace Enemy.Util
{
    public class EnemySpawner : MonoBehaviour
    {
        [Inject] private DiContainer _container;
        [Inject] private GlobalEnemyEventManager _globalEnemyEventManager;

        [SerializeField] private StageScriptableObject[] stageScriptableObjects;
        [SerializeField] private EnemyScriptableObject bossEnemy;
        [SerializeField] private EnemyScriptableObject summonEnemy;
        
        private GetSpawnPoints _getSpawnPoints;

        private bool _isInPeriod;
        
        private int _enemyIDNumber;

        private RayCasterFromScreen _rayCasterFromScreen;
        
        private CancellationTokenSource _cts;
        
        private readonly Subject<Unit> onStageEnd = new();
        public IObservable<Unit> OnStageEnd => onStageEnd;
        
        private int StageLevel{ get; set; }

        private float _timer;
        private float _nextSpawnIntervalSeconds;
        private List<float> _nextSpawnIntervalSecondsList;
        private int waveSpawnCountInPeriod;
        
        private List<WaveScriptableObject> _waveScriptableObjectsInPeriod;
        //そのperiod中に出現したwaveの数
        private int _spawnedWaveNumberInPeriod;
        
        private void Awake()
        {
            _getSpawnPoints = GetComponent<GetSpawnPoints>();
            _rayCasterFromScreen = new RayCasterFromScreen();
            _nextSpawnIntervalSecondsList = new List<float>();
            StageLevel = 0;
            waveSpawnCountInPeriod = 0;
            _timer = 0f;
            _enemyIDNumber = 0;
            _globalEnemyEventManager.OnGlobalSummon.Subscribe(_ => Summon()).AddTo(this);
        }
        private void Start()
        {
            //ステージのレベルに基づいて、periodを開始
            StartPeriod(stageScriptableObjects[StageLevel]);
        }

        private void FixedUpdate()
        {
            if (!_isInPeriod) return;
            
            //以下はperiod中の処理
            _timer += Time.deltaTime;
            //次のwaveを出現させるインターバルに達していなければ、処理を終了
            if(_timer < _nextSpawnIntervalSecondsList[waveSpawnCountInPeriod]) return;
            //出現させるwaveの数が、そのperiod中に出現するwaveの数を超えたら、periodを終了する
            if (_spawnedWaveNumberInPeriod >= _waveScriptableObjectsInPeriod.Count)
            {
                //ピリオド終了時の処理
                _isInPeriod = false;
                waveSpawnCountInPeriod = 0;
                //ステージのレベルを上げる
                IncreaseStageLevel();
                //最終ステージでなければ次のperiodを開始
                if (StageLevel < stageScriptableObjects.Length)
                {
                    StartPeriod(stageScriptableObjects[StageLevel]);
                }
                else
                {
                    //ボスを出現させる
                    SpawnBossEnemy(bossEnemy);
                }
                return;
            }
            //periodが終了していないかつ、次のwaveを出現させるインターバルに達しているため、次のwaveを出現させる
            var waveScriptableObject = _waveScriptableObjectsInPeriod[_spawnedWaveNumberInPeriod];
            SpawnEnemy(waveScriptableObject);
            _spawnedWaveNumberInPeriod++;
            _timer -= _nextSpawnIntervalSeconds;
        }
        
        private void IncreaseStageLevel()
        {
            Debug.Log("Stage Level increased.");
            StageLevel++;
        }
        
        private void StartPeriod(StageScriptableObject stageScriptableObject)
        {
            //timerを初期化し、stageScriptableObjectに基づいて、period中に出現する敵の情報を設定
            _isInPeriod = true;
            _timer = 0f;
            //period中に出現するwaveのリストの情報を設定
            _waveScriptableObjectsInPeriod = SetWaveScriptableObjectsInPeriod(stageScriptableObject);
            _spawnedWaveNumberInPeriod = 0;
        }

        private List<WaveScriptableObject> SetWaveScriptableObjectsInPeriod(
            StageScriptableObject stageScriptableObject)
        {
            //引数として渡されたstageScriptableObjectに基づいて、period中に出現するwaveのリストを設定
            const float initialIntervalSeconds = 0.5f;
            _nextSpawnIntervalSecondsList.Clear();
            _nextSpawnIntervalSecondsList.Add(initialIntervalSeconds);
            
            //period中に出現させるwaveを格納するリスト
            var waveScriptableObjectsInPeriod = new List<WaveScriptableObject>();
            //stageScriptableObjectからwaveScriptableObjectのリストを取得
            var waveScriptableObjects = stageScriptableObject.WaveScriptableObjects;

            //期間内に出現する敵の評価値の合計
            var totalEvaluation = 0;
            
            //waveDataからランダムに選択してwaveをリストに追加していく
            //期間内に出現する敵のインターバルの合計秒数が、ステージ期間の秒数を超えるまで繰り返す
            do
            {
                //stageから取得したwaveのリストの中からランダムに選択
                var selectedWaveScriptableObject = 
                    waveScriptableObjects[Random.Range(0, waveScriptableObjects.Length)];
                //選択されたwaveを出現させるwaveのリストに追加
                waveScriptableObjectsInPeriod.Add(selectedWaveScriptableObject);

                //選択されたwaveのevaluation(waveの持つ評価値,これが閾値を超えるとwaveの格納ループ終了)を計算
                var waveEvaluation = selectedWaveScriptableObject.GetEvaluation();
                Debug.Log($"waveEvaluation: " + $"{waveEvaluation}");
                totalEvaluation += (int)waveEvaluation;
                //選択されたwaveのevaluationに基づいて、次の出現インターバルを設定
                selectedWaveScriptableObject.SetNextSpawnIntervalSeconds(
                    stageScriptableObject.WaveInterval
                    * waveEvaluation
                    );
                //選択されたwaveの次の出現インターバルを取得
                _nextSpawnIntervalSeconds = selectedWaveScriptableObject.GetNextSpawnIntervalSeconds();
                _nextSpawnIntervalSecondsList.Add(_nextSpawnIntervalSeconds);
                Debug.Log($"nextSpawnIntervalSeconds: " + $"{_nextSpawnIntervalSeconds}");
            }
            //期間内に出現する敵の評価値の合計が、ステージ期間の評価値を超えるまで繰り返す
            while (totalEvaluation < stageScriptableObject.StageEvaluation);
            return waveScriptableObjectsInPeriod;
        }

        private void SpawnEnemy(WaveScriptableObject waveScriptableObject)
        {
            //出現する地点のscreenPositionを格納するリスト
            var spawnPositionsInScreen = GetSpawnPositionInScreen(waveScriptableObject);
            if (spawnPositionsInScreen == null) return;
            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (var spawnPositionInScreen in spawnPositionsInScreen)
            {
                var hitInfo = _rayCasterFromScreen.GetRayCastHit(spawnPositionInScreen, Color.green);
                if (hitInfo == null) continue;
                var spawnPositionInWorld = hitInfo.Value.point;
                BaseEnemy.Init(
                    waveScriptableObject.EnemyScriptableObject.EnemyPrefab,
                    waveScriptableObject.EnemyScriptableObject,
                    spawnPositionInScreen, 
                    spawnPositionInWorld,
                    _enemyIDNumber++,
                    _container);
            }
            waveSpawnCountInPeriod++;
        }

        private void SpawnBossEnemy(EnemyScriptableObject boss)
        {
            var spawnPositionInScreen = new Vector3(Screen.width * 0.7f, Screen.height / 2f, 0);
            var hitInfo = _rayCasterFromScreen.GetRayCastHit(spawnPositionInScreen, Color.green);
            if (hitInfo == null) return;
            var spawnPositionInWorld = hitInfo.Value.point;
            BaseEnemy.Init(
                boss.EnemyPrefab,
                boss,
                spawnPositionInScreen,
                spawnPositionInWorld,
                _enemyIDNumber++,
                _container);
        }

        private List<Vector3> GetSpawnPositionInScreen(
            WaveScriptableObject waveScriptableObject
            )
        {
            //出現する地点のscreenPositionを格納するリスト
            var spawnPositionsInScreen = new List<Vector3>();
            if (waveScriptableObject.DirectionNumber is <= 0 or > 4)
            {
                Debug.LogError("Direction number is invalid.");
            }
            //各方向に対して、出現する地点を決める
            for (var direction = 0; direction < waveScriptableObject.DirectionNumber; direction++)
            {
                var spawnDirection = Random.Range(0, 4); // 0から3までの値をランダムに生成

                // 出現するScreenPositionを入れた配列を作成
                var spawnPoints = spawnDirection switch
                {
                    0 => _getSpawnPoints.GetRightSpawnPointInScreen(), // 縦の右側
                    1 => _getSpawnPoints.GetLeftSpawnPointInScreen(),  // 縦の左側
                    2 => _getSpawnPoints.GetTopSpawnPointInScreen(),   // 横の上側
                    3 => _getSpawnPoints.GetBottomSpawnPointInScreen(),// 横の下側
                    _ => _getSpawnPoints.GetRightSpawnPointInScreen()
                };

                //出現ポイント1列あたりの要素数
                var arraySizeInOneLine = spawnPoints[0].Length;
                var selectedIndices = new List<int>();
            
                for (var i = 0; i < waveScriptableObject.GroupNumber; i++)
                {
                    var possibleStarts = new List<int>();
                    for (var start = 0; start <= arraySizeInOneLine - waveScriptableObject.EnemyNumberInGroup; start++)
                    {
                        var valid = true;
                        foreach (var index in selectedIndices)
                        {
                            if (Mathf.Abs(index - start) >= waveScriptableObject.MinElementNumberInGroup +
                                waveScriptableObject.EnemyNumberInGroup) continue;
                            valid = false;
                            break;
                        }
                        if (valid)
                        {
                            possibleStarts.Add(start);
                        }
                    }

                    if (possibleStarts.Count > 0)
                    {
                        var selectedStart = possibleStarts[Random.Range(0, possibleStarts.Count)];
                        for (var j = 0; j < waveScriptableObject.EnemyNumberInGroup; j++)
                        {
                            selectedIndices.Add(selectedStart + j);
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Not enough space to select more groups.");
                        break;
                    }
                }
                
                // lineループ
                for(var line = 0; line < waveScriptableObject.LineNumber; line++)
                {
                    foreach (var index in selectedIndices)
                    {
                        Debug.Log($"Selected Index: {index}");
                        spawnPositionsInScreen.Add(spawnPoints[line][index]);
                    }
                }
            }
            return spawnPositionsInScreen;
        }
        
        private void Summon()
        {
            Debug.Log("Summon");
            Vector3 randomPositionInScreen;
            Vector3 spawnPositionInWorld;
            bool hitInvalid;
            do {
                // 画面内のランダムな位置を取得
                //無効とならない位置を取得するまで繰り返す
                randomPositionInScreen = GetRandomPositionInScreen();
                spawnPositionInWorld = GetSpawnPositionInWorld();
                hitInvalid = HitInvalid(spawnPositionInWorld);
            } while (hitInvalid);
            
            BaseEnemy.Init(
                summonEnemy.EnemyPrefab,
                summonEnemy,
                randomPositionInScreen,
                spawnPositionInWorld,
                _enemyIDNumber++,
                _container
            );
        }
        
        private Vector3 GetSpawnPositionInWorld()
        {
            var randomPositionInScreen = GetRandomPositionInScreen();
            var hitInfo = _rayCasterFromScreen.GetRayCastHit(randomPositionInScreen, Color.blue);
            var spawnPositionInWorld = Vector3.zero;
            if(hitInfo != null)
            {
                spawnPositionInWorld = hitInfo.Value.point;
            }
            return spawnPositionInWorld;
        }
        
        private static bool HitInvalid(Vector3 spawnPosition)
        {
            //xz平面の8方向にレイを飛ばしてコライダーに当たるかチェック
            Vector3[] directions = {
                Vector3.forward, Vector3.back, Vector3.left, Vector3.right,
                Vector3.forward + Vector3.right, Vector3.forward + Vector3.left,
                Vector3.back + Vector3.right, Vector3.back + Vector3.left
            };
            foreach (var dir in directions)
            {
                if (RayHitsInvalidCollider(spawnPosition, dir))
                {
                    // コライダーに当たった場合、無効
                    return true;
                }
            }
            return false;
        }

        private static bool RayHitsInvalidCollider(Vector3 position, Vector3 direction)
        {
            const float maxDistance = 4.0f; // Rayの長さ
            
            var ray = new Ray(position, direction);
            if (!Physics.Raycast(ray, out var hit, maxDistance)) return false;
            // DefaultRayHitレイヤー以外に当たったかチェック
            return hit.collider.gameObject.layer != LayerMask.NameToLayer("DefaultRayHit");
        }
        
        private static Vector3 GetRandomPositionInScreen()
        {
            const int minDivision = 1;
            const int maxDivision = 9;
            
            // 画面を10分割した中で端点を除いてある点を取得
            var randomX = Random.Range(minDivision, maxDivision + 1) * (Screen.width / 10f);
            var randomY = Random.Range(minDivision, maxDivision + 1) * (Screen.height / 10f);
            
            return new Vector3(randomX, randomY,0);
        }
    }
}