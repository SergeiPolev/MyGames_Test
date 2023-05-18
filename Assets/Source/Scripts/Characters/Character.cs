using System;
using UnityEngine;
using UnityEngine.AI;

// Base class for all playable and non-playable characters
public abstract class Character : IDamageable
{
	protected Rigidbody _rb;
	protected NavMeshAgent _navMesh;
	protected Animator _animator;
	protected CharacterMovement _movement;
	protected CharacterAiming _aiming;
	protected CharacterModel _model;
	protected Health _health;
	protected CharacterUICanvas characterUI;

	protected GameDataContainer _data;
	
	public Health GetHealth => _health;
	public CharacterModel GetModel => _model;
	public CharacterMovement GetMovement => _movement;
	
	public event Action<Character> OnDead;
	
	
	protected virtual void InitCharacter(CharacterModel model)
	{
		InitHealth();
		
		_movement = new CharacterMovement(model.Rigidbody, model.NavMeshAgent);
		_aiming = new CharacterAiming(model);
		
		_health.OnDead += () => OnDead?.Invoke(this);

		_model.OnDamaged += GetHit;
	}

	protected abstract void InitHealth();

	public virtual void MoveTowards(Vector3 direction)
	{
		_movement.SetDirection(direction);
		_movement.Movement();
		
		if (direction != Vector3.zero)
		{
			StartMoving();
		}
		
		AimToTarget(_model.transform.position + direction);
	}
	public virtual void MoveTowardsFixed(Vector3 direction)
	{
		_movement.SetDirection(direction);
		_movement.MovementFixed();
		
		if (direction != Vector3.zero)
		{
			StartMoving();
		}
	}
	public void StartMoving()
	{
		//_animancer.Play(_loader.GetGameConfig().RunAnimation);
	}

	public virtual void AimToTarget(Vector3 target)
	{
		_aiming.Aim(target);
	}
	public virtual void StopMoving()
	{
		//_animancer.Play(_loader.GetGameConfig().IdleAnimation, .25f);
		StopNavMesh();
	}

	public void StopNavMesh()
	{
		_navMesh.velocity = Vector3.zero;

		if (_navMesh.isOnNavMesh)
		{
			_navMesh.ResetPath();
		}
	}
	public virtual void InitCanvas()
	{
		var uiScreen = _data.GetGameData().GameCanvas.GetScreen<CharacterCanvasesUIScreen>(UIScreenType.CHARACTER_CANVASES);
		characterUI = uiScreen
			.AddCanvasForCharacter(this, this is Player);
		
		if (!ReferenceEquals(characterUI, null))
		{
			_health.PointsChanged += UpdateHealthBar;
			_health.OnDead += () => uiScreen.RemoveCanvas(this);
			UpdateHealthBar();
		}
	}
	protected void UpdateHealthBar()
	{
		characterUI.UpdateHealthBar(_health.CurrentPoints, _health.MaxPoints);
	}

	public void GetHit(DamageContainer damageContainer)
	{
		_health.ApplyDamage(null, damageContainer.value);
	}
}