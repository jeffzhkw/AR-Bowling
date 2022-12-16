using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class ARTapToPlaceObject : MonoBehaviour
{
    
    private ARRaycastManager m_RaycastManager;
    static List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();

    
    public GameObject placementIndicator;
    public GameObject ballPin;

    private GameObject spawnedPin;

    private Pose placementPose;
    private bool placementPoseIsValid = false;


    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
    }


    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        if (placementPoseIsValid &&
            Input.touchCount > 0 &&
            Input.GetTouch(0).phase == TouchPhase.Began &&
            !GameManager.Instance.BasePlaced()
            )
        {
            PlaceObject();
            GameManager.Instance.startGame();
        }
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        m_RaycastManager.Raycast(screenCenter, m_Hits, TrackableType.Planes);

        placementPoseIsValid = m_Hits.Count > 0;
        if (placementPoseIsValid)
        {
            placementPose = m_Hits[0].pose;
            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }

    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid && !GameManager.Instance.BasePlaced())
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    private void PlaceObject()
    {
        spawnedPin = Instantiate(ballPin, placementPose.position, placementPose.rotation * Quaternion.Euler(0f, 180f, 0f));
    }
}
