using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
	[SerializeField] private Image fill;
	[SerializeField] private Image fakeFill;
	[SerializeField] private Sprite playerImage;
	[SerializeField] private Color _playerColor;
	[SerializeField] private Sprite enemyImage;
	[SerializeField] private Color _enemyColor;
	[SerializeField] private Image levelBack;

	private Tween _fakeFillTween;
	
	public virtual void UpdateBar(float value)
	{
		fill.fillAmount = value;
		
		_fakeFillTween.Kill();

		_fakeFillTween = fakeFill.DOFillAmount(value, 1f);
	}

	public void Init(bool isPlayer)
	{
		fill.sprite = isPlayer ? playerImage : enemyImage;
		fill.color = isPlayer ? _playerColor : _enemyColor;
		
		if (isPlayer)
		{
			levelBack.DOFade(0, 0);
		}
	}
}