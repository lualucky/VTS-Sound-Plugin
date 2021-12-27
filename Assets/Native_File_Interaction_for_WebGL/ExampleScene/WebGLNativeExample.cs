using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using WebGL;

/// <summary>
/// en:Example Test
/// cn:测试代码
/// </summary>
public class WebGLNativeExample : MonoBehaviour
{
    /// <summary>
    /// en: compoent
    /// cn：相关测试组件信息
    /// </summary>
    public RawImage contentTextRawImage;
    public Text ContentText;
    public Text fileNameText;
    public Text fileSizeText;
    public Texture2D tex;

    public void Awake()
    {
        contentTextRawImage.gameObject.SetActive(false);
        ContentText.gameObject.SetActive(false);
    }

    /// <summary>
    /// en:open native image file
    /// cn:打开本地图片
    /// </summary>
    public void OnImageLoadBtClick() 
    {
        NativeFileInteractionforWebGLManager.instance.callActionCallBack = OnLoadFinish;
        NativeFileInteractionforWebGLManager.instance._OpenSelectFilePopup("image/gif, image/psd, image/png, image/jpeg, image/bmp, image/tiff, image/tga, image/iff");
    }

    /// <summary>
    /// en:open native Sound file
    /// cn:打开本地Sound
    /// </summary>
    public void OnSoundBtClick()
    {
        NativeFileInteractionforWebGLManager.instance.callActionCallBack = OnLoadFinish;
        NativeFileInteractionforWebGLManager.instance._OpenSelectFilePopup(".mp3");
    }

    /// <summary>
    /// en:open native Video file
    /// cn:打开本地Video
    /// </summary>
    public void OnVideoBtClick()
    {
        NativeFileInteractionforWebGLManager.instance.callActionCallBack = OnLoadFinish;
        NativeFileInteractionforWebGLManager.instance._OpenSelectFilePopup(".avi,.mp4,.mkv,.wma,.wav,.flv");
    }

    /// <summary>
    /// en:open native other any file
    /// cn:打开任何本地文件
    /// </summary>
    public void OnOtherFileBtClick()
    {
        NativeFileInteractionforWebGLManager.instance.callActionCallBack = OnLoadFinish;
        NativeFileInteractionforWebGLManager.instance._OpenSelectFilePopup("");
    }

    /// <summary>
    /// en:open native .txt file
    /// cn:打开本地.txt
    /// </summary>
    public void OnTextLoadBtClick()
    {
        NativeFileInteractionforWebGLManager.instance.callActionCallBack = OnLoadFinish;
        NativeFileInteractionforWebGLManager.instance._OpenSelectFilePopup(".txt");
    }

    /// <summary>
    /// en:save texture to native .jpg
    /// cn:保持纹理到本地
    /// </summary>
    public void OnClickSaveTexture() 
    {
        NativeFileInteractionforWebGLManager.instance.SaveTextureToFile("example_texture_file", tex);
    }

    /// <summary>
    /// en:save text to .txt
    /// cn:保持文本到本地
    /// </summary>
    public void OnClickSaveText() 
    {
        NativeFileInteractionforWebGLManager.instance.SaveTextToFile("example_text_file", "asdfawef13215646481321564fgkfgh648adsgadsg13215646481321564648132156464813215646481321564648ftkfty13215646481321564648fghjkfgh132156464813215646481fhgkjfghk32156464813215646481fkfhgk3215646481321564648132156sdaga464813215646481321564648132156464813215646481fktyuk321564648fkfk13215646481321564648132156464813215646fjgdj481321asdgfasd56464813215646481321564648fghk132156464813215646481321564648132156464813215646481321564648132156fgkfghk46481321564648132156464813ghljghl21564648132156464813215646481321564648132156464813215646481321564648132156464813215646481321564648132156464813215646481321564648132156464813215646481321564648");
    }


    /// <summary>
    /// en:on load finish
    /// cn:加载完毕回调信息
    /// </summary>
    /// <param name="value"></param>
    /// <param name="data"></param>
    private void OnLoadFinish(string value, byte[] data) 
    {
        var ext = Path.GetExtension(value);
        Debug.LogError("OnLoadFinish 1");
        ext = ext.ToLower();
        if (ext.IndexOf(".gif") > -1 || ext.IndexOf(".psd") > -1 || ext.IndexOf(".png") > -1 || ext.IndexOf(".jpeg") > -1
            || ext.IndexOf(".bmp") > -1 || ext.IndexOf(".tiff") > -1 || ext.IndexOf(".tga") > -1 || ext.IndexOf(".iff") > -1
            || ext.IndexOf(".jpg") > -1)
        {
            contentTextRawImage.gameObject.SetActive(true);
            ContentText.gameObject.SetActive(false);
            contentTextRawImage.texture = GenTexture2D(data);
        }
        else if (ext.IndexOf(".txt") > -1)
        {
            contentTextRawImage.gameObject.SetActive(false);
            ContentText.gameObject.SetActive(true);
            var dt = System.Text.Encoding.Default.GetString(data);
            if (dt.Length > 8000)
            {
                ContentText.text = dt.Substring(0, 8000);
            }
            else 
            {
                ContentText.text = dt;
            }
        }
        else
        {
            contentTextRawImage.gameObject.SetActive(false);
            ContentText.gameObject.SetActive(true);
            ContentText.text = "load finish: " + value + " \r\n data length:" + data.Length;
        }
        fileSizeText.text = "File Name: " + value;
        fileNameText.text = "File Size: " + data.Length.ToString();
        NativeFileInteractionforWebGLManager.instance.callActionCallBack = null;
    }


    /// <summary>
    /// en:gen texture
    /// cn:生成纹理
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static Texture2D GenTexture2D(byte[] dt)
    {
        if (dt == null)
        {
            return null;
        }
        var texture = new Texture2D(2, 2, TextureFormat.ARGB32, false, true);
        texture.LoadImage(dt);
        texture.Apply();
        return texture;
    }
}
