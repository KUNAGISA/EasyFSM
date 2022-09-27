using System;
using System.Collections.Generic;

namespace EasyFSM
{
    public struct SwitchStateScope<TState> : IDisposable where TState : class, IState
    {
        private Type m_type;
        private IState m_state;
        private IStateMachine<TState> m_stateMachine;

        public SwitchStateScope(Type type, IState state, IStateMachine<TState> stateMachine)
        {
            m_type = type; m_state = state; m_stateMachine = stateMachine; 
        }

        void IDisposable.Dispose()
        {
            m_stateMachine?.SetState(m_type, m_state);
            m_stateMachine = null; m_state = null; m_type = null;
        }
    }

    public interface IStateMachine<TState> where TState : class, IState
    {
        internal void SetState(Type type, IState state);

        /// <summary>
        /// 设置数据后切换状态
        /// <code>
        /// using (IStateMachine.SwitchState<State>(out var state))
        /// {
        ///     ///... init state param
        /// }
        /// </code>
        /// </summary>
        SwitchStateScope<TState> SwitchState<T>(out T state) where T : TState;

        /// <summary>
        /// 切换状态
        /// </summary>
        void SwitchState<T>() where T : TState;

        void Update(float deltaTime);
    }

    public class StateMachine<TState> : IStateMachine<TState> where TState : class, IState
    {
        private Dictionary<Type, IState> m_registerStateMap = new Dictionary<Type, IState>();
        private Dictionary<Type, Action> m_bindTransitionMap = new Dictionary<Type, Action>();

        private TState m_currState = null;
        private Action m_transition = null;

        public TState currState => m_currState;

        public void Register<T>(T state, Action onTransitionDecide = null) where T : TState
        {
            var type = typeof(T);
            if (m_registerStateMap.TryGetValue(type, out var old))
            {
                old.Destroy();
            }

            m_registerStateMap[type] = state;
            OnInitState(state);

            if (onTransitionDecide != null)
            {
                m_bindTransitionMap[type] = onTransitionDecide;
            }

            state.Init();
        }

        public SwitchStateScope<TState> SwitchState<T>(out T state) where T : TState
        {
            var type = typeof(T);
            if (!m_registerStateMap.TryGetValue(type, out var newState))
            {
                throw new Exception($"not register state {type}");
            }

            state = (T)newState;
            return new SwitchStateScope<TState>(type, state, this);
        }

        public void SwitchState<T>() where T : TState
        {
            var type = typeof(T);
            if (!m_registerStateMap.TryGetValue(type, out var newState))
            {
                throw new Exception($"not register state {type}");
            }
            SetState(type, newState);
        }

        public virtual void Update(float deltaTime)
        {
            m_transition?.Invoke();
            m_currState?.Update(deltaTime);
        }

        void IStateMachine<TState>.SetState(Type type, IState state) => SetState(type, state);

        private void SetState(Type type, IState state)
        {
            m_transition = null;
            m_bindTransitionMap.TryGetValue(type, out m_transition);

            m_currState?.Exit();
            m_currState = (TState)state;
            m_currState.Enter();
        }

        protected virtual void OnInitState(TState state) { }
    }
}
