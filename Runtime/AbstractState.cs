namespace EasyFSM
{
    public interface IState
    {
        internal void Init();

        internal void Destroy();

        internal void Enter();

        internal void Exit();

        internal void Update(float deltaTime);
    }

    public abstract class AbstractState : IState
    {
        void IState.Init() => OnInit();

        void IState.Destroy() => OnDestroy();

        void IState.Enter() => OnEnter();

        void IState.Exit() => OnExit();

        void IState.Update(float deltaTime) => OnUpdate(deltaTime);

        protected virtual void OnInit() { }

        protected virtual void OnDestroy() { }

        protected abstract void OnEnter();

        protected abstract void OnExit();

        protected abstract void OnUpdate(float deltaTime);
    }
}
