namespace Enemy.Parameter
{
    public class Description
    {
        public string Name { get; private set; }
        public string DescriptionText { get; private set; }
        public int Evaluation { get; private set; }
        
        public Description(
            string name,
            string descriptionText,
            int evaluation
            )
        {
            Name = name;
            DescriptionText = descriptionText;
            Evaluation = evaluation;
        }
    }
}