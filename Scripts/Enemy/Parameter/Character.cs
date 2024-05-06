namespace Enemy.Parameter
{
    public class Character
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Evaluation { get; private set; }
        
        public Character(
            string name,
            string description,
            int evaluation
            )
        {
            Name = name;
            Description = description;
            Evaluation = evaluation;
        }
    }
}