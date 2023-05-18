using System;
using UnityEngine;
using Zenject;

public class PlayerInput : IInitializable, ITickable
{
	private Vector3 _input;
	private Vector3 _aimPoint;
	
	private Plane _plane;
	private Camera _camera;

	private GameDataContainer _data;
	private Matrix4x4 _isoMatrix;
	
	public Vector3 AxisInput => _input;
	public Vector3 AimPoint => _aimPoint;

	public event Action OnMouseHold;


	public PlayerInput(GameDataContainer data)
	{
		_data = data;

		_isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(_data.GetGameConfig().PlayerIsometricOffset));
	}
	
	public void Initialize()
	{
		_plane = new Plane(Vector3.up, _data.GetGameConfig().PlayerAimOffset);

		_camera = Camera.main;
	}
	
	public void Tick()
	{
		Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		_input = _isoMatrix.MultiplyPoint3x4(input);

		var ray = _camera.ScreenPointToRay(Input.mousePosition);
		if (_plane.Raycast(ray, out float hit))
		{
			_aimPoint = ray.GetPoint(hit);
		}

		if (Input.GetMouseButton(0))
		{
			OnMouseHold?.Invoke();
		}
	}
}