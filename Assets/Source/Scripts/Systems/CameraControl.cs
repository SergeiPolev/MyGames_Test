using Cinemachine;
using Zenject;

public class CameraControl : IInitializable
{
	private CinemachineVirtualCamera _cvCamera;
    
	[Inject]
	private Player _player;

	public CameraControl(
		CinemachineVirtualCamera cvCamera
	)
	{
		_cvCamera = cvCamera;
	}

	public void Initialize()
	{
		_cvCamera.m_Follow = _player.GetModel.transform;
		_cvCamera.m_LookAt = _player.GetModel.transform;
	}
}