using UnityEngine;

namespace Util.Calculator
{
    public static class ConstantSpeedAutoTracking
    {
        //maxAccelerationが大きいほど追跡性能が上がる
        
        //2点,速度ベクトル,最大加速度を受け取り、新たな速度ベクトルを返す
        public static Vector3 CalculateTrackingVector(
            Vector3 targetPosition, 
            Vector3 currentPosition, 
            Vector3 velocity,
            float maxAcceleration
            )
        {
            var velocityMagnitude = velocity.magnitude;
            //velocityの大きさを1に揃える
            var velocityNormalized = velocity.normalized;
            //targetPositionとcurrentPositionの差分を取得
            var diff = targetPosition - currentPosition;
            //diffの大きさをvelocityと揃える
            var diffNormalized = diff.normalized;
            //diffNormalizedとvelocityNormalizedの差分を取得
            var diffVelocity = diffNormalized - velocityNormalized;
            //diffVelocitySqrtとmaxAccelerationSqrtを比較し、小さい方をdiffVelocityの大きさとする
            //最大加速度による制限
            var diffVelocitySqrt = diffVelocity.sqrMagnitude;
            var maxAccelerationSqrt = maxAcceleration * maxAcceleration;
            if (diffVelocitySqrt > maxAccelerationSqrt)
            {
                diffVelocity = diffVelocity.normalized * maxAcceleration;
            }
            //velocityにdiffVelocityを加算し、新たな速度ベクトルを生成し、大きさを再調整
            var newVelocity = velocity + diffVelocity;
            return newVelocity.normalized * velocityMagnitude;
        }
    }
}