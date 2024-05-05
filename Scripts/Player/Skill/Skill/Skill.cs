using System;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace Player.Skill.Skill 
{ 
    [RequireComponent(typeof(LineRenderer))]
    [RequireComponent(typeof(LineRenderer))]
    public class Skill : MonoBehaviour
    {
        private readonly Subject<Vector3> _onCompleteSubject = new();
        public IObservable<Vector3> OnComplete => _onCompleteSubject;
        
        private const float Speed = 12f;
        private const float HeightOfVertex = 4;
        private const float DelayTime = 0.3f;
        
        private const float Radius = 0.5f;
        public void SetSkillTargetPosition(Vector3 spawnPosition,Vector3 targetPosition)
        {
            const float multiplier = 0.2f;
            
            var directionVector = CalculateDirectionVector(spawnPosition,targetPosition);
            var vertexPosition = CalculateVertexPosition(spawnPosition,directionVector);

            var upVector = Vector3.up;
            
            var firstOutPosition = spawnPosition + upVector * multiplier;
            var secondInPosition = vertexPosition - directionVector * multiplier;
            var secondOutPosition = vertexPosition + directionVector * multiplier;
            var thirdInPosition = targetPosition + upVector * multiplier;
            
            var lineRenderers = GetComponentsInChildren<LineRenderer>();
            
            RenderCircle(lineRenderers[0], targetPosition, Radius);
            RenderStraightLine(lineRenderers[1], spawnPosition, targetPosition);
            
            //vertexPositionを中継地点にしてDoPathを使う
            var path = new[] { 
                vertexPosition,
                firstOutPosition,
                secondInPosition,
                targetPosition,
                secondOutPosition,
                thirdInPosition,
            };
            transform.DOPath(path,Speed, PathType.CubicBezier)
                .SetSpeedBased()
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    _onCompleteSubject.OnNext(transform.position);
                    _onCompleteSubject.OnCompleted();
                    //DelayTime秒後に自身を破棄する
                    //第3引数にfalseを入れることで、TimeScaleに依存するようになる
                    DOVirtual.DelayedCall(DelayTime,
                        () => Destroy(gameObject),
                        false);
                })
                .SetLink(gameObject);
        }
        
        private static Vector3 CalculateVertexPosition(Vector3 spawnPosition,Vector3 directionVector)
        {
            const float vertexPercentage = 0.6f;
            //percentageの割合の大きさのベクトルを作る
            var halfDirectionVectorSize = directionVector.magnitude * vertexPercentage;
            //halfDirectionVectorを、directionVectorの大きさの半分にする
            var halfDirectionVector = directionVector.normalized * halfDirectionVectorSize;
            //SpawnPositionにhalfDirectionVectorを足す
            //この時にy軸の値はSpawnPositionのy軸の値になる
            var halfPosition = spawnPosition + halfDirectionVector;
            //halfPositionにy軸の値を足す
            var addY = new Vector3(0, HeightOfVertex, 0);
            var vertexPosition = halfPosition + addY;
            return vertexPosition;
        }
        
        private static Vector3 CalculateDirectionVector(Vector3 spawnPosition,Vector3 targetPosition)
        {
            var directionVector = targetPosition - spawnPosition;
            directionVector.y = 0;
            return directionVector;
        }
        
        private static void RenderStraightLine(LineRenderer lineRenderer, Vector3 spawnPosition, Vector3 targetPosition)
        {
            const float lineWidth = 0.02f;
            var lineColor = Color.white;
    
            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;
            lineRenderer.startColor = lineColor;
            lineRenderer.endColor = lineColor;
            lineRenderer.positionCount = 2;
            lineRenderer.loop = false; // 直線なので閉じない
    
            var points = new Vector3[2];
            points[0] = spawnPosition;
            points[1] = targetPosition;
            lineRenderer.SetPositions(points);
        }
        
        private static void RenderCircle(LineRenderer lineRenderer, Vector3 targetPosition, float radius)
        {
            const int segments = 100;
            const float lineWidth = 0.02f;
            var lineColor = Color.white;
    
            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;
            lineRenderer.startColor = lineColor;
            lineRenderer.endColor = lineColor;
            lineRenderer.positionCount = segments + 1;
            lineRenderer.loop = true; // 円を閉じる

            var points = new Vector3[segments + 1];
            for (var i = 0; i <= segments; i++)
            {
                var angle = Mathf.Deg2Rad * (i * 360f / segments);
                var x = targetPosition.x + Mathf.Sin(angle) * radius;
                var y = targetPosition.y;
                var z = targetPosition.z + Mathf.Cos(angle) * radius;
                points[i] = new Vector3(x, y, z);
            }
            lineRenderer.SetPositions(points);
        }
    }
}