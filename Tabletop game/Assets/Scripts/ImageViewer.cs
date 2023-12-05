using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageViewer : MonoBehaviour
{
    public MultiSourceManager mMultiSource;
    public RawImage mRawImage;
    public RawImage mRawDepth;
    public MeasureDepth mMeasureDepth;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mRawImage.texture = mMultiSource.GetColorTexture();

        // if(Input.GetKeyDown(KeyCode.Space)){
        //     foreach(ValidPoint point in mMeasureDepth.mValidPoints){
        //         Vector2 coordinates = mMeasureDepth.ScreenToCamera(new Vector2(point.colorSpace.X, point.colorSpace.Y));
        //         Debug.Log((mRawImage.GetComponent<RawImage>().texture as Texture2D).GetPixel((int)coordinates.x, (int)coordinates.y));
        //     }
        // }

        // mRawDepth.texture = mMeasureDepth.mDepthTexture;
    }
}
