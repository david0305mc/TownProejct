using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    [SerializeField] private Camera mainCamera;
    private int layerMaskGround;
    private bool isPanningStarted;

    private static Vector3 PositiveInfinityVector = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
    private int previousTouchCount;
    private Vector3 previoutPanPoint;

    private void Awake()
    {
        layerMaskGround = LayerMask.GetMask("GroundLayer");
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePan();
    }

            
    private void UpdatePan()
    {
        int touchCount;
        bool isInEditor = false;
        bool touchCountChanged = false;
        
        bool canPan = false;
        Vector2 touchPosition;

        if (Input.touchCount == 0)
        {
            if ((Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)))
            {
                touchCount = 1;
                isInEditor = true;
            }
            else
            {
                touchCount = 0;
            }
        }
        else
        {
            touchCount = Input.touchCount;
            //if (Input.GetTouch(0).phase == TouchPhase.Ended)
            //{
            //    touchCount = 0;
            //}
        }
        
        if (touchCount != previousTouchCount)
        {
            touchCountChanged = true;
        }

        if (isInEditor)
        {
            touchPosition = Input.mousePosition;
        }
        else
        {
            if (touchCount > 0)
            {
                if (touchCount == 1)
                {
                    touchPosition = Input.GetTouch(0).position;
                }
                else
                {
                    touchPosition = (Input.GetTouch(0).position + Input.GetTouch(1).position) / 2.0f;
                }
            }
            else
            {
                touchPosition = Vector2.zero;
            }
        }

        
        canPan = touchCount > 0;
        previousTouchCount = touchCount;

        if (canPan)
        {
            Vector3 hitPoint = TryGetRaycastHitBaseGround(touchPosition);
            if (!isPanningStarted)
            {
                isPanningStarted = true;
                previoutPanPoint = hitPoint;
            }
            else
            {
                if (isPanningStarted)
                {
                    OnScenePan(hitPoint);
                }
            }
        }
        else
        {
            if (touchCountChanged)
            {
                OnScenePanEnded();
            }
        }
    }

    private void OnScenePanEnded()
    {
        isPanningStarted = false;
    }

    private void OnScenePan(Vector3 newPoint)
    {
        Vector3 delta = previoutPanPoint - newPoint;
        mainCamera.transform.localPosition += delta;
        ClampCameara();
    }

    private Vector3 TryGetRaycastHitBaseGround(Vector2 touch)
    {
        RaycastHit hit;
        var ray = mainCamera.ScreenPointToRay(touch);

        if (Physics.Raycast(ray, out hit, 1000, layerMaskGround))
        {
            return hit.point;
        }
        else
        {
            return PositiveInfinityVector;
        }
    }

    private void ClampCameara()
    {
        float worldSizePerPixel = 2 * mainCamera.orthographicSize / (float)Screen.height;

        var leftClampScreenPos = mainCamera.WorldToScreenPoint(CameraBoundary.instance.CameraClampTopLeftPosition);
        if (leftClampScreenPos.x > 0)
        {
            float deltaFactor = leftClampScreenPos.x * worldSizePerPixel;
            var delta = new Vector3(deltaFactor, 0, 0);
            //delta = mainCamera.transform.TransformVector(delta);
            mainCamera.transform.localPosition += delta;
        }
    }

}
