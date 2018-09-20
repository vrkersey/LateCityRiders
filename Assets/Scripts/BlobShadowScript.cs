using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobShadowScript : MonoBehaviour
{

    [Header("Settings")]
    public Transform _parent;//prefab
    public Transform _parentSpawned;
    public Vector3 _parentOffset = new Vector3(0f, 0.01f, 0f);
    public LayerMask _layerMask;

    void Start()
    {
        _parentSpawned = Instantiate(_parent, transform.position, transform.rotation);
        _parentSpawned.gameObject.SetActive(true);
        _parentSpawned.SetParent(transform);
        _parent.gameObject.SetActive(false);
    }
    
    void Update()
    {
        Ray ray = new Ray(transform.position, -Vector3.up);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 100f, _layerMask))
        {
            //Debug.Log(hitInfo.transform.gameObject);
            // Position
            _parentSpawned.position = hitInfo.point + _parentOffset;

            // Rotate to same angle as ground
            _parentSpawned.up = hitInfo.normal;
        }
        else
        {
            // If raycast not hitting (air beneath feet), position it far away
            _parentSpawned.position = new Vector3(0f, 1000f, 0f);
        }
    }

}

