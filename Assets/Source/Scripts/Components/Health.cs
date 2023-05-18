using System;
using DG.Tweening;
using Lean.Pool;
using UnityEngine;

public class Health
{
	private HealthSettings _settings;

	public event Action OnDead;
	public event Action Damaged;
	public event Action PointsChanged;

	public GameObject LastAttacker { get; private set; }

	public float CurrentPoints
	{
		get => currentPoints;
		private set
		{
			currentPoints = value;
			PointsChanged?.Invoke();
		}
	}

	public float MaxPoints
	{
		private set => _settings.MaxPoints = value;
		get => _settings.MaxPoints;
	}

	public bool IsFull => currentPoints >= _settings.MaxPoints;
	public bool IsDead => currentPoints <= 0;
	public bool IsInvincible => isInvincible;

	private float currentPoints;
	private Tween damageColorBlinkTween;
	private bool isInvincible;

	public Health(HealthSettings settings)
	{
		_settings = settings;
		
		CurrentPoints = MaxPoints;

		_settings.HealthCollider.enabled = true;
	}

	public void ApplyDamage(GameObject attacker, float damagePoints)
	{
		// Validate inputs
		if (damagePoints <= 0)
		{
			throw new Exception("Damage points should be positive!");
		}

		// Object is already died => return
		if (CurrentPoints <= 0)
			return;

		CurrentPoints -= damagePoints;

		LastAttacker = attacker;

		if (CurrentPoints > 0)
		{
			Spawn(_settings.HitEffect);

			if (_settings.IsBlinkOnDamage)
			{
				damageColorBlinkTween.KillTo0();

				Sequence sequence = DOTween.Sequence();

				foreach (var blinkingRenderer in _settings.BlinkingRenderers)
				{
					foreach (var material in blinkingRenderer.materials)
					{
						sequence.Join(material
							.DOColor(Color.white, _settings.IsEmissionBlink ? _settings.EmissionBlinkName : "_Color", _settings.BlinkDuration)
							.SetLoops(2, LoopType.Yoyo));
					}
				}

				damageColorBlinkTween = sequence.Play();
			}

			Damaged?.Invoke();
		}
		else
		{
			OnDead?.Invoke();

			ImmediateDeath();
		}
	}

	private void ImmediateDeath()
	{
		Spawn(_settings.DeathEffect);

		_settings.HealthCollider.enabled = false;

		if (_settings.DestroyOnDeath)
		{
			GameObject.Destroy(_settings.OriginGO.gameObject);
		}
		else
		{
			_settings.OriginGO.gameObject.SetActive(false);
		}
	}

	private void Spawn(GameObject prefab)
	{
		if (prefab == null)
			return;

		GameObject instance = LeanPool.Spawn(prefab, _settings.OriginGO.position,
			_settings.OriginGO.rotation);
		
		LeanPool.Despawn(instance.gameObject, 2f);
	}

	public void Heal(float healPoints)
	{
		if (healPoints <= 0)
			throw new Exception("Heal points should be positive!");

		CurrentPoints = Math.Min(CurrentPoints + healPoints, MaxPoints);
	}

	public void SetInvincibility(bool enable)
	{
		isInvincible = enable;
	}
	
	public void SetMaxPoints(float newMaxPoints, bool needRefill = false)
	{
		if (newMaxPoints <= 0)
			throw new Exception("Max points should be positive!");

		MaxPoints = Mathf.Round(newMaxPoints);
		PointsChanged?.Invoke();

		if (needRefill)
			CurrentPoints = MaxPoints;
	}
	
}
[Serializable]
public struct HealthSettings
{
	public float MaxPoints;
	public bool DestroyOnDeath;
	public bool IsBlinkOnDamage;
	public bool IsEmissionBlink;
	public string EmissionBlinkName;
	public float BlinkDuration;
	public GameObject HitEffect;
	public GameObject DeathEffect;
	
	[HideInInspector]
	public Transform OriginGO;
	[HideInInspector]
	public Renderer[] BlinkingRenderers;
	[HideInInspector]
	public Collider HealthCollider;
}