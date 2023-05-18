using UnityEngine;
using UnityEngine.AI;

public class CharacterMovement
{
    private readonly Rigidbody rb;
    private readonly NavMeshAgent navMesh;
    private float speed;
    private Vector3 direction;
    
    public CharacterMovement(Rigidbody rb, NavMeshAgent navMeshAgent)
    {
        this.rb = rb;
        navMesh = navMeshAgent;
    }
    public void SetSpeed(float value)
    {
        speed = value;
        navMesh.speed = speed;
    }
    
    public void SetDirection(Vector3 dir)
    {
        direction = dir;
        rb.angularVelocity = Vector3.zero;
    }

    public void Movement()
    {
        if (direction == Vector3.zero)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            
            return;
        }
        
        var calculatedMovement = direction.normalized * speed;
        rb.isKinematic = true;
        /*rb.velocity = calculatedMovement;*/
        navMesh.Move(calculatedMovement * Time.deltaTime);
    }
    public void MovementFixed()
    {
        if (direction == Vector3.zero)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            
            return;
        }
        
        var calculatedMovement = direction.normalized * speed;
        rb.isKinematic = true;
        /*rb.velocity = calculatedMovement;*/
        navMesh.Move(calculatedMovement * Time.fixedDeltaTime);
    }
    public void SetDestinationPoint(Vector3 point)
    {
        rb.isKinematic = false;
        navMesh.SetDestination(point);
    }
}