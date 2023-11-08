using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlaceTrackedImages : MonoBehaviour
{
    
    public GameObject[] ARPrefabs;
    private readonly Dictionary<string, GameObject> _instantiatedPrefabs = new Dictionary<string, GameObject>();
    private ARTrackedImageManager _trackedImagesManager;
    public bool gotFirst, gotSecond, gotBoth;
    public GameObject app;
    public float nextActionTime = 0f, period = 3f;
    private Vector3 first_point, second_point, direction;

    void Awake()
    {
        _trackedImagesManager = GetComponent<ARTrackedImageManager>();
        gotFirst = false;
        gotSecond = false;
        gotBoth = false;
    }



    void OnEnable() 
    { 
        _trackedImagesManager.trackedImagesChanged += OnTrackedImagesChanged; 
    } 
    void OnDisable() 
    { 
        _trackedImagesManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs) 
    {
            // Go through all tracked images that have been added 
            // (-> new markers detected) 
            foreach (var trackedImage in eventArgs.added) 
            { 
                // Get the name of the reference image to search for the corresponding prefab 
                var imageName = trackedImage.referenceImage.name; 
            
                foreach (var curPrefab in ARPrefabs) 
                { 
                    //if (string.Compare(curPrefab.name, imageName, StringComparison.Ordinal) == 0 && !_instantiatedPrefabs.ContainsKey(imageName)) 
                    if (imageName == curPrefab.name && !_instantiatedPrefabs.ContainsKey(imageName)) 
                    { 
                        // Found a corresponding prefab for the reference image, and it has not been 
                        // instantiated yet > new instance, with the ARTrackedImage as parent 
                        // (so it will automatically get updated when the marker changes in real life) 
                        var newPrefab = Instantiate(curPrefab, trackedImage.transform); 
                        // Store a reference to the created prefab 
                        _instantiatedPrefabs[imageName] = newPrefab;
                    } 
                } 
            }

            // Remove is called if the subsystem has given up looking for the trackable again.
            // (If it's invisible, its tracking state would just go to limited initially).
            // Note: ARCore doesn't seem to remove these at all; if it does, it would delete our child GameObject
            // as well.
            foreach (var trackedImage in eventArgs.removed) 
            { 
                // Destroy the instance in the scene.
                // Note: this code does not delete the ARTrackedImage parent, which was created
                // by AR Foundation, is managed by it and should therefore also be deleted by AR Foundation.
                // Destroy(_instantiatedPrefabs[trackedImage.referenceImage.name]);
                // Also remove the instance from our array
                //_instantiatedPrefabs.Remove(trackedImage.referenceImage.name);

                // Alternative: do not destroy the instance, just set it inactive
                _instantiatedPrefabs[trackedImage.referenceImage.name].SetActive(false);
                
            }

            foreach (var trackedImage in eventArgs.updated) 
            { 
                _instantiatedPrefabs[trackedImage.referenceImage.name].SetActive(trackedImage.trackingState == TrackingState.Tracking);
                // if(trackedImage.referenceImage.name == "Point1" && !gotFirst){
                //     // _instantiatedPrefabs[trackedImage.referenceImage.name].SetActive(trackedImage.trackingState == TrackingState.Tracking);
                //     gotFirst = true;
                // }
                // if(trackedImage.referenceImage.name == "Point2" && !gotSecond){
                //     // _instantiatedPrefabs[trackedImage.referenceImage.name].SetActive(trackedImage.trackingState == TrackingState.Tracking);
                //     gotSecond = true;
                // }
                // _instantiatedPrefabs[trackedImage.referenceImage.name].SetActive(false);
                // if(trackedImage.referenceImage.name == "Point1")
                //     first_point = _instantiatedPrefabs[trackedImage.referenceImage.name].transform.position;
                // if(trackedImage.referenceImage.name == "Point2")
                //     second_point = _instantiatedPrefabs[trackedImage.referenceImage.name].transform.position;

                // if(trackedImage.trackingState == TrackingState.Tracking && !gotBoth){
                //     if(trackedImage.referenceImage.name == "Point1" && !gotFirst){
                //         gotFirst = true;
                //         first_point = _instantiatedPrefabs[trackedImage.referenceImage.name].transform.position + (new Vector3(0f, -0.12f, 0f));
                //     }
                //     if(trackedImage.referenceImage.name == "Point2" && !gotSecond){
                //         gotSecond = true;
                //         second_point = _instantiatedPrefabs[trackedImage.referenceImage.name].transform.position + (new Vector3(0f, -0.12f, 0f));
                //     }
                // }
            }
            
            // if(_instantiatedPrefabs.Count == 2 && !gotBoth){
            //     gotBoth = true;
            //     direction = second_point - first_point;
            //     app.GetComponent<Calibration>().calibrate(first_point, direction);
            // }

            // if(_instantiatedPrefabs.Count == 2 && gotBoth){
            //     direction = second_point - first_point;
            //     app.GetComponent<Calibration>().calibrateUpdate(first_point, direction);
            // }
    }
    

}
