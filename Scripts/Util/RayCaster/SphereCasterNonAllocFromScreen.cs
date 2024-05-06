#nullable enable
using UnityEngine;

namespace Util.RayCaster
{
    //画面に向けてスクリーンとなるカメラから球状のRayを飛ばし、指定したレイヤーに当たった全てのオブジェクトを取得するクラス
    public class SphereCasterNonAllocFromScreen: RayCasterNonAllocFromScreen
    { 
        //rayOriginの位置によっては、当てたいものが球の内側に入ったままrayが消えてしまうことがあるため、
        //rayLengthを長く取ることで、球の内側に入ったままrayが消えることを防ぐ
        private float SphereRadius { get; }

        // レイヤー名を指定しない場合はデフォルトのレイヤーを使用する
        public SphereCasterNonAllocFromScreen(
            float sphereRadius, 
            int size, 
            float rayLength = 10f,
            string layerName = "DefaultRayHit"
            ): base(size,rayLength,layerName)
        {
            SphereRadius = sphereRadius;
            RayOriginDistanceFromQuad = RayLength;
        }
   
        protected override RaycastHit[]? CastRay(Color debugColor)
        {
            //衝突しなかった場合は-1を返すように明示的に初期化
            //hitInfo.distance = -1の時に衝突しなかったと判定する
            for (var i = 0; i < HitInfos.Length; i++){ HitInfos[i].distance = -1;}
            var hitCount = Physics.SphereCastNonAlloc(Ray, SphereRadius, HitInfos, RayLength, LayerMask);
            Debug.DrawRay(Ray.origin, Ray.direction * RayLength, debugColor, 2f);
            return hitCount <= 0 ? null : HitInfos;
        }
    }
}
