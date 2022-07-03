using System;
using System.Collections.Generic;

namespace EasyFSM
{
    /// <summary>
    /// 基础状态类
    /// </summary>
    public abstract class AbstractState : IState, IEnterState
    {
        /// <summary>
        /// 状态转换表
        /// </summary>
        private Dictionary<Type, ITransition> m_transitionMap = new Dictionary<Type, ITransition>();

        void IState.InitState() => OnInitState();

        void IEnterState.EnterState() => OnEnterState();

        void IState.ExitState() => OnExitState();

        ITransition IState.TickState(in float delta) => OnTickState(in delta);

        /// <summary>
        /// 注册状态切换
        /// </summary>
        /// <param name="order">优先级</param>
        public void RegisterTransition<T>(int order = 0) where T : class, ITransition, new()
        {
            RegisterTransition(new T(), order);
        }

        /// <summary>
        /// 注册状态切换
        /// </summary>
        /// <param name="transition">状态切换实例</param>
        /// <param name="order">优先级</param>
        public void RegisterTransition<T>(T transition, int order = 0) where T : class, ITransition
        {
            transition.order = order;
            m_transitionMap.Add(typeof(T), transition);
        }

        /// <summary>
        /// 获取状态切换
        /// </summary>
        /// <returns>状态切换实例</returns>
        protected T GetTranstion<T>() where T : class, ITransition
        {
            if (m_transitionMap.TryGetValue(typeof(T), out var transtion))
            {
                return transtion as T;
            }
            return null;
        }

        /// <summary>
        /// 初始化状态时回调
        /// </summary>
        protected abstract void OnInitState();

        /// <summary>
        /// 进入状态时回调
        /// </summary>
        protected abstract void OnEnterState();

        /// <summary>
        /// 离开状态时回调
        /// </summary>
        protected abstract void OnExitState();

        /// <summary>
        /// 更新状态时回调
        /// </summary>
        /// <param name="delta">间隔</param>
        /// <returns>状态切换</returns>
        protected abstract ITransition OnTickState(in float delta);
    }

    /// <summary>
    /// 基础状态类
    /// </summary>
    /// <typeparam name="TParam">参数类型</typeparam>
    public abstract class AbstractState<TParam> : IState, IEnterState<TParam>
    {
        /// <summary>
        /// 状态转换表
        /// </summary>
        private Dictionary<Type, ITransition> m_transitionMap = new Dictionary<Type, ITransition>();

        void IState.InitState() => OnInitState();

        void IEnterState<TParam>.EnterState(in TParam param) => OnEnterState(in param);

        void IState.ExitState() => OnExitState();

        ITransition IState.TickState(in float delta) => OnTickState(in delta);

        /// <summary>
        /// 注册状态切换
        /// </summary>
        public void RegisterTransition<T>() where T : class, ITransition, new()
        {
            RegisterTransition(new T());
        }

        /// <summary>
        /// 注册状态切换
        /// </summary>
        /// <param name="transition">状态切换实例</param>
        public void RegisterTransition<T>(T transition) where T : class, ITransition
        {
            m_transitionMap.Add(typeof(T), transition);
        }

        /// <summary>
        /// 获取状态切换
        /// </summary>
        /// <returns>状态切换实例</returns>
        protected T GetTranstion<T>() where T : class, ITransition
        {
            if (m_transitionMap.TryGetValue(typeof(T), out var transtion))
            {
                return transtion as T;
            }
            return null;
        }

        /// <summary>
        /// 初始化状态时回调
        /// </summary>
        protected abstract void OnInitState();

        /// <summary>
        /// 进入状态时回调
        /// </summary>
        protected abstract void OnEnterState(in TParam param);

        /// <summary>
        /// 离开状态时回调
        /// </summary>
        protected abstract void OnExitState();

        /// <summary>
        /// 更新状态时回调
        /// </summary>
        /// <param name="delta">间隔</param>
        /// <returns>状态切换</returns>
        protected abstract ITransition OnTickState(in float delta);
    }
}
