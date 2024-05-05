using System.Collections.Generic;
using UnityEngine;

namespace Util.Calculator
{
    public abstract class EllipseCalculator
    {
        public static float[] CalculateEllipseSize(
            Camera mainCamera,
            float majorAxisEndPointInScreenX,
            float centerOfEllipseInScreenX
            )
        {
            var cameraAngleX = mainCamera.transform.eulerAngles.x;
            var majorAxisRadius = Mathf.Abs(majorAxisEndPointInScreenX - centerOfEllipseInScreenX);
            var minorAxisRadius = majorAxisRadius * Mathf.Sin(cameraAngleX * Mathf.Deg2Rad);
            var ellipseSize = new float[2]; // 2D楕円なので要素は2つで十分
            ellipseSize[0] = majorAxisRadius;
            ellipseSize[1] = minorAxisRadius;
            return ellipseSize;
        }
        
        public static bool IsInEllipse(Vector3 center, IReadOnlyList<float> ellipseSize, Vector3 position) 
        { 
            var sum = 0f;
            for (var i = 0; i < 2; i++) // 2D判定なのでi < 2
            {
                var a = (position[i] - center[i]);
                sum += (a * a) / (ellipseSize[i] * ellipseSize[i]);
            }
            return sum <= 1f;
        }
    }
}