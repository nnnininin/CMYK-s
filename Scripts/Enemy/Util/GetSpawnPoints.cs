using System.Collections.Generic;
using UnityEngine;

namespace Enemy.Util
{
    public class GetSpawnPoints : MonoBehaviour
    {
        //水平方向（上端下端）のスポーンポイントの数
        private const int HorizontalSpawnPointNumber = 32;
        //垂直方向（左端右端）のスポーンポイントの数
        private const int VerticalSpawnPointNumber = 18;
        //敵の出現する列の数
        private const int NumberOfEnemySpawnLine = 4;

        private float _horizontalDistanceBetweenSpawnPoint;
        private float _verticalDistanceBetweenSpawnPoint;

        //画面上端のスポーンポイントの位置
        private Vector3[][] _topSpawnPointScreenPosition;
        //画面下端のスポーンポイントの位置
        private Vector3[][] _bottomSpawnPointScreenPosition;
        //画面左端のスポーンポイントの位置
        private Vector3[][] _leftSpawnPointScreenPosition;
        //画面右端のスポーンポイントの位置
        private Vector3[][] _rightSpawnPointScreenPosition;

        private void Awake()
        {
            InitializeSpawnPoint();
        }

        private void InitializeSpawnPoint()
        {
            // スポーンポイント間の距離を計算
            _horizontalDistanceBetweenSpawnPoint = Screen.width / (float)HorizontalSpawnPointNumber;
            _verticalDistanceBetweenSpawnPoint = Screen.height / (float)VerticalSpawnPointNumber;

            // ジャグ配列の初期化
            _topSpawnPointScreenPosition = new Vector3[NumberOfEnemySpawnLine][];
            _bottomSpawnPointScreenPosition = new Vector3[NumberOfEnemySpawnLine][];
            _leftSpawnPointScreenPosition = new Vector3[NumberOfEnemySpawnLine][];
            _rightSpawnPointScreenPosition = new Vector3[NumberOfEnemySpawnLine][];

            for (var i = 0; i < NumberOfEnemySpawnLine; i++)
            {
                _topSpawnPointScreenPosition[i] = new Vector3[HorizontalSpawnPointNumber];
                _bottomSpawnPointScreenPosition[i] = new Vector3[HorizontalSpawnPointNumber];
                _leftSpawnPointScreenPosition[i] = new Vector3[VerticalSpawnPointNumber];
                _rightSpawnPointScreenPosition[i] = new Vector3[VerticalSpawnPointNumber];
            }

            // スポーンポイントの位置を計算
            CalculateSpawnPosition(_topSpawnPointScreenPosition, true, true);
            CalculateSpawnPosition(_bottomSpawnPointScreenPosition, true, false);
            CalculateSpawnPosition(_leftSpawnPointScreenPosition, false, false);
            CalculateSpawnPosition(_rightSpawnPointScreenPosition, false, true);
        }

        private void CalculateSpawnPosition(
            IReadOnlyList<Vector3[]> spawnPointArray,
            bool isHorizontal, 
            bool isLineIncreasingDirectionPositive
            )
        {
            // 水平方向の場合は水平方向のスポーンポイント数を、垂直方向の場合は垂直方向のスポーンポイント数を取得
            var pointNumber = isHorizontal ? HorizontalSpawnPointNumber : VerticalSpawnPointNumber;
            
            for (var line = 0; line < NumberOfEnemySpawnLine; line++)
            {
                for (var point = 0; point < pointNumber; point++)
                {
                    var marginFromScreenEdge = line+1;
                    // 水平方向の場合
                    if (isHorizontal)
                    {
                        var x = _horizontalDistanceBetweenSpawnPoint / 2 + point * _horizontalDistanceBetweenSpawnPoint;
                        //列の増加方向によって、yの値を変える
                        var y = isLineIncreasingDirectionPositive ? 
                            Screen.height + marginFromScreenEdge * _verticalDistanceBetweenSpawnPoint : 
                            -marginFromScreenEdge * _verticalDistanceBetweenSpawnPoint;
                        spawnPointArray[line][point] = new Vector3(x, y, 0);
                    }
                    // 垂直方向の場合
                    else
                    {
                        var x = isLineIncreasingDirectionPositive ? 
                            Screen.width + marginFromScreenEdge* _horizontalDistanceBetweenSpawnPoint :
                            -marginFromScreenEdge * _horizontalDistanceBetweenSpawnPoint;
                        var y = _verticalDistanceBetweenSpawnPoint / 2 + point * _verticalDistanceBetweenSpawnPoint;
                        spawnPointArray[line][point] = new Vector3(x, y, 0);
                    }
                }
            }
        }

        // Getterメソッドの返り値の型をジャグ配列に変更
        public Vector3[][] GetTopSpawnPointInScreen() => _topSpawnPointScreenPosition;
        public Vector3[][] GetBottomSpawnPointInScreen() => _bottomSpawnPointScreenPosition;
        public Vector3[][] GetLeftSpawnPointInScreen() => _leftSpawnPointScreenPosition;
        public Vector3[][] GetRightSpawnPointInScreen() => _rightSpawnPointScreenPosition;
    }
}