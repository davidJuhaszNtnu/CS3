using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class Calibration : MonoBehaviour
{
    [SerializeField]
    private Camera arCamera;
    private Vector2 touchPosition = default;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private ARRaycastManager arRaycastManager;

    public GameObject calibrationPanel, appPanel, worldCanvas, objectController, app;
    public GameObject[] infoPanels;
    public bool gotFirst, gotSecond, gotBoth;
    public TextMeshProUGUI calibrationText;
    public Vector3 firstPoint, secondPoint, direction;

    private GameObject aRSessionOrigin;

    void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        
        arRaycastManager = GetComponent<ARRaycastManager>();
        aRSessionOrigin = GameObject.Find("AR Session Origin");
        calibrationPanel.SetActive(true);
        appPanel.SetActive(false);
        worldCanvas.SetActive(true);
        foreach(GameObject infoPanel in infoPanels)
            infoPanel.SetActive(false);

        gotFirst = false;
        gotSecond = false;
        gotBoth = false;

        // firstPoint = new Vector3(0f,0f,0f);
        // secondPoint = new Vector3(0f,0f,1f);
        // gotBoth = true;

        // Debug.Log(Quaternion.Euler(-90f, 180f, 0f)*point1.transform.forward);
    }

    void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchPosition = touch.position;

            if(touch.phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(touchPosition);
                RaycastHit hitObject;

                if (Physics.Raycast(ray, out hitObject))
                {
                    if(!gotBoth){
                        if(hitObject.collider.tag=="point1" && !gotSecond){
                            gotFirst=true;
                            firstPoint=hitObject.transform.position+(new Vector3(0f, -0.00f, 0f));
                            calibrationText.text="Select the second point";
                        }
                        if(hitObject.collider.tag=="point2" && gotFirst){
                            gotSecond=true;
                            gotBoth = true;
                            secondPoint=hitObject.transform.position+(new Vector3(0f, -0.00f, 0f));
                            calibrateFirst();
                        }
                    }

                    // if(hitObject.collider.tag=="point1"){
                    //     // gotFirst = true;
                    //     firstPoint = hitObject.transform.position + (new Vector3(0f, 0f, 0f));
                    //     direction = Quaternion.Euler(0f, 90f, 0f ) * hitObject.collider.transform.forward;
                    //     Debug.Log(direction);
                    //     if(!gotFirst)
                    //         calibrate();
                    //     else calibrateUpdate();
                    // }
                }
            } 
        }
    }

    public void calibrate(){
        // direction = Vector3.Normalize(second - first);

        objectController.GetComponent<objectController>().actualHeight = 0.6f;
        worldCanvas.transform.position = (new Vector3(0f, worldCanvas.GetComponent<RectTransform>().sizeDelta.y/2f, objectController.GetComponent<objectController>().actualHeight));
        objectController.GetComponent<objectController>().callibrateCreate();
        objectController.GetComponent<objectController>().createRectangle();

        // transform.position = firstPoint;
        // transform.rotation = Quaternion.LookRotation(new Vector3(dir.x,0f,dir.z), Vector3.up);

        // if (app.GetComponent<ARAnchor>() == null){
        //     app.AddComponent<ARAnchor>();
        // }

        calibrationPanel.SetActive(false);
        appPanel.SetActive(true);
    }

    public void calibrateFirst(){
        // var aRScript = aRSessionOrigin.GetComponent<ARSessionOrigin>();
        direction=secondPoint-firstPoint;

        // aRScript.MakeContentAppearAt(transform, firstPoint, Quaternion.LookRotation(new Vector3(direction.x,0f,direction.z), Vector3.up));
        worldCanvas.transform.position = (new Vector3(0f, worldCanvas.GetComponent<RectTransform>().sizeDelta.y/2f, direction.magnitude));
        objectController.GetComponent<objectController>().actualHeight = direction.magnitude;
        objectController.GetComponent<objectController>().callibrateCreate();
        objectController.GetComponent<objectController>().createRectangle();

        transform.position = firstPoint;
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

        if (app.GetComponent<ARAnchor>() == null){
            app.AddComponent<ARAnchor>();
        }
        
        calibrationPanel.SetActive(false);
        appPanel.SetActive(true);
    }

    public void calibrateUpdate(Vector3 point, Vector3 dir){
        // direction = second-first;

        transform.position = point;
        transform.rotation = Quaternion.LookRotation(new Vector3(dir.x,0f,dir.z), Vector3.up);
        // worldCanvas.transform.position = (new Vector3(0f, worldCanvas.GetComponent<RectTransform>().sizeDelta.y/2f, direction.magnitude));
    }
}
