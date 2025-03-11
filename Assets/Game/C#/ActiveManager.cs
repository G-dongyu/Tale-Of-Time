using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveManager : MonoBehaviour
{
    public Transform point;
    public float radius;
    public List<ActiveItem> activeItem = new List<ActiveItem>();
    private Collider[] colliders;
    public List<ActiveItem> _currentActiveItems = new List<ActiveItem>();
    public List<ActiveItem> _lastActiveItems = new List<ActiveItem>();
    
    
    private void Start()
    {
        
    }
    
    private void Update()
    {
        DeteMine();
        PickUp();
    }

    public void DeteMine()
    {   
        colliders = Physics.OverlapSphere(point.position, radius, LayerMask.GetMask("ActiveItem"));
        _currentActiveItems.Clear();
        foreach (var collider in colliders)
        {
            ActiveItem item = collider.GetComponent<ActiveItem>();
            item?.OnAround();
            if (!_currentActiveItems.Contains(item))
            {
                _currentActiveItems.Add(item);
            }
        }
        foreach (var item in _currentActiveItems)
        {
            if (!_lastActiveItems.Contains(item))
            {
                item?.OnEnter();
            }
        }
        foreach (var item in _lastActiveItems)
        {
            if (!_currentActiveItems.Contains(item))
            {
                item?.OnExit();
            }
        }
        _lastActiveItems.Clear();
        foreach (var item in _currentActiveItems)
        {
            if (!_lastActiveItems.Contains(item))
            {
                _lastActiveItems.Add(item);
            }
        }
    }

    private void PickUp()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            float distanceMix = 10000;
            GameObject activeItem = null;
            foreach (var coll in colliders)
            {
                if (distanceMix > Vector3.Distance(coll.transform.position, point.position))
                {
                    distanceMix = Vector3.Distance(coll.transform.position, point.position);

                    activeItem = coll.gameObject;
                }
            }
            if (activeItem != null)
            {
                activeItem.GetComponent<ActiveItem>().Dete();
            }
        }
      
    }

    public void DestroyItem(GameObject a,float delayTime)
    {
        Destroy(a,delayTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(point.position, radius);
    }
}