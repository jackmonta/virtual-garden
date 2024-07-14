using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Garden : MonoBehaviour
{

    public static Garden Instance { get; set; }
    public ARPlane Plane { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
