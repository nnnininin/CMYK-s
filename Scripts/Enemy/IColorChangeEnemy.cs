using System.Collections;
using Scene;

namespace Enemy
{
    public interface IColorChangeEnemy
    {
        public ColorType GetRandomColorType();
        public void SetColorType(ColorType colorType);
        public void ChangeColorCountDown(float deltaTime);
        public IEnumerator BlinkColor();
    }
}