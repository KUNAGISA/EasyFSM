using System;
using System.Collections.Generic;

namespace EasyFSM
{
    public struct SwitchStateScope : IDisposable
    {
        private Type m_type;
        private IState m_state;
        private IStateMachine m_stateMachine;

        public SwitchStateScope(Type type, IState state, IStateMachine stateMachine)
        {
            m_type = type; m_state = state; m_stateMachine = stateMachine; 
        }

        void IDisposable.Dispose()
        {
            m_stateMachine?.SetState(m_type, m_state);
            m_stateMachine = null; m_state = null; m_type = null;
        }
    }

    public interface IStateMachine
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
        SwitchStateScope SwitchState<TState>(out TState state) where TState : IState;

        /// <summary>
        /// 切换状态
        /// </summary>
        void SwitchState<TState>() where TState : IState;

        void Update(float deltaTime);
    }

    public class StateMachine : IStateMachine
    {
        private Dictionary<Type, IState> m_registerStateMap = new Dictionary<Type, IState>();
        private Dictionary<Type, Action> m_bindTransitionMap = new Dictionary<Type, Action>();

        private IState m_currState = null;
        private Action m_transition = null;

        public void Register<TState>(TState state, Action onTransitionDecide = null) where TState : IState
        {
            if (m_registerStateMap.TryGetValue(typeof(TState), out var old))
            {
                old.Destroy();
            }

            m_registerStateMap[typeof(TState)] = state;

            if (onTransitionDecide != null)
            {
                m_bindTransitionMap[typeof(TState)] = onTransitionDecide;
            }

            state.Init();
        }

        public SwitchStateScope SwitchState<TState>(out TState state) where TState : IState
        {
            var type = typeof(TState);
            if (!m_registerStateMap.TryGetValue(type, out var newState))
            {
                throw new Exception($"not register state {type}");
            }


            state = (TState)newState;
            return new SwitchStateScope(type, state, this);
        }

        public void SwitchState<TState>() where TState : IState
        {
            var type = typeof(TState);
            if (!m_registerStateMap.TryGetValue(type, out var newState))
            {
                throw new Exception($"not register state {type}");
            }
            ((IStateMachine)this).SetState(type, newState);
        }

        public void Update(float deltaTime)
        {
            m_transition?.Invoke();
            m_currState?.Update(deltaTime);
        }

        void IStateMachine.SetState(Type type, IState state)
        {
            m_transition = null;
            m_bindTransitionMap.TryGetValue(type, out m_transition);

            m_currState?.Exit();
            m_currState = state;
            m_currState.Enter();
        }
    }
}
