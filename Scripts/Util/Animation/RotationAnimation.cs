using UnityEngine;

namespace Util.Animation
{
    public class RotationAnimation : MonoBehaviour
    {
        [SerializeField] private float rotateDegree;    // 1回の回転角度
        [SerializeField] private int fps; // 回転のFPS
        [SerializeField] private RotateAxis rotateAxis=RotateAxis.Y; // 回転軸
        public float RotateDegree
        {
            get => rotateDegree;
            set => rotateDegree = value;
        }
        
        private void Start()
        {
            var interval = 1f / fps;
            InvokeRepeating(nameof(RotateObject), 0f, interval);
        }

        private void RotateObject()
        {
            switch (rotateAxis)
            {
                case RotateAxis.X:
                    transform.Rotate(RotateDegree, 0, 0);
                    break;
                case RotateAxis.Y:
                    transform.Rotate(0, RotateDegree, 0);
                    break;
                case RotateAxis.Z:
                    transform.Rotate(0, 0, RotateDegree);
                    break;
                default:
                    transform.Rotate(0,RotateDegree,0);
                    break;
            }
        }
    }
    
    //enumで回転軸を指定する
    public enum RotateAxis
    {
        X,
        Y,
        Z
    }
}