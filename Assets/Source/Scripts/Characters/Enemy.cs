using UnityEngine;
using Zenject;

public class Enemy : Character, IInitializable, ITickable, ILateTickable, IFixedTickable
{
	private PlayerInput _playerInput;
	private EnemyHitting _hitting;
	private CharacterModel _targetModel;
	
	public Enemy(
		GameDataContainer data,
		Player player
	)
	{
		_data = data;
		_targetModel = player.GetModel;
		
		Initialize();
	}

	public void Initialize()
	{
		_model = GameObject.Instantiate(_data.GetGameConfig().EnemyPrefab);
		InitCharacter(_model);
		InitCanvas();

		_movement.SetSpeed(_data.GetGameConfig().EnemyStats.Speed);

		_hitting = new EnemyHitting(_model, _targetModel, _data);
	}
	public void LateTick()
	{
		AimToTarget(_targetModel.transform.position);
	}
	public void FixedTick()
	{
		var direction = (_targetModel.transform.position - _model.transform.position).normalized;
		
		MoveTowardsFixed(direction);
	}

	protected override void InitHealth()
	{
		var healthSettings = _data.GetGameConfig().EnemyStats.HealthSettings;

		healthSettings.HealthCollider = _model.Collider;
		healthSettings.OriginGO = _model.transform;
		healthSettings.BlinkingRenderers = new[] { _model.MeshRenderer };

		_health = new Health(healthSettings);
	}
	public class Factory : PlaceholderFactory<Enemy>
	{
	}

	public void Tick()
	{
		_hitting.Tick();
	}
}

internal class EnemyHitting : ITickable
{
	private CharacterModel _owner;
	private CharacterModel _target;

	private float timer;

	private DamageContainer _damage;

	private GameDataContainer _data;

	public EnemyHitting(
		CharacterModel owner,
		CharacterModel target,
		GameDataContainer data
	)
	{
		_data = data;
		_owner = owner;
		_target = target;

		_damage = new DamageContainer();
		_damage.value = _data.GetGameConfig().EnemyStats.Damage;
	}


	public void Tick()
	{
		if (timer <= Time.time)
		{
			if (Vector3.Distance(_owner.transform.position, _target.transform.position) <=
			    _data.GetGameConfig().EnemyStats.AttackRange)
			{
				Hit();
			}
		}
	}

	private void Hit()
	{
		_target.GetHit(_damage);

		timer = Time.time + _data.GetGameConfig().EnemyStats.AttackRate;
	}
}