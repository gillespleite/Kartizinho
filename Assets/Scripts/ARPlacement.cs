using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class ARPlacement : MonoBehaviour
{
   
    public GameObject arObjectToSpawn;
    public GameObject placementIndicator;
    private GameObject spawnedObject;
    private Pose PlacementPose;
    private ARRaycastManager aRRaycastManager;
    private bool placementPoseIsValid = false;
    private bool adicionar;
    public GameObject Cameraa;
    private Transform targetPosition;
    // Start is called before the first frame update
    void Start()
    {
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();
        adicionar = false;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = new Vector3(Cameraa.transform.position.x, transform.position.y, Cameraa.transform.position.z);
        if(placementPoseIsValid && Input.touchCount> 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            PinPong();
           
        }
        

        UpdatePlacementPose();
        UpdatePlacementIndicator();
        if (spawnedObject != null)
        {
            spawnedObject.transform.LookAt(new Vector3(Cameraa.transform.position.x,spawnedObject.transform.position.y, Cameraa.transform.position.z));
        }
    }

    void UpdatePlacementIndicator()
    {
        if(spawnedObject == null && placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(PlacementPose.position, PlacementPose.rotation);

        }
        else
        {
            placementIndicator.SetActive(false);

        }
    }

    void PinPong()
    {
        if (!adicionar)
        {
            ARPlaceObject();
            adicionar = true;
        }
        else
        {
            adicionar = false;
            Destroy(spawnedObject);
            placementIndicator.SetActive(true);
        }

    }

    void UpdatePlacementPose()
    {
       
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        aRRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            PlacementPose = hits[0].pose;
        }

    }

    void ARPlaceObject()
    {
        
        spawnedObject = Instantiate(arObjectToSpawn, PlacementPose.position, Quaternion.Euler(PlacementPose.rotation.x, PlacementPose.rotation.y+180, PlacementPose.rotation.z));
        

    }
}
