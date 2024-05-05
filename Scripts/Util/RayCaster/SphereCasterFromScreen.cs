using UnityEngine;

namespace Util.RayCaster
{
    public class SphereCasterFromScreen : RayCasterFromScreen
    {
        private float SphereRadius { get; }
    
        public SphereCasterFromScreen(float sphereRadius,float rayLength = 10f, string layerName = "DefaultRayHit") : base(rayLength,layerName)
        {
            SphereRadius = sphereRadius;
        }
    
        protected override RaycastHit? CastRay(Color debugColor)
        {
            Debug.DrawRay(Ray.origin, Ray.direction * RayLength, debugColor, 2f);
            if (!Physics.SphereCast(Ray, SphereRadius, out HitInfo, RayLength, LayerMask)) return null;
            return HitInfo;
        }
    }
}