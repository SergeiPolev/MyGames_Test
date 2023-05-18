using System;
using System.Linq;
using UnityEngine;

public class CharacterTarget
{
	private Transform currentOverlapTarget;
	private Collider currentTarget;
	private Transform ownerTransform;
	
	private OverlapSphere targetOverlap;
	
	public Transform CurrentTarget => currentTarget.transform;
	public bool HasTarget => targetOverlap.HasTouch;

	public event Action OnCast; 

	public CharacterTarget(OverlapSphere overlap, Transform owner)
	{
		targetOverlap = overlap;
		ownerTransform = owner;
		targetOverlap.OnCast += UpdateCast;
	}

	public void SetRadius(float value) => targetOverlap.SetRadius(value);
	public void CastTarget()
	{
		targetOverlap.Cast();
	}
	private void UpdateCast()
	{
		currentTarget = targetOverlap.NearestTouched(ownerTransform);
		OnCast?.Invoke();
	}

	public Collider GetTargetManually()
	{
		var modelForward = ownerTransform.forward;
		
		var targets = targetOverlap.AllTouched.
			Where(x => x != null && !x.GetComponent<Character>().GetHealth.IsDead).
			OrderByDescending(x =>
		{
			Vector3 direction = (x.transform.position - ownerTransform.position).normalized;
			return Vector3.Dot(modelForward, direction);
		})
			.ToArray();

		return targets.Length > 0 ? targets[0] : null;
	}
}