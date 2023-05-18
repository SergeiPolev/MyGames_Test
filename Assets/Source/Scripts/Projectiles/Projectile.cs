using Lean.Pool;
using UnityEngine;

public class Projectile : MonoBehaviour, IPoolable
{
    [SerializeField] private float _lifeTime = 2f;
    
    public DamageContainer DamageContainer;

    private Vector3 _lastPos;
    private Vector3 _direction;

    private int _targetLayer;
    private DamageContainer _damage;

    private float _lifeTimer;
    private float _speed;

    private bool initialised;
    public void Init(int layer, DamageContainer damage, Vector3 direction, float speed)
    {
        _targetLayer = layer;
        _damage = damage;
        _speed = speed;
        _lifeTimer = Time.time + _lifeTime;
        initialised = true;
        _lastPos = transform.position;
        _direction = direction;
    }
    private void Update()
    {
        if (!initialised)
        {
            return;
        }
        
        if (_lifeTimer <= Time.time)
        {
            LeanPool.Despawn(this);
        }

        Vector3 delta = _direction * (_speed * Time.deltaTime);
        var position = transform.position + delta;
        var ray = new Ray(_lastPos, _direction);
        _lastPos = position;

        if (Physics.Raycast(ray, out RaycastHit hit, delta.magnitude, _targetLayer))
        {
            if (hit.collider.TryGetComponent(out IDamageable result))
            {
                result.GetHit(_damage);

                LeanPool.Despawn(this);
                initialised = false;
            }
        }

        transform.position = position;
    }

    public void OnSpawn()
    {
    }

    public void OnDespawn()
    {
        initialised = false;
    }
}

public class DamageContainer
{
    public float value;
}