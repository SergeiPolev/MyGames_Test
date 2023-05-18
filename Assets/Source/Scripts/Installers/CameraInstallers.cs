using Cinemachine;
using UnityEngine;
using Zenject;

public class CameraInstallers : MonoInstaller
{
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<CameraControl>()
            .AsSingle()
            .WithArguments(_virtualCamera);
    }
}