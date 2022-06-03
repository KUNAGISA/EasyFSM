namespace EasyFSM
{
    /// <summary>
    /// 状态接口
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// 所有初始化数据完成后状态初始化
        /// </summary>
        void InitState();

        /// <summary>
        /// 状态激活时每帧回调
        /// </summary>
        /// <param name="delta">时间差</param>
        /// <returns>如果需要切换状态则返回状态转换实例</returns>
        ITransition TickState(in float delta);

        /// <summary>
        /// 离开状态回调
        /// </summary>
        void ExitState();
    }
}
