using DG.Tweening;
using Lean.Pool;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour, IPoolable
{
	[SerializeField] private TextMeshProUGUI damageText;
	[SerializeField] private int damageFontSize = 46;
	[SerializeField] private int critDamageFontSize = 72;
	[SerializeField] private Color damageColor = Color.red;
	[SerializeField] private Color healColor = Color.green;

	public void SetDamage(float damage, float duration, bool isCrit = false)
	{
		damageText.fontSize = isCrit ? critDamageFontSize : damageFontSize;
		damageText.text = $"{damage: 0}";
		damageText.color = damageColor;
		damageText.DOFade(0, duration -.2f).SetDelay(.2f);
	}
	public void SetHeal(float heal, float duration)
	{
		damageText.text = $"{heal: 0}";
		damageText.color = healColor;
		damageText.DOFade(0, duration -.2f).SetDelay(.2f);
	}
	
	public void OnSpawn()
	{
		damageText.alpha = 1;
	}

	public void OnDespawn()
	{
		
	}
}