using System;
using System.Collections.Generic;

public enum GameState
{
	GAME = 1,
	WIN = 2,
	LOSE = 3,
}

public class GameStateMachine
{
	private IGameDataContainer _gameDataContainer;

	private GameState _currentGameState;

	private Dictionary<GameState, List<Action>> _gameStatesListeners = new();

	public GameState GetCurrentGameState => _currentGameState;

	public event Action<GameState> OnStateChanged;
	
	private const GameState StartState = GameState.GAME;
	
	public GameStateMachine(IGameDataContainer gameDataContainer)
	{
		_gameDataContainer = gameDataContainer;

		SetState(StartState);
	}

	public void SetState(GameState gameState)
	{
		_currentGameState = gameState;

		if (!_gameStatesListeners.ContainsKey(gameState))
		{
			_gameStatesListeners.Add(gameState, new List<Action>());
		}
		
		var subscribers = _gameStatesListeners[gameState];

		foreach (var item in subscribers)
		{
			item?.Invoke();
		}
		
		OnStateChanged?.Invoke(gameState);
	}

	public void SubscribeToState(GameState state, Action action)
	{
		if (!_gameStatesListeners.ContainsKey(state))
		{
			_gameStatesListeners.Add(state, new List<Action>());
		}

		_gameStatesListeners[state].Add(action);
	}
	public void UnsubscribeFromState(GameState state, Action action)
	{
		if (!_gameStatesListeners.ContainsKey(state))
		{
			_gameStatesListeners.Add(state, new List<Action>());
			
			return;
		}

		_gameStatesListeners[state].Remove(action);
	}
}