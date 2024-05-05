using UnityEngine;

namespace Util.Animation
{
    public class GenerateRectangleParticle : MonoBehaviour
    {
        [SerializeField] private GameObject rectangleParticlePrefab;
        [SerializeField] private float interval;
        // 格子の半分のサイズ
        [SerializeField] private int gridHalfSize;

        private void Start()
        {
            Generate();
        }

        private void Generate()
        {
            // 中心からのオフセットを計算（格子の中心がこのオブジェクトの位置に来るように）
            for (var i = -gridHalfSize; i <= gridHalfSize; i++)
            {
                for (var j = -gridHalfSize; j <= gridHalfSize; j++)
                {
                    // 中心からのオフセットを考慮して位置を計算
                    var transformOfThis = transform;
                    var positionOfThis = transformOfThis.position;
                    var position = new Vector3(
                        positionOfThis.x + interval * j,
                        positionOfThis.y,
                        positionOfThis.z + interval * i);
                    // 子オブジェクトとして生成
                    Instantiate(rectangleParticlePrefab, position, Quaternion.identity, transformOfThis);
                }
            }
        }
    }
}