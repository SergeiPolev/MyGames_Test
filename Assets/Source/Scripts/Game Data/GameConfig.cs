using System;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/GAME CONFIG")]
public class GameConfig : ScriptableObject
{
	[field: Header("Player")]
	[field: SerializeField] public PlayerStats PlayerStats { get; set; }
	[field: SerializeField] public Vector3 PlayerIsometricOffset { get; set; }
	[field: SerializeField] public Vector3 PlayerAimOffset { get; set; }
	
	[field: Header("Enemies")]
	[field: SerializeField] public CharacterModel EnemyPrefab { get; set; }
	[field: SerializeField] public EnemiesWave[] EnemiesWaves { get; set; }
	[field: SerializeField] public EnemyStats EnemyStats { get; set; }
	
	[field: Header("Others")]
	[field: SerializeField] public Ease DamageNumbersEase { get; set; }

	[field: SerializeField] public int[] ObstacleRaycastLayers;

	public const int PLAYER_LAYER = 1 << 6;
	public const int ENEMY_LAYER = 1 << 7;
	public const int GROUND_LAYER = 1 << 9;

	public int GetObstacleLayers()
	{
		int layers = 0;
		
		foreach (var index in ObstacleRaycastLayers)
		{
			layers |= 1 << index;
		}

		return layers;
	}
}

[Serializable]
public class EnemiesWave
{
	public int Count;
}

[Serializable]
public struct EnemyStats
{
	public float Speed;
	public float Health;
	public float Damage;
	public float AttackRate;
	public float AttackRange;
	public float ProjectileForce;
	public Projectile Projectile;

	[Header("Health Settings")] 
	public HealthSettings HealthSettings;
}

[Serializable]
public struct PlayerStats
{
	public float Speed;
	public float Health;
	public float Damage;
	public float AttackRate;
	public float ProjectileForce;
	public Projectile Projectile;

	[Header("Health Settings")] 
	public HealthSettings HealthSettings;
}

[Serializable]
public struct BaseEnemyStats
{
	public float SpawnRate;
	public float Speed;
	public float Health;
	public float Damage;
	public float AttackDelay;
	public float AttackRange;
	// public Enemy EnemyPrefab;
	public AnimationClip EnemyMoveClip;
}