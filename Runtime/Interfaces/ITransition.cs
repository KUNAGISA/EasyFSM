namespace EasyFSM
{
    /// <summary>
    /// 状态转换接口
    /// </summary>
    public interface ITransition
    {
        internal int order { get; set; }

        void Excute(IChangeState context);
    }
}
