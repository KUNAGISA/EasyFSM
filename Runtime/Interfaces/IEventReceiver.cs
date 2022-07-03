namespace EasyFSM
{
    /// <summary>
    /// 状态事件接收接口
    /// </summary>
    public interface IEventReceiver<TEvent>
    {
        ITransition ReceiveEvent(in TEvent @event);
    }
}
