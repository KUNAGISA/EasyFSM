namespace EasyFSM
{
    /// <summary>
    /// 状态转换接口
    /// </summary>
    public interface ITransition<TState> where TState : IState<TState>
    {
        public void Excute(IChangeState<TState> context);
    }
}
