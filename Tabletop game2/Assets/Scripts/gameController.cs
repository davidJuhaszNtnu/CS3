using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.Video;

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
    private GameObject[] circles;
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
    public Material idleTitle_mat;
    private float t_idleTitle, t_idleText;

    public GameObject debugPanel;
    private TextMeshProUGUI debugText_lake, debugText_wwtp, debugText_house, debugText_island;
    public TMP_InputField input_lake, input_wwtp, input_house, input_island;
    public GameObject[] triggerAreaShow;

    public bool allIsTriggered, allIsNotTriggered, allIsFinished_pos, allIsFinished_neg, smthIsShowing_pos, smthIsShowing_neg;
    //game assets
    public GameObject video_pos, video_neg;
    //happyEnd
    private bool startMeasuring_happyEnd, time_happyEnd_elapsed;
    private float time_happyEnd;
    private bool happyEndOn;
    private float t_videos_pos;

    private bool startMeasuring_sadEnd, time_sadEnd_elapsed;
    private float time_sadEnd;
    private bool sadEndOn;
    private float t_videos_neg;

    //circles logic
    public bool smthIsOff, bool_temp_pos, bool_temp_neg;
    private int k;

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
        circles = new GameObject[4];
        circles[0] = trigger1Action.transform.GetChild(2).gameObject;
        circles[1] = trigger2Action.transform.GetChild(2).gameObject;
        circles[2] = trigger3Action.transform.GetChild(2).gameObject;
        circles[3] = trigger4Action.transform.GetChild(2).gameObject;
        foreach(GameObject circle in circles)
            circle.SetActive(false);
        circles[3].SetActive(true);

        trig1_positiveAction.SetActive(true);
        trig2_positiveAction.SetActive(true);
        trig3_positiveAction.SetActive(true);
        trig4_positiveAction.SetActive(true);
        trig1_negativeAction.SetActive(true);
        trig2_negativeAction.SetActive(true);
        trig3_negativeAction.SetActive(true);
        trig4_negativeAction.SetActive(true);

        //fade away the clouds
        //idle
        // idleTitle_mat = idleAction.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material;
        c = idleTitle_mat.color;
        Debug.Log(c);
        c.a = 0f;
        idleTitle_mat.SetColor("_Color", c);
        // fade_idle(0f,0f, 0f, true, true, true);

        //3d models
        // trig1_negativeAction.transform.GetChild(1).gameObject.SetActive(false);
        trig1_positiveAction.transform.GetChild(0).gameObject.SetActive(false);
        trig2_positiveAction.transform.GetChild(0).gameObject.SetActive(false);
        trig3_positiveAction.transform.GetChild(0).gameObject.SetActive(false);
        trig4_positiveAction.transform.GetChild(0).gameObject.SetActive(false);
        trig1_positiveAction.transform.GetChild(1).gameObject.SetActive(false);
        trig2_positiveAction.transform.GetChild(1).gameObject.SetActive(false);
        trig3_positiveAction.transform.GetChild(1).gameObject.SetActive(false);
        trig4_positiveAction.transform.GetChild(1).gameObject.SetActive(false);
        trig1_negativeAction.transform.GetChild(0).gameObject.SetActive(false);
        trig2_negativeAction.transform.GetChild(0).gameObject.SetActive(false);
        trig3_negativeAction.transform.GetChild(0).gameObject.SetActive(false);
        trig4_negativeAction.transform.GetChild(0).gameObject.SetActive(false);
        trig1_negativeAction.transform.GetChild(1).gameObject.SetActive(false);
        trig2_negativeAction.transform.GetChild(1).gameObject.SetActive(false);
        trig3_negativeAction.transform.GetChild(1).gameObject.SetActive(false);
        trig4_negativeAction.transform.GetChild(1).gameObject.SetActive(false);

        fade_videos(0f, video_pos);
        fade_videos(0f, video_neg);

        time_idle = Time.time;
        t_idleTitle = 0f;
        t_idleText = 0f;
        allIsOff = false;

        allIsTriggered = false;
        happyEndOn = false;
        t_videos_pos = 0f;
        allIsNotTriggered = false;
        sadEndOn = false;
        t_videos_neg = 0f;

        //circle logic
        smthIsOff = false;
        allIsFinished_pos = false;
        allIsFinished_neg = false;
        smthIsShowing_pos = false;
        smthIsShowing_neg = false;
        k = 3;

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

    private int increment(int index){
        return (index + 1) > 3 ? 0 : index + 1;
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
        if(!trigger1.GetComponent<Trigger1>().mIsTriggered || !trigger2.GetComponent<Trigger2>().mIsTriggered || !trigger3.GetComponent<Trigger3>().mIsTriggered || !trigger4.GetComponent<Trigger4>().mIsTriggered){
            smthIsOff = true;
        }else smthIsOff = false;
        if(trigger1.GetComponent<Trigger1>().finished_pos && trigger2.GetComponent<Trigger2>().finished_pos && trigger3.GetComponent<Trigger3>().finished_pos && trigger4.GetComponent<Trigger4>().finished_pos){
            allIsFinished_pos = true;
        }else allIsFinished_pos = false;
        if(trigger1.GetComponent<Trigger1>().finished_neg && trigger2.GetComponent<Trigger2>().finished_neg && trigger3.GetComponent<Trigger3>().finished_neg && trigger4.GetComponent<Trigger4>().finished_neg){
            allIsFinished_neg = true;
        }else allIsFinished_neg = false;
        if(trigger1.GetComponent<Trigger1>().isShowing_pos || trigger2.GetComponent<Trigger2>().isShowing_pos || trigger3.GetComponent<Trigger3>().isShowing_pos || trigger4.GetComponent<Trigger4>().isShowing_pos){
            smthIsShowing_pos = true;
        }else smthIsShowing_pos = false;
        if(trigger1.GetComponent<Trigger1>().isShowing_neg || trigger2.GetComponent<Trigger2>().isShowing_neg || trigger3.GetComponent<Trigger3>().isShowing_neg || trigger4.GetComponent<Trigger4>().isShowing_neg){
            smthIsShowing_neg = true;
        }else smthIsShowing_neg = false;

        // Debug.Log(k);
        circles[k].SetActive(true);
        switch(k){
            case 0:
            bool_temp_pos = !trigger2.GetComponent<Trigger2>().isShowing_pos && !trigger3.GetComponent<Trigger3>().isShowing_pos && !trigger4.GetComponent<Trigger4>().isShowing_pos;
                if(!smthIsShowing_neg && trigger1.GetComponent<Trigger1>().mIsTriggered && bool_temp_pos){
                    if(!trigger1.GetComponent<Trigger1>().finished_pos){
                        if(!trigger1.GetComponent<Trigger1>().isShowing_pos)
                            trigger1.GetComponent<Trigger1>().isShowing_pos = true;
                    }
                    else if(!allIsFinished_pos){
                        circles[k].SetActive(false);
                        k = increment(k);
                        if(!trigger2.GetComponent<Trigger2>().mIsTriggered)
                        circles[k].SetActive(true);
                    }
                }
            break;
            case 1:
                bool_temp_pos = !trigger1.GetComponent<Trigger1>().isShowing_pos && !trigger3.GetComponent<Trigger3>().isShowing_pos && !trigger4.GetComponent<Trigger4>().isShowing_pos;
                if(!smthIsShowing_neg && trigger2.GetComponent<Trigger2>().mIsTriggered && bool_temp_pos){
                    if(!trigger2.GetComponent<Trigger2>().finished_pos){
                        if(!trigger2.GetComponent<Trigger2>().isShowing_pos)
                            trigger2.GetComponent<Trigger2>().isShowing_pos = true;
                    }
                    else if(!allIsFinished_pos){
                        circles[k].SetActive(false);
                        k = increment(k);
                        if(!trigger3.GetComponent<Trigger3>().mIsTriggered)
                        circles[k].SetActive(true);
                    }
                }
            break;
            case 2:
                bool_temp_pos = !trigger1.GetComponent<Trigger1>().isShowing_pos && !trigger2.GetComponent<Trigger2>().isShowing_pos && !trigger4.GetComponent<Trigger4>().isShowing_pos;
                if(!smthIsShowing_neg && trigger3.GetComponent<Trigger3>().mIsTriggered && bool_temp_pos){
                    if(!trigger3.GetComponent<Trigger3>().finished_pos){
                        
                        if(!trigger3.GetComponent<Trigger3>().isShowing_pos)
                            trigger3.GetComponent<Trigger3>().isShowing_pos = true;
                    }
                    else if(!allIsFinished_pos){
                        circles[k].SetActive(false);
                        k = increment(k);
                        if(!trigger4.GetComponent<Trigger4>().mIsTriggered)
                            circles[k].SetActive(true);
                    }
                }
            break;
            case 3:
                bool_temp_pos = !trigger1.GetComponent<Trigger1>().isShowing_pos && !trigger2.GetComponent<Trigger2>().isShowing_pos && !trigger3.GetComponent<Trigger3>().isShowing_pos;
                if(!smthIsShowing_neg && trigger4.GetComponent<Trigger4>().mIsTriggered && bool_temp_pos){
                    if(!trigger4.GetComponent<Trigger4>().finished_pos){
                        
                        if(!trigger4.GetComponent<Trigger4>().isShowing_pos){
                            trigger4.GetComponent<Trigger4>().isShowing_pos = true;
                        }
                    }
                    else if(!allIsFinished_pos){
                        circles[k].SetActive(false);
                        k = increment(k);
                        // Debug.Log(k);
                        if(!trigger1.GetComponent<Trigger1>().mIsTriggered)
                        circles[k].SetActive(true);
                    }
                }
            break;
        }
        
        // k_neg = increment(k_neg);

        // switch(k_neg){
        //     case 0:
        // Debug.Log(smthIsShowing_pos);
                bool_temp_neg = !trigger2.GetComponent<Trigger2>().isShowing_neg && !trigger3.GetComponent<Trigger3>().isShowing_neg && !trigger4.GetComponent<Trigger4>().isShowing_neg;
                if(!smthIsShowing_pos && !trigger1.GetComponent<Trigger1>().mIsTriggered && bool_temp_neg){
                    if(!trigger1.GetComponent<Trigger1>().finished_neg){
                        if(!trigger1.GetComponent<Trigger1>().isShowing_neg)
                            trigger1.GetComponent<Trigger1>().isShowing_neg = true;
                        
                    }
                    // else if(!allIsFinished_neg){
                    //     k_neg = increment(k_neg);
                    //     // Debug.Log(k);
                    // }
                }
            // break;
            // case 1:
                bool_temp_neg = !trigger1.GetComponent<Trigger1>().isShowing_neg && !trigger3.GetComponent<Trigger3>().isShowing_neg && !trigger4.GetComponent<Trigger4>().isShowing_neg;
                if(!smthIsShowing_pos && !trigger2.GetComponent<Trigger2>().mIsTriggered && bool_temp_neg){
                    if(!trigger2.GetComponent<Trigger2>().finished_neg){
                        if(!trigger2.GetComponent<Trigger2>().isShowing_neg)
                            trigger2.GetComponent<Trigger2>().isShowing_neg = true;
                        // k_neg = increment(k_neg);
                    }
                    // else if(!allIsFinished_neg){
                    //     k_neg = increment(k_neg);
                    //     // Debug.Log(k);
                    // }
                }
            // break;
            // case 2:
                bool_temp_neg = !trigger1.GetComponent<Trigger1>().isShowing_neg && !trigger2.GetComponent<Trigger2>().isShowing_neg && !trigger4.GetComponent<Trigger4>().isShowing_neg;
                if(!smthIsShowing_pos && !trigger3.GetComponent<Trigger3>().mIsTriggered && bool_temp_neg){
                    if(!trigger3.GetComponent<Trigger3>().finished_neg){
                        if(!trigger3.GetComponent<Trigger3>().isShowing_neg)
                            trigger3.GetComponent<Trigger3>().isShowing_neg = true;
                        // k_neg = increment(k_neg);
                    }
                    // else if(!allIsFinished_neg){
                    //     k_neg = increment(k_neg);
                    //     // Debug.Log(k);
                    // }
                }
            // break;
            // case 3:
                bool_temp_neg = !trigger1.GetComponent<Trigger1>().isShowing_neg && !trigger2.GetComponent<Trigger2>().isShowing_neg && !trigger3.GetComponent<Trigger3>().isShowing_neg;
                if(!smthIsShowing_pos && !trigger4.GetComponent<Trigger4>().mIsTriggered && bool_temp_neg){
                    if(!trigger4.GetComponent<Trigger4>().finished_neg){
                        if(!trigger4.GetComponent<Trigger4>().isShowing_neg)
                            trigger4.GetComponent<Trigger4>().isShowing_neg = true;
                        // k_neg = increment(k_neg);
                    }
                    // else if(!allIsFinished_neg){
                    //     k_neg = increment(k_neg);
                    //     // Debug.Log(k);
                    // }
                }
        //     break;
        // }

        if(allIsOff && !happyEndOn && !sadEndOn){
            //idle
            if(!startMeasuring_idle){
                // if(smthIsOff)
                    // circles[k].SetActive(true);
                startMeasuring_idle = true;
                time_idle = Time.time;
            }else if((Time.time - time_idle) < 5f){
                time_idle_elapsed = false;
            }else{
                time_idle_elapsed = true;
            }
            if(time_idle_elapsed){
                //fade in title
                // if(t_idleTitle < 1f && !time_title_elapsed && !time_text_elapsed){
                //     t_idleTitle += 0.01f;
                //     fade_idle(0f, t_idleTitle, 0f, false, true, false);
                // }

                
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
                        // fade_idle(0f, t_idleTitle, 0f, false, true, false);
                    }
                    //fade in text
                    if(t_idleText < 1f && !time_text_elapsed){
                        t_idleText += 0.01f;
                        // fade_idle(0f, 0f, t_idleText, false, false, true);
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
                            // fade_idle(0f, 0f, t_idleText, false, false, true);
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
            if(t_idleTitle > 0f){
                t_idleTitle -= 0.01f;
                // fade_idle(0f, t_idleTitle, 0f, false, true, false);
            }
            if(t_idleText > 0f){
                t_idleText -= 0.01f;
                // fade_idle(0f, 0f, t_idleText, false, false, true);
            }
        }

        //happy end
        if(allIsTriggered && allIsFinished_pos){
            if(!startMeasuring_happyEnd){
                startMeasuring_happyEnd = true;
                time_happyEnd = Time.time;
            }else if((Time.time - time_happyEnd) < 5f){
                // circles[k].SetActive(false);
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

        //sad end
        if(allIsNotTriggered && !smthIsShowing_pos && !smthIsShowing_neg){
            if(!startMeasuring_sadEnd){
                startMeasuring_sadEnd = true;
                time_sadEnd = Time.time;
            }else if((Time.time - time_sadEnd) < 5f){
                // circles[k].SetActive(false);
                sadEndOn = true;
                time_sadEnd_elapsed = false;
                video_neg.transform.GetChild(0).GetComponent<UnityEngine.Video.VideoPlayer>().Play();
                video_neg.transform.GetChild(1).GetComponent<UnityEngine.Video.VideoPlayer>().Play();
            }else{
                time_sadEnd_elapsed = true;
                sadEndOn = false;
            }
            if(!time_sadEnd_elapsed){
                //fade in videos
                if(t_videos_neg < 1f){
                    t_videos_neg += 0.01f;
                    fade_videos(t_videos_neg, video_neg);
                }
            }else{
                //fade out videos
                if(t_videos_neg > 0f){
                    t_videos_neg -= 0.01f;
                    fade_videos(t_videos_neg, video_neg);
                }else{
                    video_neg.transform.GetChild(0).GetComponent<UnityEngine.Video.VideoPlayer>().Stop();
                    video_neg.transform.GetChild(1).GetComponent<UnityEngine.Video.VideoPlayer>().Stop();
                }
            }
        }else{
            startMeasuring_sadEnd = false;
            time_sadEnd_elapsed = false;
            sadEndOn = false;
            //fade out videos
            if(t_videos_neg > 0f){
                t_videos_neg -= 0.01f;
                fade_videos(t_videos_neg, video_neg);
            }else{
                video_neg.transform.GetChild(0).GetComponent<UnityEngine.Video.VideoPlayer>().Pause();
                video_neg.transform.GetChild(1).GetComponent<UnityEngine.Video.VideoPlayer>().Pause();
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