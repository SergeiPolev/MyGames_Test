using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Collider))]
public class CharacterModel : MonoBehaviour, IDamageable
{
	[SerializeField] private Rigidbody _rigidbody;
	[SerializeField] private MeshRenderer _meshRenderer;
	[SerializeField] private Collider _collider;
	[SerializeField] private NavMeshAgent _navMeshAgent;
	[SerializeField] private Transform _shootPoint;

	public Rigidbody Rigidbody => _rigidbody;
	public MeshRenderer MeshRenderer => _meshRenderer;
	public Collider Collider => _collider;
	public NavMeshAgent NavMeshAgent => _navMeshAgent;
	public Transform ShootPoint => _shootPoint;

	public event Action<DamageContainer> OnDamaged;

	private Tween _shotTween;
	
	public void GetHit(DamageContainer damageContainer)
	{
		OnDamaged?.Invoke(damageContainer);
	}

	public void AnimateShot()
	{
		_shotTween.KillTo0();
		
		_shotTween = _shootPoint.DOLocalMoveZ(_shootPoint.localPosition.z - .2f, .05f).SetLoops(2, LoopType.Yoyo);
	}
}