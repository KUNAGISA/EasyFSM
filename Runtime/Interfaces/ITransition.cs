namespace EasyFSM
{
    /// <summary>
    /// 状态转换接口
    /// </summary>
    public interface ITransition
    {
        void Excute(IChangeState context);
    }
}
