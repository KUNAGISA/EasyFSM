using System;
using System.Collections.Generic;

namespace EasyFSM
{
    /// <summary>
    /// 简易状态机
    /// </summary>
    public class StateMachine<TState> : IStateMachine<TState> where TState : IState<TState>
    {
        private TState m_state = default(TState);
        private Dictionary<Type, TState> m_stateMap = new Dictionary<Type, TState>();

        public TState CurrState => m_state;

        public void ChangeState<T>() where T : class, IEnterState, TState
        {
            /// 不带参数切状态的话相同状态不会切换
            if (m_stateMap.TryGetValue(typeof(T), out var newState))
            {
                m_state?.ExitState();
                m_state = newState;
                (m_state as T).EnterState();
            }
        }

        public void ChangeState<T, TParam>(in TParam param) where T : class, IEnterState<TParam>, TState
        {
            /// 带参数有可能参数不一样，所以需要重新切换
            if (m_stateMap.TryGetValue(typeof(T), out var newState) && !newState.Equals(m_state))
            {
                m_state?.ExitState();
                m_state = newState;
                (m_state as T).EnterState(in param);
            }
        }

        public void RegisterState<T>() where T : TState, new()
        {
            if (m_stateMap.ContainsKey(typeof(T)))
            {
                throw new Exception("重复添加状态");
            }

            var state = new T();
            m_stateMap.Add(typeof(T), state);
            OnRegisterState(state);
        }

        public void RegisterState<T>(T state) where T : TState
        {
            if (m_stateMap.ContainsKey(typeof(T)))
            {
                throw new Exception("重复添加状态");
            }
            m_stateMap.Add(typeof(T), state);
            OnRegisterState(state);
        }

        public void SendEvent<T>() where T : new()
        {
            (m_state as IEventReceiver<T>)?.Receive(new T());
        }

        public void SendEvent<T>(in T @event)
        {
            (m_state as IEventReceiver<T>)?.Receive(@event);
        }

        void IStateMachine<TState>.TickStateMachine(in float delta)
        {
            m_state?.TickState(in delta)?.Excute(this);
            OnTickStateMachine(in delta);
        }
        protected virtual void OnRegisterState(TState state) { }

        protected virtual void OnTickStateMachine(in float delta) { }
    }
}
