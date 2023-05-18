using DG.Tweening;
using Lean.Pool;
using UnityEngine;

public class DamageScreen : MonoBehaviour
{
	[SerializeField] private DamageText damageTextPrefab;
	[SerializeField] private float damageDuration;
	[SerializeField] private Vector3 endOffset = new(0, 200, 0);
	[SerializeField] private Vector3 appearOffset = new(0, 200, 0);
	[SerializeField] private Ease ease = Ease.InSine;

	private Camera currentCamera;
	
	private void Awake()
	{
		currentCamera = Camera.main;
	}

	public void CalculateDamageText(float value, Vector3 worldPos, bool isCrit = false)
	{
		if (value > 0)
		{
			SpawnDamage(value, worldPos, isCrit);
		}
		else
		{
			SpawnHeal(value, worldPos);
		}
	}

	private void SpawnDamage(float value, Vector3 worldPos, bool isCrit = false)
	{
		var damageText = SpawnText(worldPos);
		
		damageText.SetDamage(value, damageDuration, isCrit);
	}
	private void SpawnHeal(float value, Vector3 worldPos)
	{
		var absValue = Mathf.Abs(value);
		
		var damageText = SpawnText(worldPos);
		
		damageText.SetHeal(absValue, damageDuration);
	}

	private DamageText SpawnText(Vector3 worldPos)
	{
		Vector3 screenOffset = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
		
		var screenPosition = currentCamera.WorldToScreenPoint(worldPos);

		screenPosition += appearOffset;
		screenPosition -= screenOffset;
		
		var damageText = LeanPool.Spawn(damageTextPrefab, transform);
		damageText.transform.localPosition = screenPosition;
		
		damageText.transform.DOLocalMove(screenPosition + endOffset, damageDuration)
			.SetEase(ease)
			.OnComplete(() => LeanPool.Despawn(damageText));
		
		return damageText;
	}
}