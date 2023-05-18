using Zenject;

public class GameData : ITickable
{
	public Player Player { get; set; }
	public GameCanvas GameCanvas { get; set; }
	public GameStateMachine GameStateMachine { get; set; }

	public GameData(IGameDataContainer gameDataContainer)
	{
		GameStateMachine = new GameStateMachine(gameDataContainer);
	}

	public void Tick()
	{
		
	}
}