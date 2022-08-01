using System;
using System.Collections.Generic;

namespace EasyFSM
{
    /// <summary>
    /// 简易状态机
    /// </summary>
    public class StateMachine : IStateMachine
    {
        private IState m_currState = null;
        private ITransition m_waitExecuteTransition = null;

        private Dictionary<Type, IState> m_stateMap = new Dictionary<Type, IState>();

        public IState currState => m_currState;

        public void ChangeState<T>() where T : class, IEnterState, IState
        {
            /// 不带参数切状态的话相同状态不会切换
            if (m_stateMap.TryGetValue(typeof(T), out var newState))
            {
                m_currState?.ExitState();
                m_currState = newState;
                (m_currState as T).EnterState();
            }
        }

        public void ChangeState<T, TParam>(in TParam param) where T : class, IEnterState<TParam>, IState
        {
            /// 带参数有可能参数不一样，所以需要重新切换
            if (m_stateMap.TryGetValue(typeof(T), out var newState) && !newState.Equals(m_currState))
            {
                m_currState?.ExitState();
                m_currState = newState;
                (m_currState as T).EnterState(in param);
            }
        }

        public void RegisterState<T>() where T : IState, new()
        {
            if (m_stateMap.ContainsKey(typeof(T)))
            {
                throw new Exception("重复添加状态");
            }

            var state = new T();
            state.InitState();
            m_stateMap.Add(typeof(T), state);
        }

        public void RegisterState<T>(T state) where T : IState
        {
            if (m_stateMap.ContainsKey(typeof(T)))
            {
                throw new Exception("重复添加状态");
            }
            state.InitState();
            m_stateMap.Add(typeof(T), state);
        }

        public void SendEvent<T>() where T : new()
        {
            SwapWaitTransition((m_currState as IEventReceiver<T>)?.ReceiveEvent(new T()));
        }

        public void SendEvent<T>(in T @event)
        {
            SwapWaitTransition((m_currState as IEventReceiver<T>)?.ReceiveEvent(@event));
        }

        void IStateMachine.TickStateMachine(in float delta)
        {
            SwapWaitTransition(m_currState?.TickState(delta));
            m_waitExecuteTransition?.Excute(this);
            m_waitExecuteTransition = null;
        }

        /// <summary>
        /// 切换等待执行的状态切换
        /// </summary>
        /// <param name="transtion">切换目标</param>
        private void SwapWaitTransition(ITransition transtion)
        {   
            if (transtion != null && (m_waitExecuteTransition == null || m_waitExecuteTransition.order <= transtion.order))
            {
                m_waitExecuteTransition = transtion;
            }
        }
    }
}
