using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class Calibration : MonoBehaviour
{
    [SerializeField]
    private Camera arCamera;
    private Vector2 touchPosition = default;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private ARRaycastManager arRaycastManager;

    public GameObject calibrationPanel, welcomePanel, appPanel, worldCanvas, objectController, app;
    public GameObject[] infoPanels;
    public bool gotFirst, gotSecond, gotBoth;
    public TextMeshProUGUI calibrationText;
    public Vector3 firstPoint, secondPoint, direction;
    public Sprite rosignano_image;
    public Image calibration_image;
    private float firstHeight;

    private GameObject aRSessionOrigin;

    void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        
        arRaycastManager = GetComponent<ARRaycastManager>();
        aRSessionOrigin = GameObject.Find("AR Session Origin");
        calibrationPanel.SetActive(false);
        appPanel.SetActive(false);
        welcomePanel.SetActive(true);
        objectController.GetComponent<objectController>().languageChanger.changetoItalian_welcomePanel();
        worldCanvas.SetActive(true);
        foreach(GameObject infoPanel in infoPanels)
            infoPanel.SetActive(false);

        gotFirst = false;
        gotSecond = false;
        gotBoth = false;

        // firstPoint = new Vector3(0f, 0f, 0f);
        // secondPoint = new Vector3(0f, 0f, 1f);
        // gotBoth = true;
        // welcomePanel.SetActive(false);
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
                            gotFirst = true;
                            firstPoint = hitObject.transform.position + (new Vector3(0f, 0.06f, 0f));
                            // GameObject sphere =  GameObject.CreatePrimitive(PrimitiveType.Sphere);
                            // sphere.transform.localScale *= 0.05f;
                            // sphere.transform.position = firstPoint;
                            // sphere.AddComponent<ARAnchor>();

                            // transform.position = firstPoint;
                            calibration_image.sprite = rosignano_image;
                            // calibrationText.text="Point your camera on the picture in top left corner of the map (Rosignano emblem) and click on the 3D object that appears.";
                        }
                        if(hitObject.collider.tag=="point2" && gotFirst){
                            gotSecond = true;
                            gotBoth = true;
                            secondPoint = hitObject.transform.position + (new Vector3(0f, 0.06f, 0f));
                            // secondPoint = new Vector3(hitObject.transform.position.x, firstPoint.y, hitObject.transform.position.z) + (new Vector3(0f, 0.0f, 0f));
                            calibrateFirst();
                        }
                    }
                }
            } 
        }
    }

    public void calibrateFirst(){
        float offsetX, offsetY, height;
        // offsetX = 0.065f;
        // offsetY = 0.045f;
        offsetX = 0.04f;
        offsetY = -0.16f;
        height = 0.81f;

        direction = secondPoint - firstPoint;
        direction = new Vector3(direction.x, 0f, direction.z);

        // firstPoint += (Vector3.Normalize(new Vector3(direction.z, 0f, -direction.x))) * (direction.magnitude * 0.66f / 2f + offsetX);
        // firstPoint -= Vector3.Normalize(direction) * offsetY;

        firstPoint += (Vector3.Normalize(new Vector3(direction.z, 0f, -direction.x))) * (height * 0.66f / 2f + offsetX);
        firstPoint -= Vector3.Normalize(direction) * (offsetY + height);

        // worldCanvas.transform.position = (new Vector3(0f, worldCanvas.GetComponent<RectTransform>().sizeDelta.y/2f, direction.magnitude + offsetY*2));
        // objectController.GetComponent<objectController>().actualHeight = direction.magnitude + offsetY * 2;
        worldCanvas.transform.position = (new Vector3(0f, worldCanvas.GetComponent<RectTransform>().sizeDelta.y / 2f, height));
        objectController.GetComponent<objectController>().actualHeight = height;
        
        objectController.GetComponent<objectController>().scale = objectController.GetComponent<objectController>().actualHeight;
        // Debug.Log(direction.magnitude);
        objectController.GetComponent<objectController>().callibrateCreate();
        objectController.GetComponent<objectController>().createRectangle();
        

        transform.position = firstPoint + Vector3.Normalize(direction) * 0f + new Vector3(0f, 0f, 0f);
        // transform.position = secondPoint + Vector3.Normalize(direction) * (0.1f - objectController.GetComponent<objectController>().actualHeight) + new Vector3(0f, 0f, 0f);
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

        if (app.GetComponent<ARAnchor>() == null){
            app.AddComponent<ARAnchor>();
        }

        calibrationPanel.SetActive(false);
        appPanel.SetActive(true);
    }

    public void ok_welcome(){
        welcomePanel.SetActive(false);
        calibrationPanel.SetActive(true);
    }

    public void exit(){
        Application.Quit();
    }
}
