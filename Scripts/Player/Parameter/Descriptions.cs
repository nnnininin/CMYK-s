using UniRx;

namespace Player.Parameter
{
    public class Descriptions
    {
        private readonly ReactiveProperty<string> _name = new();
        public IReadOnlyReactiveProperty<string> NameValue => _name;
        
        private readonly ReactiveProperty<string> _charaDescription = new();
        public IReadOnlyReactiveProperty<string> CharaDescriptionValue => _charaDescription;
        
        public Descriptions(CharaParameter charaParameter)
        {
            _name.Value = charaParameter.Name.Value;
            _charaDescription.Value = charaParameter.CharaDescription.Value;
        }
    }
}