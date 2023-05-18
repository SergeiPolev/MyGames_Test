using UnityEngine;
using Zenject;

public class Player : Character, IInitializable, ILateTickable, IFixedTickable
{
	private PlayerInput _playerInput;
	private PlayerShooting _shooting;
	private GameStateMachine _stateMachine;

	private bool IsPaused;
	
	public Player(
		CharacterModel model,
		GameDataContainer data,
		PlayerInput input
		)
	{
		_model = model;
		_data = data;
		_playerInput = input;
		_stateMachine = _data.GetGameData().GameStateMachine;
	}

	public void Initialize()
	{
		InitCharacter(_model);
		InitCanvas();

		_movement.SetSpeed(_data.GetGameConfig().PlayerStats.Speed);
		_shooting = new PlayerShooting(_playerInput, this, _data);
	}
	public void LateTick()
	{
		if (IsPaused)
		{
			return;
		}
		
		AimToTarget(_playerInput.AimPoint);
	}
	public void FixedTick()
	{
		if (IsPaused)
		{
			return;
		}
		
		MoveTowardsFixed(_playerInput.AxisInput);
	}

	protected override void InitHealth()
	{
		var healthSettings = _data.GetGameConfig().PlayerStats.HealthSettings;

		healthSettings.HealthCollider = _model.Collider;
		healthSettings.OriginGO = _model.transform;
		healthSettings.BlinkingRenderers = new[] { _model.MeshRenderer };

		_health = new Health(healthSettings);

		_health.OnDead += () => KillPlayer();
	}

	private void KillPlayer()
	{
		_shooting.UnsubscribeShoot();
		IsPaused = true;
		_stateMachine.SetState(GameState.LOSE);
	}
}