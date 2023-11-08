using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapToCenter : MonoBehaviour
{
    public RectTransform scrollPanel;
    public GameObject[] layers, images;
    public GameObject appPanel;
    public float lerpSpeed;
    public Sprite selectedImage, notSelectedImage;

    private float[] distances;
    private bool dragging;
    private float distanceBetweenLayers;
    private int indexMin; //index of the layer closest to the center

    void Start()
    {
        int layersCount = layers.Length;
        distances = new float[layersCount];
        // distanceBetweenLayers = Screen.width + 20;
        if(Screen.orientation == ScreenOrientation.Portrait)
            distanceBetweenLayers = 740f;
        else distanceBetweenLayers = 1400f;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < layers.Length; i++){
            distances[i] = Mathf.Abs(transform.position.x - layers[i].transform.position.x);
        }

        float minDistance = Mathf.Min(distances);
        // Debug.Log(minDistance);

        for (int i = 0; i < layers.Length; i++){
            if(distances[i] == minDistance){
                indexMin = i;
                appPanel.GetComponent<ButtonsController>().current_layer = indexMin;
                images[i].GetComponent<Image>().sprite = selectedImage;
            }else images[i].GetComponent<Image>().sprite = notSelectedImage;
        }

        // distanceBetweenLayers = Screen.width + 20;
        if(Screen.orientation == ScreenOrientation.Portrait)
            distanceBetweenLayers = 740f;
        else distanceBetweenLayers = 1400f;

        if(!dragging){
            LerpToLayer(indexMin * (-distanceBetweenLayers));
        }
    }

    void LerpToLayer(float position){
        float newX = Mathf.Lerp(scrollPanel.anchoredPosition.x, position, Time.deltaTime * lerpSpeed);
        Vector2 newPosition = new Vector2(newX, scrollPanel.anchoredPosition.y);

        scrollPanel.anchoredPosition = newPosition;
    }

    public void StartDrag(){
        dragging = true;
    }

    public void EndDrag(){
        dragging = false;
    }
}
