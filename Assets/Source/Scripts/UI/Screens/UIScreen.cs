using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public abstract class UIScreen : MonoBehaviour
{
	[SerializeField] protected float fadeInOutDuration = .4f;
	[SerializeField] private Button[] closeButtons;
    
	protected CanvasGroup canvasGroup;
	protected GameCanvas gameCanvas;

	public abstract UIScreenType GetUIType();

	[Button("Open")]
	public virtual void Open()
	{
		canvasGroup.DOFade(0, 0);
		canvasGroup.gameObject.SetActive(true);
		canvasGroup.DOFade(1, fadeInOutDuration);
		canvasGroup.interactable = true;
		canvasGroup.blocksRaycasts = true;
	}
    
	[Button("Close")]
	public virtual void Close()
	{
		canvasGroup.interactable = false;
		canvasGroup.blocksRaycasts = false;
		canvasGroup.DOFade(0, fadeInOutDuration).OnComplete(
			() =>
			{
				canvasGroup.gameObject.SetActive(false);
			});
	}

	public virtual void Init(GameDataContainer gameDataContainer, GameCanvas gameCanvas)
	{
		canvasGroup = GetComponent<CanvasGroup>();
        
		this.gameCanvas = gameCanvas;
        
		canvasGroup.interactable = false;
		canvasGroup.blocksRaycasts = false;
		canvasGroup.DOFade(0, 0);
		canvasGroup.gameObject.SetActive(false);

		foreach (var item in closeButtons)
		{
			item.onClick.AddListener(Close);
		}
	}
}

public enum UIScreenType
{
	MAIN = 1,
	WIN = 2,
	LOSE = 3,
	CHARACTER_CANVASES
}