using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemiesSpawner : ITickable, IFixedTickable, ILateTickable
{
	private int _targetCountAlive = 10;
	private int _enemiesRemaining;
	private int _currentWave;

	private float _spawnTimer;

	private GameDataContainer _data;
	private Enemy.Factory _factory;
	private Player _player;
	private Camera _camera;

	private List<Enemy> _aliveEnemies = new(); 
    
	public EnemiesSpawner(
		GameDataContainer data,
		Player target,
		Enemy.Factory enemyFactory
	)
	{
		_player = target;
		_data = data;
		_camera = Camera.main;
		_factory = enemyFactory;

		CheckForNextWave();
	}


	public void Tick()
	{
		foreach (var enemy in _aliveEnemies)
		{
			enemy.Tick();
		}
		
		if (_aliveEnemies.Count >= _targetCountAlive || _enemiesRemaining <= 0)
		{
			return;
		}

		if (Time.time >= _spawnTimer)
		{
			SpawnRandomEnemy();
		}
	}

	private void SpawnRandomEnemy()
	{
		float xPos;
		float yPos;
		float sign = Mathf.Sign(Random.Range(-1, 2));

		if (Random.Range(0, 100) > 50)
		{
			xPos = 0.5f + .6f * sign;
			yPos = Random.Range(0, 100) / 100f;
		}
		else
		{
			xPos = Random.Range(0, 100) / 100f;
			yPos = 0.5f + .6f * sign;
		}

		Vector3 point = _camera.ViewportToWorldPoint(new Vector3(xPos, yPos, 0));

		Ray ray = new Ray(point, _camera.transform.forward);

		if (Physics.Raycast(ray, out RaycastHit hit, 1000f, GameConfig.GROUND_LAYER))
		{
			var enemy = _factory.Create();
			enemy.GetModel.NavMeshAgent.Warp(hit.point);
			
			_aliveEnemies.Add(enemy);
			enemy.OnDead += OnEnemyDied;
			_enemiesRemaining--;
		}
	}

	private void OnEnemyDied(Character character)
	{
		_aliveEnemies.Remove((Enemy) character);

		if (_aliveEnemies.Count <= 0)
		{
			CheckForNextWave();
		}
	}

	private void CheckForNextWave()
	{
		if (_currentWave >= _data.GetGameConfig().EnemiesWaves.Length)
		{
			_data.GetGameData().GameStateMachine.SetState(GameState.WIN);
		}
		else
		{
			_currentWave++;
			_enemiesRemaining = _data.GetGameConfig().EnemiesWaves[_currentWave - 1].Count;
		}
	}

	public void FixedTick()
	{
		foreach (var enemy in _aliveEnemies)
		{
			enemy.FixedTick();
		}
	}

	public void LateTick()
	{
		foreach (var enemy in _aliveEnemies)
		{
			enemy.LateTick();
		}
	}
}