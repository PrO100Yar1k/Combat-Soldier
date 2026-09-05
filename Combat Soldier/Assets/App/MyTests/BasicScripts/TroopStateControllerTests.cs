using App.Scripts.Core.Troops.State_Machine.Base;
using App.Scripts.Core.Troops.State_Machine.State_Controller;
using App.Scripts.Core.Troops.Troop_Scripts;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class TroopStateControllerTests
{
    private TestableStateController _stateController;
    private static GameObject _dummyGo;

    [SetUp]
    public void SetUp()
    {
        _dummyGo = new GameObject("DummyTroopForTests");
        _dummyGo.AddComponent<TestableTroopController>();

        _stateController = new TestableStateController();
    }

    [TearDown]
    public void TearDown()
    {
        _stateController.Dispose();
        if (_dummyGo != null) Object.DestroyImmediate(_dummyGo);
    }

    [Test]
    public void SwitchState_ShouldStopCurrentStateAndStartNextState()
    {
        var stateA = new MockTroopState();
        var stateB = new AnotherMockTroopState();

        _stateController.RegisterState<MockTroopState>(stateA);
        _stateController.RegisterState<AnotherMockTroopState>(stateB);

        _stateController.SwitchState<MockTroopState>();

        Assert.IsTrue(stateA.IsStarted, "First state must launch Start()");
        Assert.IsFalse(stateA.IsStopped, "First state don't have launch Stop()");

        _stateController.SwitchState<AnotherMockTroopState>();

        Assert.IsTrue(stateA.IsStopped, "State A must launch Stop() after switching!");
        Assert.IsTrue(stateB.IsStarted, "State B must launch Start() after switching!");
        Assert.IsTrue(_stateController.CheckStateForActivity<AnotherMockTroopState>());
    }

    [Test]
    public void GetState_WhenStateRegistered_ShouldReturnCorrectState()
    {
        var mockState = new MockTroopState();
        _stateController.RegisterState<MockTroopState>(mockState);

        var result = _stateController.GetState<MockTroopState>();

        Assert.IsNotNull(result);
        Assert.AreEqual(mockState, result);
    }

    [Test]
    public void CheckStateForActivity_ShouldReturnTrueOnlyForActiveState()
    {
        var mockState = new MockTroopState();
        _stateController.RegisterState<MockTroopState>(mockState);

        _stateController.SwitchState<MockTroopState>();

        Assert.IsTrue(_stateController.CheckStateForActivity<MockTroopState>());
        Assert.IsFalse(_stateController.CheckStateForActivity<AnotherMockTroopState>());
    }

    [Test]
    public void Dispose_ShouldDisposeAllRegisteredStatesAndClearDictionary()
    {
        var stateA = new MockTroopState();
        var stateB = new AnotherMockTroopState();

        _stateController.RegisterState<MockTroopState>(stateA);
        _stateController.RegisterState<AnotherMockTroopState>(stateB);

        _stateController.Dispose();

        Assert.IsTrue(stateA.IsDisposed, "State A must be disposed!");
        Assert.IsTrue(stateB.IsDisposed, "State B must be disposed!");
        Assert.IsNull(_stateController.GetState<MockTroopState>(), "Dictionary must be empty!");
    }

    private class TestableStateController : TroopStateController
    {
        public void RegisterState<T>(TroopBaseState state) where T : TroopBaseState
        {
            _states[typeof(T)] = state;
        }
    }

    private class MockTroopState : TroopBaseState
    {
        public bool IsStarted { get; private set; }
        public bool IsStopped { get; private set; }
        public bool IsDisposed { get; private set; }

        protected override string StateIconLocation => "";

        public MockTroopState() : base(null, _dummyGo.GetComponent<TroopController>(), null, null, null) { }

        public override void OnStart() => IsStarted = true;
        public override void OnStop() => IsStopped = true;
        protected override void PlayStateAnimation() { }

        public override void Dispose()
        {
            IsDisposed = true;
            base.Dispose();
        }
    }

    private class AnotherMockTroopState : TroopBaseState
    {
        public bool IsStarted { get; private set; }
        public bool IsStopped { get; private set; }
        public bool IsDisposed { get; private set; }

        protected override string StateIconLocation => "";

        public AnotherMockTroopState() : base(null, _dummyGo.GetComponent<TroopController>(), null, null, null) { }

        public override void OnStart() => IsStarted = true;
        public override void OnStop() => IsStopped = true;

        protected override void PlayStateAnimation() { }

        public override void Dispose()
        {
            IsDisposed = true;
            base.Dispose();
        }
    }

    private class TestableTroopController : TroopController
    {
        protected override void OnEnable() { }
        protected override void OnDisable() { }

        public override void InitializeTroop() { }
    }
}