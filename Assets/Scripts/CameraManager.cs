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
    private Vector3 panVelocity;
    private float oldZoom;

    private void Awake()
    {
        layerMaskGround = LayerMask.GetMask("GroundLayer");
        oldZoom = mainCamera.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePan();
        UpdateZoom();
    }

    void UpdateZoom()
    {
        float newZoom = mainCamera.orthographicSize;
        //Editor
        float scrollAmount = Input.GetAxis("Mouse ScrollWheel");
        if (scrollAmount != 0)
        {
            newZoom = newZoom - scrollAmount;
        }

        if (oldZoom != newZoom)
        {
            mainCamera.orthographicSize = newZoom;
        }
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
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                touchCount = 0;
            }
            else
            {
                touchCount = Input.touchCount;
            }
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

            if (isPanningStarted)
            {
                OnScenePan(hitPoint);
            }
        }
        else
        {
            if (isPanningStarted)
            {
                isPanningStarted = false;
                OnScenePanEnded();
            }
            UpdatePanInertia();
        }
    }

    private void OnScenePanEnded()
    {
        Debug.Log($"stop _panVelocity{panVelocity}");
    }

    private void OnScenePan(Vector3 newPoint)
    {
        Vector3 delta = previoutPanPoint - newPoint;
        Debug.Log($"_previous {previoutPanPoint} evtpoint {newPoint} delta {delta}");
        mainCamera.transform.localPosition += delta;
        if(delta.magnitude > 0.1f)
            panVelocity = delta;
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

        var leftTopClampScreenPos = mainCamera.WorldToScreenPoint(CameraBoundary.instance.CameraClampTopLeftPosition);
        if (leftTopClampScreenPos.x > 0)
        {
            float deltaFactor = leftTopClampScreenPos.x * worldSizePerPixel;
            var delta = new Vector3(deltaFactor, 0, 0);
            delta = mainCamera.transform.TransformVector(delta);
            mainCamera.transform.localPosition += delta;
        }

        if (leftTopClampScreenPos.y < Screen.height)
        {
            float deltaFactor = (Screen.height - leftTopClampScreenPos.y) * worldSizePerPixel;
            var delta = new Vector3(0, deltaFactor, 0);
            delta = mainCamera.transform.TransformVector(delta);
            mainCamera.transform.localPosition -= delta;
        }

        var rightBottomClampScreenPos = mainCamera.WorldToScreenPoint(CameraBoundary.instance.CameraClampBottomRightBotPosition);
        if (rightBottomClampScreenPos.x < Screen.width)
        {
            float deltaFactor = (Screen.width - rightBottomClampScreenPos.x) * worldSizePerPixel;
            var delta = new Vector3(deltaFactor, 0, 0);
            delta = mainCamera.transform.TransformVector(delta);
            mainCamera.transform.localPosition -= delta;
        }

        if (rightBottomClampScreenPos.y > 0)
        {
            float deltaFactor = rightBottomClampScreenPos.y * worldSizePerPixel;
            var delta = new Vector3(0, deltaFactor, 0);
            delta = mainCamera.transform.TransformVector(delta);
            mainCamera.transform.localPosition += delta;
        }
    }

    public void UpdatePanInertia()
    {
        if (panVelocity.magnitude < 0.05f)
        {
            panVelocity = Vector3.zero;
        }
    
        if (panVelocity != Vector3.zero)
        {
            panVelocity = Vector3.Lerp(panVelocity, Vector3.zero, Time.deltaTime * 5);
            mainCamera.transform.localPosition += panVelocity;
            ClampCameara();
        }
    }

}
