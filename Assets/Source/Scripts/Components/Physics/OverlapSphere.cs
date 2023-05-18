using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class OverlapSphere : MonoBehaviour
{
    [SerializeField] private LayerMask layer;
    [SerializeField] private int framesSkipBetween = 3;
    [SerializeField] private int collidersCapacity = 100;
    [SerializeField] private bool autoCastInUpdate = true;
    
    public Collider NearestTouched(Transform to) => HasTouch ? to.Closest(AllTouched) : null;
    public T NearestTouchedOfType<T>(Transform to) where T: Component => HasTouch ? to.ClosestOfType<T>(AllTouched) : null;
    public List<Collider> AllTouched { get; private set; }
    public Collider Touched { get; private set; }
    public bool HasTouch { get; private set; }

    private Collider[] colliders;

    private bool isSphere;
    private bool isInited;
    private float radius;
    private Vector3 offset;

    private SphereCollider sphere;

    public event Action OnCast;

    public void SetRadius(float value)
    {
        radius = value;
        isInited = true;
    }

    private void OnDrawGizmos()
    {
        if (isSphere)
        {
            Color color = Touched ? Color.green : Color.red;
            color.a = .4f;
            Gizmos.color = color;
            
            Transform t = transform;
            var p = t.TransformPoint(offset);
            var r = radius * t.localScale.x;
            Gizmos.DrawSphere(p, r);
        }
    }

    private void Start()
    {
        colliders = new Collider[collidersCapacity];
        AllTouched = new List<Collider>();
        
        sphere = GetComponent<SphereCollider>();
        if (!ReferenceEquals(sphere, null))
        {
            isSphere = true;

            if (!isInited)
            {
                radius = sphere.radius;
                offset = sphere.center;
            }

            sphere.enabled = false;
        }
    }

    private void Update()
    {
        if (!autoCastInUpdate)
            return;

        if (Time.frameCount % framesSkipBetween == 0)
            Cast();
    }

    public void Cast()
    {
        Array.Clear(colliders,0, collidersCapacity);

        Transform tr = transform;
        var p = tr.TransformPoint(offset);
        var r = radius * tr.localScale.x;
        Physics.OverlapSphereNonAlloc(p, r, colliders, layer.value);
        
        HasTouch = false;
        Touched = null;

        AllTouched.Clear();
        
        foreach (var item in colliders)
        {
            if (!ReferenceEquals(item, null) && item.gameObject != gameObject)
            {
                if (ReferenceEquals(Touched, null))
                {
                    Touched = item;
                    HasTouch = true;
                }
                
                AllTouched.Add(item);
            }
        }
        
        OnCast?.Invoke();
    }
}