using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System;

public class gameController : MonoBehaviour
{
    public delegate void NewTriggerPoints(List<Vector2> triggerPoints);
    public static event NewTriggerPoints OnTrigger1Points = null;
    public static event NewTriggerPoints OnTrigger2Points = null;
    public static event NewTriggerPoints OnTrigger3Points = null;
    public static event NewTriggerPoints OnTrigger4Points = null;
    public static event NewTriggerPoints OnTrigger5Points = null;
    private Camera mCamera = null;

    public MeasureDepth mMeasureDepth;
    public List<ValidPoint> mValidPoints = null;

    private List<Vector2> mTrigger1Points = null;
    private List<Vector2> mTrigger2Points = null;
    private List<Vector2> mTrigger3Points = null;
    private List<Vector2> mTrigger4Points = null;
    private List<Vector2> mTrigger5Points = null;

    public bool showObject1, showObject2, showObject3, showObject4, showObject5;

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
    [Header("Trigger 5 language sensitivity and depth")]
    [Range(0, 0.1f)]
    public float mTrigger5Sensitivity;
    public float mTrigger5Depth;
    public GameObject trigger5Action;

    public GameObject trig1_positiveAction, trig1_negativeAction, trig2_positiveAction, trig2_negativeAction, trig3_positiveAction, trig3_negativeAction, 
    trig4_positiveAction, trig4_negativeAction;
    Color c;

    public GameObject trigger1, trigger2, trigger3, trigger4;
    public bool allIsOff;
    private bool idleOn, time_idle_elapsed, startMeasuring_idle;
    private float time_idle;
    //title
    private bool startMeasuring_title, time_title_elapsed;
    private float time_title;
    //text
    private bool startMeasuring_text, time_text_elapsed;
    private float time_text;

    public GameObject idleAction;
    private float t_idleCloud, t_idleTitle, t_idleText;

    public GameObject debugPanel;
    private TextMeshProUGUI debugText_lake, debugText_wwtp, debugText_house, debugText_island;
    public TMP_InputField input_lake, input_wwtp, input_house, input_island;
    public GameObject[] triggerAreaShow;

    private bool allIsTriggered, allIsNotTriggered;
    //game assets
    public GameObject video_pos;
    //happyEnd
    private bool startMeasuring_happyEnd, time_happyEnd_elapsed;
    private float time_happyEnd;
    private bool happyEndOn;
    private float t_videos_pos;
    

    void Start()
    {
        trigger1Action.SetActive(true);
        trigger2Action.SetActive(true);
        trigger3Action.SetActive(true);
        trigger4Action.SetActive(true);
        idleAction.SetActive(true);
        // idleAction.SetActive(false);
        trig1_positiveAction = trigger1Action.transform.GetChild(0).gameObject;
        trig1_negativeAction = trigger1Action.transform.GetChild(1).gameObject;
        trig2_positiveAction = trigger2Action.transform.GetChild(0).gameObject;
        trig2_negativeAction = trigger2Action.transform.GetChild(1).gameObject;
        trig3_positiveAction = trigger3Action.transform.GetChild(0).gameObject;
        trig3_negativeAction = trigger3Action.transform.GetChild(1).gameObject;
        trig4_positiveAction = trigger4Action.transform.GetChild(0).gameObject;
        trig4_negativeAction = trigger4Action.transform.GetChild(1).gameObject;

        trig1_positiveAction.SetActive(true);
        trig2_positiveAction.SetActive(true);
        trig3_positiveAction.SetActive(true);
        trig4_positiveAction.SetActive(true);

        //fade away the clouds
        //lake
        fade(0f, trig1_positiveAction);
        fade(0f, trig1_negativeAction);
        //wwtp
        fade(0f, trig2_positiveAction);
        fade(0f, trig2_negativeAction);
        //house
        fade(0f, trig3_positiveAction);
        fade(0f, trig3_negativeAction);
        //island
        fade(0f, trig4_positiveAction);
        fade(0f, trig4_negativeAction);
        //idle
        fade_idle(0f,0f, 0f, true, true, true);

        //3d models
        trig1_negativeAction.transform.GetChild(1).gameObject.SetActive(false);
        // trigger1.transform.GetComponent<Trigger1>().scale_orig_neg = trig1_negativeAction.transform.GetChild(1).localScale.x;
        // Debug.Log(trig1_negativeAction.transform.GetChild(1).localScale);
        // fade3D(0f, trig1_negativeAction.transform.GetChild(1));
        // fade3D(0f, trig1_negativeAction.transform.GetChild(1));
        fade_videos(0f, video_pos);

        time_idle = Time.time;
        t_idleCloud = 0f;
        t_idleTitle = 0f;
        t_idleText = 0f;
        allIsOff = false;
        allIsTriggered = false;
        happyEndOn = false;
        t_videos_pos = 0f;

        debugPanel.SetActive(false);
        debugText_lake = debugPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        debugText_wwtp = debugPanel.transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        debugText_house = debugPanel.transform.GetChild(7).GetComponent<TextMeshProUGUI>();
        debugText_island = debugPanel.transform.GetChild(10).GetComponent<TextMeshProUGUI>();

        readTargets();
        foreach(GameObject area in triggerAreaShow)
            area.SetActive(false);

        mCamera = Camera.main;
    }

