using CameraScript;
using UnityEngine;

namespace Player.Util
{
    public class PositionTransmitterToCameraPoint : MonoBehaviour
    {
        //カメラポイントの位置が分かっているかつオブジェクトの座標を同期させたいのでraycastは使わない
        private const string Tag = "CameraPoint";
        
        private Camera _mainCamera;
        private GameObject _cameraPoint;
        private CameraPointController _cameraPointController;
        private Vector3 _previousPosition;
        
        private void Start()
        {
            _mainCamera = Camera.main;
            if (_mainCamera == null)
                Debug.LogError("MainCamera is not found");
            _cameraPoint = GameObject.FindWithTag(Tag);
            if (_cameraPoint == null)
                Debug.LogError("CameraPoint is not found");
            _cameraPointController = _cameraPoint.GetComponent<CameraPointController>();
            _previousPosition = transform.position;
        }

        private void FixedUpdate()
        {
            _cameraPointController.DifferenceWorldPosition = GetDifference();
        }

        private Vector3 GetDifference()
        {
            var position = transform.position;
            //screenPositionに変換してスクリーン上に投影
            var currentScreenPosition = _mainCamera.WorldToScreenPoint(position);
            var previousScreenPosition = _mainCamera.WorldToScreenPoint(_previousPosition);
            //再びworld座標に戻す
            var currentWorldPositionOnPlane = _mainCamera.ScreenToWorldPoint(currentScreenPosition);
            var previousWorldPositionOnPlane = _mainCamera.ScreenToWorldPoint(previousScreenPosition);
            var difference = currentWorldPositionOnPlane - previousWorldPositionOnPlane;
            _previousPosition = position;
            return difference == Vector3.zero ? Vector3.zero : difference;
        }
    }
}