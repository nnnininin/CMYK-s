using UnityEngine;

namespace CameraScript
{
    public class CameraPointController : MonoBehaviour
    {
        public Vector3 DifferenceWorldPosition { get; set; }  //前回の位置とのワールド差分

        private void Start()
        {
            DifferenceWorldPosition = Vector3.zero;
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            transform.position += DifferenceWorldPosition;
        }
    }
}