    // private void fade3D(float t, Transform model){
    //     foreach(Transform child in model){
    //         if(child.GetComponent<Renderer>() != null)
    //             foreach(Material material in child.GetComponent<Renderer>().materials){
    //                 material.SetFloat("_Mode", 3);
    //                 c = material.color;
    //                 c.a = 0;
    //                 c.r = 0;
    //                 material.color = c;
    //                 // material.SetColor("_Color", c);
    //             }
    //     }
    // }
    private void fade3D(float t, Transform model){
        model.localScale = Vector3.one * t;
    }

    private void fade(float t, GameObject action){
        //image
        c = action.transform.GetChild(0).GetComponent<Image>().color;
        c.a = t;
        action.transform.GetChild(0).GetComponent<Image>().color = c;
        //text
        c = action.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color;
        c.a = t;
        action.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color = c;
    }

    private void fade_videos(float t, GameObject videos){
        //land
        c = videos.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<RawImage>().color;
        c.a = t;
        videos.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<RawImage>().color = c;
        //sea
        c = videos.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<RawImage>().color;
        c.a = t;
        videos.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<RawImage>().color = c;
    }

    private void fade_idle(float t_cloud, float t_title, float t_text, bool bool_cloud, bool bool_title, bool bool_text){
        if(bool_cloud){
            c = idleAction.transform.GetChild(0).GetComponent<Image>().color;
            c.a = t_cloud;
            idleAction.transform.GetChild(0).GetComponent<Image>().color = c;
        }
        if(bool_title){
            c = idleAction.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color;
            c.a = t_title;
            idleAction.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color = c;
        }
        if(bool_text){
            c = idleAction.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().color;
            c.a = t_text;
            idleAction.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().color = c;
        }
    }

