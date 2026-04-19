namespace Assets.Systems.StateMachineSpace
{
	public class TestMachine : StateMachine
	{
        public override State DefaultState { get; protected set; }
        public override State ActiveState { get; protected set; }
		protected override void InitializeStates() { }

		// Use this for initialization
		void Start()
		{
			
		}

		// Update is called once per frame
		void Update()
		{

		}
	}
}