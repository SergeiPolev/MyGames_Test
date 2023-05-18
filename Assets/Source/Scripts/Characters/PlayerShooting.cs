using Unity.VisualScripting;
using UnityEngine;

public class PlayerShooting
{
	private PlayerInput _input;
	private Player _player;
	private GameDataContainer _data;

	private float _timer;
	
	public PlayerShooting(
		PlayerInput input,
		Player player,
		GameDataContainer data)
	{
		_input = input;
		_player = player;
		_data = data;
		
		_input.OnMouseHold += TryToShoot;
	}

	private void TryToShoot()
	{
		if (Time.time >= _timer)
		{
			Shoot();
			_timer = Time.time + _data.GetGameConfig().PlayerStats.AttackRate;
		}
	}

	private void Shoot()
	{
		CharacterModel playerModel = _player.GetModel;
		
		var projectile = Lean.Pool.LeanPool.Spawn(_data.GetGameConfig().PlayerStats.Projectile, playerModel.ShootPoint.position, Quaternion.identity);

		projectile.transform.position = playerModel.ShootPoint.position;
		Vector3 direction = playerModel.ShootPoint.forward;

		var damageContainer = new DamageContainer();
		damageContainer.value = _data.GetGameConfig().PlayerStats.Damage;
		
		projectile.Init(GameConfig.ENEMY_LAYER, damageContainer, direction, _data.GetGameConfig().PlayerStats.ProjectileForce);
		
		playerModel.AnimateShot();
	}

	public void UnsubscribeShoot()
	{
		_input.OnMouseHold -= TryToShoot;
	}
}