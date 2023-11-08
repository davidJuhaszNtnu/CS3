using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LayerScreenLayout : MonoBehaviour
{
    public GameObject[] layerPanels;

    //adjust text location depending on screen orientation
    public TextMeshProUGUI[] titles;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < layerPanels.Length; i++){
            layerPanels[i].GetComponent<RectTransform>().offsetMin = new Vector2((i+1) * (Screen.width + 20), 0);
            layerPanels[i].GetComponent<RectTransform>().offsetMax = new Vector2((i+1) * (Screen.width + 20), 0);
        }
        foreach(TextMeshProUGUI title in titles){
            title.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -Screen.height*0.07f);
        }
    }
}
