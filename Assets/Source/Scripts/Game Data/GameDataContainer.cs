using Zenject;

public class GameDataContainer : IGameDataContainer, ITickable
{
	private GameConfig gameConfig;
	private GameData gameData;
	
	public GameDataContainer(
		GameConfig gameConfig
	)
	{
		this.gameConfig = gameConfig;
		gameData = new GameData(this);
	}

	public GameConfig GetGameConfig() => gameConfig;
	public GameData GetGameData() => gameData;
	public void Tick()
	{
		gameData.Tick();
	}
}

public interface IGameDataContainer
{
	public GameConfig GetGameConfig();
	public GameData GetGameData();
}
