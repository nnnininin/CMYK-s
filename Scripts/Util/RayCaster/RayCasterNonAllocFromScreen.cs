#nullable enable
using UnityEngine;

namespace Util.RayCaster
{
    public class RayCasterNonAllocFromScreen
    {
        protected LayerMask LayerMask;
        private int Size { get; }   // HitInfoが格納される配列のサイズ
        private const float BaseQuadHeight = 1.1f;
        protected readonly float RayLength;

        protected Ray Ray;
        protected readonly RaycastHit[] HitInfos;
        private readonly Camera? _mainCamera;
        private readonly Vector3 _cameraForward;
        protected float RayOriginDistanceFromQuad;

        public RayCasterNonAllocFromScreen(int size,float rayLength = 10f,string layerName = "DefaultRayHit")
        {
            Size = size;
            RayLength = rayLength;
            Ray = new Ray();
            HitInfos = new RaycastHit[Size];
            if (Camera.main != null) _mainCamera = Camera.main;
            if (_mainCamera == null) return;
            var mainCameraTransform = _mainCamera.transform;
            _cameraForward = mainCameraTransform.forward;
            LayerMask = LayerMask.GetMask(layerName);
            RayOriginDistanceFromQuad = RayLength/2;
        }

        public RaycastHit[]? GetRayCastHits(Vector3 screenPosition, Color debugColor)
        {
            if (!_mainCamera) { return null; }

            var screenPosOnCameraQuad = new Vector3(screenPosition.x, screenPosition.y, 0f);
            var worldPosOnCameraQuad = _mainCamera.ScreenToWorldPoint(screenPosOnCameraQuad);

            // 与えられたscreenPositionに応じてQuadからの距離を求める
            var heightFromQuad = worldPosOnCameraQuad.y - BaseQuadHeight;
            var distanceFromQuad = Mathf.Abs(heightFromQuad / Mathf.Cos(Vector3.Angle(_cameraForward, Vector3.up) * Mathf.Deg2Rad));

            // Rayの原点を設定
            Ray.origin = worldPosOnCameraQuad + _cameraForward * (distanceFromQuad - RayOriginDistanceFromQuad);
            Ray.direction = _cameraForward;

            return CastRay(debugColor);
        }

        protected virtual RaycastHit[]? CastRay(Color debugColor)
        {
            //hitCountはint型で当たった数を返す
            var hitCount = Physics.RaycastNonAlloc(Ray, HitInfos, RayLength, LayerMask);
            Debug.DrawRay(Ray.origin, Ray.direction * RayLength, debugColor, 2f);

            // 当たった数が0以下の場合は null を返す
            if (hitCount <= 0) return null;

            // HitInfos のサイズが hitCount より大きい場合、不要なヒット情報をクリア
            for (var i = hitCount; i < HitInfos.Length; i++)
            {
                HitInfos[i] = new RaycastHit();
            }
            return HitInfos;
        }
    }
}