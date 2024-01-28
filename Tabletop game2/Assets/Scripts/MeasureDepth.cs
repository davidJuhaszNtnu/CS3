using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;
using UnityEngine.UI;

public class MeasureDepth : MonoBehaviour
{
    public MultiSourceManager mMultiSource;
    public Texture2D mDepthTexture;
    public RawImage mRawImage;

    // Kinect
    private KinectSensor mSensor = null;
    private CoordinateMapper mMapper = null;
    private Camera mCamera = null;
    
    // Depth data
    private CameraSpacePoint[] mCameraSpacePoints = null;
    private ColorSpacePoint[] mColorSpacePoints = null;
    private ushort[] mDepthData = null;

    //Cutoffs
    // [Range(-10f, 10f)]
    public float mWallDepth = 10;

    [Header("Top and Bottom")]
    [Range(-1f, 1f)]
    public float mTopCutoff = 1;
    [Range(-1f, 1f)]
    public float mBottomCutoff = -1;

    [Header("Left and Right")]
    [Range(-1f, 1f)]
    public float mLeftCutoff = -1;
    [Range(-1f, 1f)]
    public float mRightCutoff = 1;

    private readonly Vector2Int mDepthResolution = new Vector2Int(512, 424);
    private Rect mRect;

    private void Awake()
    {
        mSensor = KinectSensor.GetDefault();
        mMapper = mSensor.CoordinateMapper;
        mCamera = Camera.main;
        
        int arraySize = mDepthResolution.x * mDepthResolution.y;

        mCameraSpacePoints = new CameraSpacePoint[arraySize];
        mColorSpacePoints = new ColorSpacePoint[arraySize];
    }

    public List<ValidPoint> DepthToColor(){
        // points to return
        List<ValidPoint> validPoints = new List<ValidPoint>();

        //get depth data
        mDepthData = mMultiSource.GetDepthData();

        //Map
        mMapper.MapDepthFrameToCameraSpace(mDepthData, mCameraSpacePoints);
        mMapper.MapDepthFrameToColorSpace(mDepthData, mColorSpacePoints);

        //filter for cutoff
        for(int i = 0; i < mDepthResolution.x / 2; i++)
            for(int j = 0; j < mDepthResolution.y / 2; j++){
                int sampleIndex = (j * mDepthResolution.x) + i;
                sampleIndex *= 2;

                if(mCameraSpacePoints[sampleIndex].X < mLeftCutoff)
                    continue;
                if(mCameraSpacePoints[sampleIndex].X > mRightCutoff)
                    continue;
                if(mCameraSpacePoints[sampleIndex].Y < mTopCutoff)
                    continue;
                if(mCameraSpacePoints[sampleIndex].Y > mBottomCutoff)
                    continue;

                ValidPoint newPoint = new ValidPoint(mColorSpacePoints[sampleIndex], mCameraSpacePoints[sampleIndex].Z);

                if(mCameraSpacePoints[sampleIndex].Z >= mWallDepth)
                    newPoint.mWithinWallDepth = true;

                validPoints.Add(newPoint);
            }

        return validPoints;
    }

    public Vector2 ScreenToCamera(Vector2 screenPosition){
        Vector2 normalizedScreen = new Vector2(Mathf.InverseLerp(0, 1920, screenPosition.x), Mathf.InverseLerp(0, 1080, screenPosition.y));
        Vector2 screenPoint = new Vector2(normalizedScreen.x * mCamera.pixelWidth, normalizedScreen.y * mCamera.pixelHeight);
    
        return screenPoint;
    }

    // private Texture2D CreateTexture(List<ValidPoint> validPoints){
    //     Texture2D newTexture = new Texture2D(1920, 1080, TextureFormat.Alpha8, false);

    //     for(int x = 0; x < 1920; x++)
    //         for(int y = 0; y < 1080; y++){
    //             newTexture.SetPixel(x, y, Color.clear);
    //         }

    //     foreach(ValidPoint point in validPoints){
    //         newTexture.SetPixel((int)point.colorSpace.X, (int)point.colorSpace.Y, Color.black);
    //     }

    //     newTexture.Apply();

    //     return newTexture;
    // }

    // #region Rect Creation
    // private Rect CreateRect(List<ValidPoint> points){
    //     if(points.Count == 0)
    //         return new Rect();

    //     Vector2 topLeft = GetTopLeft(points);
    //     Vector2 bottomRight = GetBottomRight(points);

    //     Vector2 screenTopLeft = ScreenToCamera(topLeft);
    //     Vector2 screenBottomRight = ScreenToCamera(bottomRight);

    //     // top left corner is (0,0) and bottom right is (1920, 1080)
    //     int width = (int)(screenBottomRight.x - screenTopLeft.x);
    //     int height = (int)(screenBottomRight.y - screenTopLeft.y);

    //     Vector2 size = new Vector2(width, height);
    //     Rect rect = new Rect(screenTopLeft, size);

    //     return rect;
    // }

    // private Vector2 GetTopLeft(List<ValidPoint> points){
    //     Vector2 topLeft = new Vector2(int.MaxValue, int.MaxValue);

    //     foreach(ValidPoint point in points){
    //         if(point.colorSpace.X < topLeft.x)
    //             topLeft.x = point.colorSpace.X;

    //         if(point.colorSpace.Y < topLeft.y)
    //             topLeft.y = point.colorSpace.Y;
    //     }
        
    //     return topLeft;
    // }

    // private Vector2 GetBottomRight(List<ValidPoint> points){
    //     Vector2 bottomRight = new Vector2(int.MinValue, int.MinValue);

    //     foreach(ValidPoint point in points){
    //         if(point.colorSpace.X > bottomRight.x)
    //             bottomRight.x = point.colorSpace.X;

    //         if(point.colorSpace.Y > bottomRight.y)
    //             bottomRight.y = point.colorSpace.Y;
    //     }
        
    //     return bottomRight;
    // }

    // #endregion
}

public class ValidPoint
{
    public ColorSpacePoint colorSpace;
    public float z = 0.0f;

    public bool mWithinWallDepth = false;

    public ValidPoint(ColorSpacePoint newColorSpace, float newZ){
        colorSpace = newColorSpace;
        z = newZ;
    }
}