    // Update is called once per frame
    void Update()
    {
        mValidPoints =  mMeasureDepth.DepthToColor();

        mTrigger1Points = FilterToTrigger(mValidPoints, mTrigger1Depth, mTrigger1Sensitivity);
        mTrigger2Points = FilterToTrigger(mValidPoints, mTrigger2Depth, mTrigger2Sensitivity);
        mTrigger3Points = FilterToTrigger(mValidPoints, mTrigger3Depth, mTrigger3Sensitivity);
        mTrigger4Points = FilterToTrigger(mValidPoints, mTrigger4Depth, mTrigger4Sensitivity);
        mTrigger5Points = FilterToTrigger(mValidPoints, mTrigger5Depth, mTrigger5Sensitivity);
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

        if(OnTrigger5Points != null && mTrigger5Points.Count != 0){
            OnTrigger5Points(mTrigger5Points);
        }

        if(trigger1.GetComponent<Trigger1>().isOff && trigger2.GetComponent<Trigger2>().isOff && trigger3.GetComponent<Trigger3>().isOff && trigger4.GetComponent<Trigger4>().isOff){
            allIsOff = true;
        }else allIsOff = false;
        if(trigger1.GetComponent<Trigger1>().mIsTriggered && trigger2.GetComponent<Trigger2>().mIsTriggered && trigger3.GetComponent<Trigger3>().mIsTriggered && trigger4.GetComponent<Trigger4>().mIsTriggered){
            allIsTriggered = true;
        }else allIsTriggered = false;
        if(!trigger1.GetComponent<Trigger1>().mIsTriggered && !trigger2.GetComponent<Trigger2>().mIsTriggered && !trigger3.GetComponent<Trigger3>().mIsTriggered && !trigger4.GetComponent<Trigger4>().mIsTriggered){
            allIsNotTriggered = true;
        }else allIsNotTriggered = false;
        // Debug.Log(allIsTriggered);

        if(allIsOff && !happyEndOn){
            if(!startMeasuring_idle){
                startMeasuring_idle = true;
                time_idle = Time.time;
            }else if((Time.time - time_idle) < 5f){
                time_idle_elapsed = false;
            }else{
                time_idle_elapsed = true;
            }
            if(time_idle_elapsed){
                //fade in cloud
                if(t_idleCloud < 1f){
                    t_idleCloud += 0.01f;
                    fade_idle(t_idleCloud, 0f, 0f, true, false, false);
                }
                //fade in title
                if(t_idleTitle < 1f && !time_title_elapsed && !time_text_elapsed){
                    t_idleTitle += 0.01f;
                    fade_idle(0f, t_idleTitle, 0f, false, true, false);
                }
                
                if(t_idleTitle >= 1f){
                    if(!startMeasuring_title){
                        startMeasuring_title = true;
                        time_title = Time.time;
                    }else if((Time.time - time_title) < 5f){
                        time_title_elapsed = false;
                    }else{
                        time_title_elapsed = true;
                    }
                }
                if(time_title_elapsed){
                    //fade out title
                    if(t_idleTitle > 0f){
                        t_idleTitle -= 0.01f;
                        fade_idle(0f, t_idleTitle, 0f, false, true, false);
                    }
                    //fade in text
                    if(t_idleText < 1f && !time_text_elapsed){
                        t_idleText += 0.01f;
                        fade_idle(0f, 0f, t_idleText, false, false, true);
                    }
                    if(t_idleText >= 1f){
                        if(!startMeasuring_text){
                            startMeasuring_text = true;
                            time_text = Time.time;
                        }else if((Time.time - time_text) < 5f){
                            time_text_elapsed = false;
                        }else time_text_elapsed = true;
                    }
                    if(time_text_elapsed){
                        //fade out text
                        if(t_idleText > 0f){
                            t_idleText -= 0.01f;
                            fade_idle(0f, 0f, t_idleText, false, false, true);
                        }
                        if(t_idleText < 0f){
                            time_title_elapsed = false;
                            time_text_elapsed = false;
                            startMeasuring_title = false;
                            startMeasuring_text = false;
                        }
                    }
                }
            }
        }else{
            startMeasuring_idle = false;
            startMeasuring_title = false;
            startMeasuring_text = false;
            time_idle_elapsed = false;
            time_title_elapsed = false;
            time_text_elapsed = false;
            //fade out everything
            if(t_idleCloud > 0f){
                t_idleCloud -= 0.01f;
                fade_idle(t_idleCloud, 0f, 0f, true, false, false);
            }
            if(t_idleTitle > 0f){
                t_idleTitle -= 0.01f;
                fade_idle(0f, t_idleTitle, 0f, false, true, false);
            }
            if(t_idleText > 0f){
                t_idleText -= 0.01f;
                fade_idle(0f, 0f, t_idleText, false, false, true);
            }
        }

        if(allIsTriggered){
            if(!startMeasuring_happyEnd){
                startMeasuring_happyEnd = true;
                time_happyEnd = Time.time;
            }else if((Time.time - time_happyEnd) < 60f){
                happyEndOn = true;
                time_happyEnd_elapsed = false;
                video_pos.transform.GetChild(0).GetComponent<UnityEngine.Video.VideoPlayer>().Play();
                video_pos.transform.GetChild(1).GetComponent<UnityEngine.Video.VideoPlayer>().Play();
            }else{
                time_happyEnd_elapsed = true;
                happyEndOn = false;
            }
            if(!time_happyEnd_elapsed){
                //fade in videos
                if(t_videos_pos < 1f){
                    t_videos_pos += 0.01f;
                    fade_videos(t_videos_pos, video_pos);
                }
            }else{
                //fade out videos
                if(t_videos_pos > 0f){
                    t_videos_pos -= 0.01f;
                    fade_videos(t_videos_pos, video_pos);
                }else{
                    video_pos.transform.GetChild(0).GetComponent<UnityEngine.Video.VideoPlayer>().Stop();
                    video_pos.transform.GetChild(1).GetComponent<UnityEngine.Video.VideoPlayer>().Stop();
                }
            }
        }else{
            startMeasuring_happyEnd = false;
            time_happyEnd_elapsed = false;
            happyEndOn = false;
            //fade out videos
            if(t_videos_pos > 0f){
                t_videos_pos -= 0.01f;
                fade_videos(t_videos_pos, video_pos);
            }else{
                video_pos.transform.GetChild(0).GetComponent<UnityEngine.Video.VideoPlayer>().Pause();
                video_pos.transform.GetChild(1).GetComponent<UnityEngine.Video.VideoPlayer>().Pause();
            }
        }

        //debug
        if(Input.GetKeyDown("space")){
            if(!debugPanel.activeSelf)
                debugPanel.SetActive(true);
            else debugPanel.SetActive(false);

            foreach(GameObject area in triggerAreaShow)
                if(!area.activeSelf)
                    area.SetActive(true);
                else area.SetActive(false);
        }
    
        if(debugPanel.activeSelf){
            debugText_lake.text = ",getting = " + Mathf.Round(trigger1.GetComponent<Trigger1>().average).ToString();
            debugText_wwtp.text = ",getting = " + Mathf.Round(trigger2.GetComponent<Trigger2>().average).ToString();
            debugText_house.text = ",getting = " + Mathf.Round(trigger3.GetComponent<Trigger3>().average).ToString();
            debugText_island.text = ",getting = " + Mathf.Round(trigger4.GetComponent<Trigger4>().average).ToString();
        }
    }

