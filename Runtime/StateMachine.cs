using System;
using System.Collections.Generic;

namespace EasyFSM
{
    /// <summary>
    /// 简易状态机
    /// </summary>
    public class StateMachine : IStateMachine
    {
        private IState m_state = default(IState);
        private Dictionary<Type, IState> m_stateMap = new Dictionary<Type, IState>();

        public IState CurrState => m_state;

        public void ChangeState<T>() where T : class, IEnterState, IState
        {
            /// 不带参数切状态的话相同状态不会切换
            if (m_stateMap.TryGetValue(typeof(T), out var newState))
            {
                m_state?.ExitState();
                m_state = newState;
                (m_state as T).EnterState();
            }
        }

        public void ChangeState<T, TParam>(in TParam param) where T : class, IEnterState<TParam>, IState
        {
            /// 带参数有可能参数不一样，所以需要重新切换
            if (m_stateMap.TryGetValue(typeof(T), out var newState) && !newState.Equals(m_state))
            {
                m_state?.ExitState();
                m_state = newState;
                (m_state as T).EnterState(in param);
            }
        }

        public void RegisterState<T>() where T : IState, new()
        {
            if (m_stateMap.ContainsKey(typeof(T)))
            {
                throw new Exception("重复添加状态");
            }

            var state = new T();
            m_stateMap.Add(typeof(T), state);
        }

        public void RegisterState<T>(T state) where T : IState
        {
            if (m_stateMap.ContainsKey(typeof(T)))
            {
                throw new Exception("重复添加状态");
            }
            m_stateMap.Add(typeof(T), state);
        }

        public void SendEvent<T>() where T : new()
        {
            (m_state as IEventReceiver<T>)?.Receive(new T());
        }

        public void SendEvent<T>(in T @event)
        {
            (m_state as IEventReceiver<T>)?.Receive(@event);
        }

        void IStateMachine.TickStateMachine(in float delta)
        {
            m_state?.TickState(in delta)?.Excute(this);
        }
    }
}
