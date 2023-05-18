using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterCanvasesUIScreen : UIScreen
{
	[SerializeField] private CharacterUICanvas characterUIPrefab;
	[FormerlySerializedAs("canvasOffset")] [SerializeField] private Vector3 cameraWorldOffset;
	[SerializeField] private Transform canvasesRoot;

	private Dictionary<Character, CharacterUICanvas> canvasesByCharacter = new();

	private Camera currentCamera;
	
	public override UIScreenType GetUIType()
	{
		return UIScreenType.CHARACTER_CANVASES;
	}

	public override void Init(GameDataContainer loader, GameCanvas gameCanvas)
	{
		base.Init(loader, gameCanvas);
		
		currentCamera = Camera.main;
	}

	public CharacterUICanvas AddCanvasForCharacter(Character character, bool isPlayer)
	{
		var canvas = Instantiate(characterUIPrefab, canvasesRoot);

		canvas.InitCanvas(isPlayer);

		canvasesByCharacter.Add(character, canvas);

		return canvas;
	}

	public void RemoveCanvas(Character character)
	{
		Destroy(canvasesByCharacter[character].gameObject);
		canvasesByCharacter.Remove(character);
	}

	private void LateUpdate()
	{
		foreach (var item in canvasesByCharacter)
		{
			var canvasPosition = currentCamera.WorldToScreenPoint(item.Key.GetModel.Rigidbody.position + cameraWorldOffset);
			
			item.Value.RectTransform.position = canvasPosition;
		}
	}
}