    private void OnGUI(){
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
        
        if(mTrigger5Points != null && showObject5)
            foreach(Vector2 point in mTrigger5Points){
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
                    triggerPoints.Add(flippedY);
                }
            }
        }
        
        return triggerPoints;
    }

    private void readTargets(){
        StreamReader input = new StreamReader("Assets/Debug/targets.txt");

        string average1 = input.ReadLine();
        string average2 = input.ReadLine();
        string average3 = input.ReadLine();
        string average4 = input.ReadLine();
        trigger1.GetComponent<Trigger1>().averageTarget = Int32.Parse(average1);
        trigger2.GetComponent<Trigger2>().averageTarget = Int32.Parse(average2);
        trigger3.GetComponent<Trigger3>().averageTarget = Int32.Parse(average3);
        trigger4.GetComponent<Trigger4>().averageTarget = Int32.Parse(average4);

        input_lake.text = average1;
        input_wwtp.text = average2;
        input_house.text = average3;
        input_island.text = average4;

        input.Close();
    }

    public void updateTargets_lake(string text){
        if(text != ""){
            trigger1.GetComponent<Trigger1>().averageTarget = Int32.Parse(text);
            updateTargetsFile();
        }
    }
    public void updateTargets_wwtp(string text){
        if(text != ""){
            trigger2.GetComponent<Trigger2>().averageTarget = Int32.Parse(text);
            updateTargetsFile();
        }
    }
    public void updateTargets_house(string text){
        if(text != ""){
            trigger3.GetComponent<Trigger3>().averageTarget = Int32.Parse(text);
            updateTargetsFile();
        }
    }
    public void updateTargets_island(string text){
        if(text != ""){
            trigger4.GetComponent<Trigger4>().averageTarget = Int32.Parse(text);
            updateTargetsFile();
        }
    }

    private void updateTargetsFile(){
        StreamWriter output = new StreamWriter("Assets/Debug/targets.txt");

        output.WriteLine(trigger1.GetComponent<Trigger1>().averageTarget.ToString());
        output.WriteLine(trigger2.GetComponent<Trigger2>().averageTarget.ToString());
        output.WriteLine(trigger3.GetComponent<Trigger3>().averageTarget.ToString());
        output.WriteLine(trigger4.GetComponent<Trigger4>().averageTarget.ToString());

        output.Close();
    }
}
