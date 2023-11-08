using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class gameController : MonoBehaviour
{
    public delegate void NewTriggerPoints(List<Vector2> triggerPoints);
    public static event NewTriggerPoints OnTrigger1Points = null;
    public static event NewTriggerPoints OnTrigger2Points = null;
    public static event NewTriggerPoints OnTrigger3Points = null;
    public static event NewTriggerPoints OnTrigger4Points = null;
    private Camera mCamera = null;

    public MeasureDepth mMeasureDepth;
    public List<ValidPoint> mValidPoints = null;

    private List<Vector2> mTrigger1Points = null;
    private List<Vector2> mTrigger2Points = null;
    private List<Vector2> mTrigger3Points = null;
    private List<Vector2> mTrigger4Points = null;

    public bool showObject1, showObject2, showObject3, showObject4;

    [Header("Trigger 1 lake sensitivity and depth")]
    [Range(0, 1f)]
    public float mTrigger1Sensitivity;
    public float mTrigger1Depth;
    public GameObject trigger1Action;
    [Header("Trigger 2 wwtp sensitivity and depth")]
    [Range(0, 0.1f)]
    public float mTrigger2Sensitivity;
    public float mTrigger2Depth;
    public GameObject trigger2Action;
    [Header("Trigger 3 house sensitivity and depth")]
    [Range(0, 0.1f)]
    public float mTrigger3Sensitivity;
    public float mTrigger3Depth;
    public GameObject trigger3Action;
    [Header("Trigger 4 island sensitivity and depth")]
    [Range(0, 0.1f)]
    public float mTrigger4Sensitivity;
    public float mTrigger4Depth;
    public GameObject trigger4Action;

    public GameObject trig1_positiveAction, trig1_negativeAction, trig2_positiveAction, trig2_negativeAction, trig3_positiveAction, trig3_negativeAction, trig4_positiveAction, trig4_negativeAction;

    //game assets
    

    void Start()
    {
        trigger1Action.SetActive(true);
        trigger2Action.SetActive(true);
        trigger3Action.SetActive(true);
        trigger4Action.SetActive(true);
        trig1_positiveAction = trigger1Action.transform.GetChild(0).gameObject;
        trig1_negativeAction = trigger1Action.transform.GetChild(1).gameObject;
        trig2_positiveAction = trigger2Action.transform.GetChild(0).gameObject;
        trig2_negativeAction = trigger2Action.transform.GetChild(1).gameObject;
        trig3_positiveAction = trigger3Action.transform.GetChild(0).gameObject;
        trig3_negativeAction = trigger3Action.transform.GetChild(1).gameObject;
        trig4_positiveAction = trigger4Action.transform.GetChild(0).gameObject;
        trig4_negativeAction = trigger4Action.transform.GetChild(1).gameObject;

        trig1_positiveAction.SetActive(true);
        // trig1_negativeAction.SetActive(true);
        trig2_positiveAction.SetActive(true);
        // trig2_negativeAction.SetActive(true);
        trig3_positiveAction.SetActive(true);
        // trig3_negativeAction.SetActive(true);
        trig4_positiveAction.SetActive(true);
        // trig4_negativeAction.SetActive(true);

        //fade away the clouds
        Color c;
        //lake
        c = trig1_positiveAction.transform.GetChild(0).GetComponent<Image>().color;
        c.a = 0;
        trig1_positiveAction.transform.GetChild(0).GetComponent<Image>().color = c;
        c = trig1_positiveAction.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color;
        c.a = 0;
        trig1_positiveAction.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color = c;
        c = trig1_negativeAction.transform.GetChild(0).GetComponent<Image>().color;
        c.a = 0;
        trig1_negativeAction.transform.GetChild(0).GetComponent<Image>().color = c;
        c = trig1_negativeAction.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color;
        c.a = 0;
        trig1_negativeAction.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color = c;
        //wwtp
        c = trig2_positiveAction.transform.GetChild(0).GetComponent<Image>().color;
        c.a = 0;
        trig2_positiveAction.transform.GetChild(0).GetComponent<Image>().color = c;
        c = trig2_positiveAction.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color;
        c.a = 0;
        trig2_positiveAction.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color = c;
        c = trig2_negativeAction.transform.GetChild(0).GetComponent<Image>().color;
        c.a = 0;
        trig2_negativeAction.transform.GetChild(0).GetComponent<Image>().color = c;
        c = trig2_negativeAction.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color;
        c.a = 0;
        trig2_negativeAction.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color = c;
        //house
        c = trig3_positiveAction.transform.GetChild(0).GetComponent<Image>().color;
        c.a = 0;
        trig3_positiveAction.transform.GetChild(0).GetComponent<Image>().color = c;
        c = trig3_positiveAction.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color;
        c.a = 0;
        trig3_positiveAction.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color = c;
        c = trig3_negativeAction.transform.GetChild(0).GetComponent<Image>().color;
        c.a = 0;
        trig3_negativeAction.transform.GetChild(0).GetComponent<Image>().color = c;
        c = trig3_negativeAction.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color;
        c.a = 0;
        trig3_negativeAction.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color = c;
        //island
        c = trig4_positiveAction.transform.GetChild(0).GetComponent<Image>().color;
        c.a = 0;
        trig4_positiveAction.transform.GetChild(0).GetComponent<Image>().color = c;
        c = trig4_positiveAction.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color;
        c.a = 0;
        trig4_positiveAction.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color = c;
        c = trig4_negativeAction.transform.GetChild(0).GetComponent<Image>().color;
        c.a = 0;
        trig4_negativeAction.transform.GetChild(0).GetComponent<Image>().color = c;
        c = trig4_negativeAction.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color;
        c.a = 0;
        trig4_negativeAction.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color = c;
        mCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        mValidPoints =  mMeasureDepth.DepthToColor();

        mTrigger1Points = FilterToTrigger(mValidPoints, mTrigger1Depth, mTrigger1Sensitivity);
        mTrigger2Points = FilterToTrigger(mValidPoints, mTrigger2Depth, mTrigger2Sensitivity);
        mTrigger3Points = FilterToTrigger(mValidPoints, mTrigger3Depth, mTrigger3Sensitivity);
        mTrigger4Points = FilterToTrigger(mValidPoints, mTrigger4Depth, mTrigger4Sensitivity);
        // mRect = CreateRect(mValidPoints);

        if(OnTrigger1Points != null && mTrigger1Points.Count != 0){
            OnTrigger1Points(mTrigger1Points);
        }

        if(OnTrigger2Points != null && mTrigger2Points.Count != 0){
            OnTrigger2Points(mTrigger2Points);
        }

        if(OnTrigger3Points != null && mTrigger3Points.Count != 0){
            OnTrigger3Points(mTrigger3Points);
        }

        if(OnTrigger4Points != null && mTrigger4Points.Count != 0){
            OnTrigger4Points(mTrigger4Points);
        }
    }

    private void OnGUI(){
        // GUI.Box(mRect, "");

        if(mTrigger1Points != null && showObject1)
            foreach(Vector2 point in mTrigger1Points){
                Rect rect = new Rect(point, new Vector2(10, 10));
                GUI.Box(rect, "");
            }

        if(mTrigger2Points != null && showObject2)
            foreach(Vector2 point in mTrigger2Points){
                Rect rect = new Rect(point, new Vector2(10, 10));
                GUI.Box(rect, "");
            }

        if(mTrigger3Points != null && showObject3)
            foreach(Vector2 point in mTrigger3Points){
                Rect rect = new Rect(point, new Vector2(10, 10));
                GUI.Box(rect, "");
            }

        if(mTrigger4Points != null && showObject4)
            foreach(Vector2 point in mTrigger4Points){
                Rect rect = new Rect(point, new Vector2(10, 10));
                GUI.Box(rect, "");
            }
    }

    private List<Vector2> FilterToTrigger(List<ValidPoint> validPoints, float triggerDepth, float triggerSensitivity){
        List<Vector2> triggerPoints = new List<Vector2>();
        // Vector3 average = new Vector3(0, 0, 0);
        // int count = 0;

        foreach(ValidPoint point in validPoints){
            if(!point.mWithinWallDepth){
                if(point.z > triggerDepth - triggerSensitivity && point.z < triggerDepth){
                    Vector2 screenPoint = mMeasureDepth.ScreenToCamera(new Vector2(point.colorSpace.X, point.colorSpace.Y));
                    Vector2 flippedY = new Vector2(screenPoint.x, mCamera.pixelHeight - screenPoint.y);
                    Vector2 rotated;
                    rotated = flippedY - (new Vector2(mCamera.pixelWidth/2, mCamera.pixelHeight/2));
                    rotated = new Vector2(-rotated.y, rotated.x);
                    rotated += (new Vector2(mCamera.pixelWidth/2, mCamera.pixelHeight/2));

                    triggerPoints.Add(flippedY);
                    // triggerPoints.Add(rotated);
                    // if(Input.GetKeyDown(KeyCode.Space)){
                        // Vector2 coordinates = ScreenToCamera(new Vector2(point.colorSpace.X, point.colorSpace.Y));
                        // Color color = (mRawImage.GetComponent<RawImage>().texture as Texture2D).GetPixel((int)coordinates.x, (int)coordinates.y)*255;
                        // count++;
                        // average += new Vector3(color.r, color.g, color.b);
                        // Debug.Log(color);
                    // }
                }
            }
        }

        // if(count !=0 ){
        //     average /= count;
        //     Debug.Log(average);
        //     if(Mathf.Abs(average.x - 173) < 5 && Mathf.Abs(average.y - 185) < 5 && Mathf.Abs(average.z - 195) < 5)
        //         Debug.Log("red thing");
        //     else Debug.Log("not red thing");
        // }
        
        return triggerPoints;
    }
}
