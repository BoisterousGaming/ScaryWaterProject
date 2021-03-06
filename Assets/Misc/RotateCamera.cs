﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour 
{
    public Transform target;
    public float angularSpeed;

    [SerializeField]
    Vector3 initialOffset;
    Vector3 currentOffset;

    [ContextMenu("Set Current Offset")]
    private void SetCurrentOffset()
    {
        if (target == null)
            return;

        initialOffset = transform.position - target.position;
    }

    private void Start()
    {
        if (target == null)
            Debug.LogError("Assign a target for the camera in Unity's inspector");

        currentOffset = initialOffset;
    }

    private void LateUpdate()
    {
        transform.position = target.position + currentOffset;

        float movement = Input.GetAxis("Horizontal") * angularSpeed * Time.deltaTime;
        if (!Mathf.Approximately(movement, 0f))
        {
            transform.RotateAround(target.position, Vector3.up, movement);
            currentOffset = transform.position - target.position;
        }
    }
}
