using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFSM
{
    /// <summary>
    /// 状态事件接收接口
    /// </summary>
    public interface IEventReceiver<TState, TEvent> where TState : IState<TState>
    {
        ITransition<TState> Receive(in TEvent @event);
    }
}
