namespace EasyFSM
{
    /// <summary>
    /// 切换状态接口
    /// </summary>
    public interface IChangeState<TState> where TState : IState<TState>
    {
        /// <summary>
        /// 切换状态
        /// </summary>
        void ChangeState<T>() where T : class, TState, IEnterState;

        /// <summary>
        /// 带参数切换状态
        /// </summary>
        /// <param name="param">切换状态参数</param>
        void ChangeState<T, TParam>(in TParam param) where T : class, TState, IEnterState<TParam>;
    }
}
