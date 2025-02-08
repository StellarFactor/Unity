namespace StellarFactor
{
    public interface IVisitable
    {
        bool BeenVisited { get; set; }
        void Visit();
        void Enter();
        void Leave();
    }
}