using Scene;
using UnityEngine;

namespace Enemy.Util
{
    public class ItemInstantiateController: MonoBehaviour
    {
        [SerializeField]
        private GameObject cyanItemPrefab;
        [SerializeField]
        private GameObject magentaItemPrefab;
        [SerializeField]
        private GameObject yellowItemPrefab;

        public void InstantiateItem(Vector3 position, ColorType colorType)
        {
            var itemPrefab = colorType switch
            {
                ColorType.Cyan => cyanItemPrefab,
                ColorType.Magenta => magentaItemPrefab,
                ColorType.Yellow => yellowItemPrefab,
                _ => null
            };
            Instantiate(itemPrefab, position, Quaternion.identity);
        }
    }
}