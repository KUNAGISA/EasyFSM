namespace EasyFSM
{
    /// <summary>
    /// 进入状态接口
    /// </summary>
    public interface IEnterState
    {
        void EnterState();
    }

    /// <summary>
    /// 带参数进入状态接口
    /// </summary>
    /// <typeparam name="TParam">进入状态所需参数</typeparam>
    public interface IEnterState<TParam>
    {
        void EnterState(in TParam param);
    }
}
