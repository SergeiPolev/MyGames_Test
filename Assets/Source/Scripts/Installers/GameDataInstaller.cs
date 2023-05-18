using UnityEngine;
using Zenject;

public class GameDataInstaller : MonoInstaller
{
	[SerializeField] private GameConfig _gameConfig;
	
	public override void InstallBindings()
	{
		Container.Bind<GameConfig>().FromInstance(_gameConfig).AsSingle();
		Container.BindInterfacesAndSelfTo<GameDataContainer>().AsSingle();
	}
}
