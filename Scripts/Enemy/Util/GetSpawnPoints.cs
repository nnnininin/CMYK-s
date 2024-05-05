using System.Collections.Generic;
using UnityEngine;

namespace Enemy.Util
{
    public class GetSpawnPoints : MonoBehaviour
    {
        private const int HorizontalSpawnPointNumber = 32;
        private const int VerticalSpawnPointNumber = 18;
        private const int NumberOfEnemySpawnLine = 4;

        private float _horizontalDistanceBetweenSpawnPoint;
        private float _verticalDistanceBetweenSpawnPoint;

        private Vector3[][] _topSpawnPointScreenPosition;
        private Vector3[][] _bottomSpawnPointScreenPosition;
        private Vector3[][] _leftSpawnPointScreenPosition;
        private Vector3[][] _rightSpawnPointScreenPosition;

        private void Awake()
        {
            InitializeSpawnPoint();
        }

        private void InitializeSpawnPoint()
        {
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
            var pointNumber = isHorizontal ? HorizontalSpawnPointNumber : VerticalSpawnPointNumber;

            for (var line = 0; line < NumberOfEnemySpawnLine; line++)
            {
                for (var point = 0; point < pointNumber; point++)
                {
                    var marginFromScreenEdge = line+1;
                    if (isHorizontal)
                    {
                        var x = _horizontalDistanceBetweenSpawnPoint / 2 + point * _horizontalDistanceBetweenSpawnPoint;
                        //列の増加方向によって、yの値を変える
                        var y = isLineIncreasingDirectionPositive ? 
                            Screen.height + marginFromScreenEdge * _verticalDistanceBetweenSpawnPoint : 
                            -marginFromScreenEdge * _verticalDistanceBetweenSpawnPoint;
                        spawnPointArray[line][point] = new Vector3(x, y, 0);
                    }
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