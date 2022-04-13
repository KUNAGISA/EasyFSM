using System;
using System.Collections.Generic;

namespace EasyFSM
{
    /// <summary>
    /// 基础状态类
    /// </summary>
    public abstract class AbstractState<TState> : IState<TState> where TState : IState<TState>
    {
        /// <summary>
        /// 状态转换表
        /// 用来缓存状态所用的状态转换，减少GC
        /// </summary>
        private Dictionary<Type, ITransition<TState>> m_transitionMap = new Dictionary<Type, ITransition<TState>>();

        /// <summary>
        /// 注册状态切换
        /// </summary>
        public void RegisterTransition<T>() where T : class, ITransition<TState>, new()
        {
            RegisterTransition(new T());
        }

        /// <summary>
        /// 注册状态切换
        /// </summary>
        /// <param name="transition">状态切换实例</param>
        public void RegisterTransition<T>(T transition) where T : class, ITransition<TState>
        {
            m_transitionMap.Add(typeof(T), transition);
        }

        /// <summary>
        /// 获取状态切换
        /// </summary>
        /// <returns>状态切换实例</returns>
        protected T GetTranstion<T>() where T : class, ITransition<TState>
        {
            if (m_transitionMap.TryGetValue(typeof(T), out var transtion))
            {
                return transtion as T;
            }
            return null;
        }

        public abstract void ExitState();

        public abstract ITransition<TState> TickState(in float delta);
    }
}
