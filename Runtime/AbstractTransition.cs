namespace EasyFSM
{
    public abstract class AbstractTransition : ITransition
    {
        int ITransition.order { get; set; } = 0;

        public abstract void Excute(IChangeState context);
    }
}
