using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CenterofMass : MonoBehaviour
{
    public Vector3 CenterofMass2;
    public bool Awake;
    protected Rigidbody _rigidbody;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        _rigidbody.centerOfMass = CenterofMass2;
        _rigidbody.WakeUp();
        Awake = !_rigidbody.IsSleeping();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + transform.rotation * CenterofMass2,.25f);
    }
}
