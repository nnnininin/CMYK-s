using UnityEngine;

namespace Util.RayCaster
{
    //画面に向けてスクリーンとなるカメラからRayを飛ばし、指定したレイヤーに当たったオブジェクトを取得するクラス
    public class RayCasterFromScreen
    {
        protected LayerMask LayerMask;

        //rayに通ってほしい平面BaseQuadのy座標
        private const float BaseQuadHeight = 1.1f;
        protected readonly float RayLength;

        protected Ray Ray;
        protected RaycastHit HitInfo;
        private readonly Camera _mainCamera;
        private readonly Vector3 _cameraForward;
        private readonly float _rayOriginDistanceFromQuad;

        // レイヤー名を指定しない場合はデフォルトの当たり判定用のレイヤーを使用する
        public RayCasterFromScreen(float rayLength = 10f, string layerName = "DefaultRayHit")
        {
            RayLength = rayLength;
            LayerMask = LayerMask.GetMask(layerName);
            _mainCamera = Camera.main;
            Ray = new Ray();
            if (_mainCamera == null) return;
            var mainCameraTransform = _mainCamera.transform;
            _cameraForward = mainCameraTransform.forward;
            _rayOriginDistanceFromQuad = RayLength / 2;
        }
        public RaycastHit? GetRayCastHit(Vector3 screenPosition, Color debugColor)
        {
            if (!_mainCamera) { return null; }

            var screenPosOnCameraQuad = new Vector3(screenPosition.x, screenPosition.y, 0f);
            var worldPosOnCameraQuad = _mainCamera.ScreenToWorldPoint(screenPosOnCameraQuad);

            //与えられたscreenPositionに応じてQuadからの距離を求める
            var heightFromQuad = worldPosOnCameraQuad.y - BaseQuadHeight;
            var distanceFromQuad = Mathf.Abs(heightFromQuad / Mathf.Cos(Vector3.Angle(_cameraForward, Vector3.up) * Mathf.Deg2Rad));
            //Rayの原点を設定
            Ray.origin = worldPosOnCameraQuad + _cameraForward * (distanceFromQuad - _rayOriginDistanceFromQuad);
            Ray.direction = _cameraForward;

            return CastRay(debugColor);
        }
        protected virtual RaycastHit? CastRay(Color debugColor)
        {
            Debug.DrawRay(Ray.origin, Ray.direction * RayLength, debugColor, 2f);
            if (!Physics.Raycast(Ray, out HitInfo, RayLength, LayerMask)) return null;
            return HitInfo;
        }
    }
}
