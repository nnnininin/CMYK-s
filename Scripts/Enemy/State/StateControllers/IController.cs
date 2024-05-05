namespace Enemy.State.StateControllers
{
    public interface IController
    {
        void Initialize(IEnemy enemy);
        void AddUniqueStates();
        IState.State GetInitialState();
        IState.State CheckTransitions();
    }
}