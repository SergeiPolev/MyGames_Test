using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private CharacterModel _playerModel;
    
    public override void InstallBindings()
    {
        var playerInstance = Instantiate(_playerModel, Vector3.zero, Quaternion.identity);

        Container.BindInterfacesAndSelfTo<Player>().AsSingle().WithArguments(playerInstance);
        Container.BindInterfacesAndSelfTo<PlayerInput>().AsSingle();
    }
}