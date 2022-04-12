namespace EasyFSM
{
    /// <summary>
    /// 状态机接口
    /// </summary>
    public interface IStateMachine<TState> : IChangeState<TState> where TState : IState<TState>
    {
        /// <summary>
        /// 当前状态实例
        /// </summary>
        TState CurrState { get; }

        /// <summary>
        /// 注册状态
        /// </summary>
        void RegisterState<T>() where T : TState, new();

        /// <summary>
        /// 注册状态
        /// </summary>
        /// <param name="state">状态实例</param>
        void RegisterState<T>(T state) where T : TState;

        /// <summary>
        /// 每帧回调
        /// </summary>
        /// <param name="delta">时间差</param>
        void TickStateMachine(in float delta);

        /// <summary>
        /// 发送事件到状态
        /// </summary>
        void SendEvent<T>() where T : new();

        /// <summary>
        /// 发送事件到状态
        /// </summary>
        /// <param name="event">事件</param>
        void SendEvent<T>(in T @event);
    }
}
