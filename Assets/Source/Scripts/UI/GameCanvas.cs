using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameCanvas : MonoBehaviour
{
	[SerializeField] private UIScreenType startScreen = UIScreenType.MAIN;
	[SerializeField] private DamageScreen damageScreen;
	[SerializeField] private CanvasScaler canvasScaler;
	[SerializeField] private RectTransform canvasRect;

	private Dictionary<UIScreenType, UIScreen> uiScreens = new();

	public Canvas Canvas => _canvas;
	public CanvasScaler CanvasScaler => canvasScaler;
	public RectTransform CanvasRect => canvasRect;

	private Canvas _canvas;

	private GameDataContainer _gameDataContainer;

	[Inject]
	public void Construct(GameDataContainer gameDataContainer)
	{
		_gameDataContainer = gameDataContainer;
		_gameDataContainer.GetGameData().GameCanvas = this;
	}
	
	private void Start()
	{
		Open(startScreen);

		_canvas = GetComponent<Canvas>();
		_canvas.worldCamera = Camera.main;
	}

	[Inject]
	public void Construct()
	{
		Init();
	}

	public void Init()
	{
		InitScreens();
	}
	
	private void InitScreens()
	{
		var screens = GetComponentsInChildren<UIScreen>();

		foreach (var item in screens)
		{
			uiScreens.Add(item.GetUIType(), item);
			item.Init(_gameDataContainer,this);
		}
	}

	public T GetScreen<T>(UIScreenType type) where T : UIScreen
	{
		return uiScreens[type] as T;
	}
	public void Open(UIScreenType type)
	{
		if (uiScreens.ContainsKey(type))
		{
			uiScreens[type].Open();
			
			return;
		}

		throw new Exception($"There's no screen for type {type.GetType().Name}");
	}
	public void Close(UIScreenType type)
	{
		if (uiScreens.ContainsKey(type))
		{
			uiScreens[type].Close();
			
			return;
		}

		throw new Exception($"There's no screen for type {type.GetType()}");
	}

	/// <summary>
	/// Calculates value to set color of damage text. To heal send negative value.
	/// </summary>
	/// <param name="value">Health change value</param>
	/// <param name="worldPos">World point of character</param>
	public void ProceedDamageText(float value, Vector3 worldPos)
	{
		damageScreen.CalculateDamageText(value, worldPos);
	}
}