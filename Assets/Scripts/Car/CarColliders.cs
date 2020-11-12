using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarColliders : MonoBehaviour
{
    [SerializeField] private Car car;

    [SerializeField] private List<Collider> colliders;
    [SerializeField] private Collider headCollider;
    [SerializeField] private Collider tailCollider;
    [SerializeField] private Collider bodyCollider;
    [SerializeField] private List<Collider> roofColliders;

    // Start is called before the first frame update
    void Awake()
    {
        colliders.AddRange(GetComponentsInChildren<Collider>());
        headCollider = transform.Find("head").gameObject.GetComponent<Collider>();
        tailCollider = transform.Find("tail").gameObject.GetComponent<Collider>();
        bodyCollider = transform.Find("body").gameObject.GetComponent<Collider>();
        roofColliders.AddRange(transform.Find("roof").gameObject.GetComponents<Collider>());
        car = GetComponentInParent<Car>();
    }
    public Transform GetHeadCollider()
    {
        return headCollider.transform;
    }
    public Transform GetTailCollider()
    {
        return tailCollider.transform;
    }
}

