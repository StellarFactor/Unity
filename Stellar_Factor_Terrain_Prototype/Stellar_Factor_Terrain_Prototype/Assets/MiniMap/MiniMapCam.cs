using Overtown.Util;
using UnityEngine;

namespace Overtown
{
    public class MiniMapCam : MonoBehaviour
    {
    //    [SerializeField] private Flattener flattener;

    //    [SerializeField] private Camera cam;

    //    private void Awake()
    //    {
    //        if (flattener == null)
    //        {
    //            flattener = GetComponent<Flattener>();
    //        }
    //    }

    //    private void Update()
    //    {
    //        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
    //        Camera.main.targetTexture = rt;
    //        Texture2D thumbnail = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
    //        Camera.main.Render();
    //        RenderTexture.active = rt;
    //        thumbnail.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

    //        thumbnail = flattener.Flatten(thumbnail);
    //        thumbnail.Apply();

    //        Camera.main.targetTexture = null;
    //        RenderTexture.active = null;
    //    }
    }
}
