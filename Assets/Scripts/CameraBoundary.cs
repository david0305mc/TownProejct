using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBoundary : MonoBehaviour
{
    public static CameraBoundary instance;

    [SerializeField] private Rect bound;
    private Vector3 cameraClampTopLeftPosition;
    public Vector3 CameraClampTopLeftPosition { get { return cameraClampTopLeftPosition; } }

    private Vector3 cameraClampBottomRightPosition;
    public Vector3 CameraClampBottomRightPosition { get { return cameraClampBottomRightPosition; } }

    public Rect Bound
    {
        get
        {
            return bound;
        }
        set
        {
            bound = value;
            //_UpdateBoundPositions();
        }
    }

    private void Awake()
    {
        instance = this;
    }


    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(this.Bound.width, this.Bound.height, 0));
    }

    private void LateUpdate()
    {
        if (transform.hasChanged)
        {
            UpdateBoundPositions();
            transform.hasChanged = false;
        }
    }

    private void UpdateBoundPositions()
    {
        Vector3 delta = transform.TransformVector(new Vector3(-Bound.width, Bound.height, 0) / 2.0f);
        cameraClampTopLeftPosition = transform.position + delta;
        cameraClampBottomRightPosition = transform.position - delta;
    }

    private void OnValidate()
    {
        UpdateBoundPositions();
    }
}
