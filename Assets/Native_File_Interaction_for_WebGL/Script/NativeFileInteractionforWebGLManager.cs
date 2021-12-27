using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

namespace WebGL
{
    /// <summary>
    /// Webgl read local file manager
    /// </summary>
    public class NativeFileInteractionforWebGLManager : MonoBehaviour
    {
        /// <summary>
        /// call back info
        /// </summary>
        public Action<string, byte[]> callActionCallBack;
        /// <summary>
        /// instance
        /// </summary>
        public static NativeFileInteractionforWebGLManager instance;

#if (UNITY_WEBGL && !UNITY_EDITOR)
        [DllImport("__Internal")]
        private static extern IntPtr GetFile(string name);
        [DllImport("__Internal")]
        private static extern void UnloadFile(string name);
        [DllImport("__Internal")]
        private static extern void OpenSelectFilePopup(string typeInfo);
        [DllImport("__Internal")]
        private static extern void CloseSelectFilePopup();
        [DllImport("__Internal")]
        private static extern int FileLength(string name);
        [DllImport("__Internal")]
        private static extern void SaveText(string fileName, string data);
        [DllImport("__Internal")]
        private static extern void SaveTextureToWebGLTexture(string str, string fileName, IntPtr texture);
#endif
        /// <summary>
        /// set instance info
        /// </summary>
        public void Awake()
        {
            instance = this;
        }

        /// <summary>
        /// en: WebGL selected file finish and call this function.
        /// cn:获取文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="bytes"></param>
        /// <param name="fileName"></param>
        public void _GetFile(string filePath) 
        {
#if (UNITY_WEBGL && !UNITY_EDITOR)
            if (string.IsNullOrEmpty(filePath))
            {
                Debug.LogError("filePath error. Replace is null.");
                return;
            }
            int len = FileLength(filePath);
            IntPtr ptr = GetFile(filePath);
            var data = new byte[len];
            Marshal.Copy(ptr, data, 0, data.Length);
            _CloseSelectFilePopup();
            UnloadFile(filePath);
            try
            {
                if (callActionCallBack != null)
                {
                    callActionCallBack(filePath, data);
                }
            }
            catch (System.Exception x)
            {
                Debug.LogError(x);
            }
#endif
        }

        /// <summary>
        /// en:open html Select File UI
        /// cn:打开窗口
        /// </summary>
        /// <param name="typeInfo"></param>
        public void _OpenSelectFilePopup(string typeInfo) 
        {
#if (UNITY_WEBGL && !UNITY_EDITOR)
            OpenSelectFilePopup(typeInfo);
#endif
        }

        /// <summary>
        /// en:
        /// cn:关闭窗口
        /// </summary>
        public void _CloseSelectFilePopup() 
        {
#if (UNITY_WEBGL && !UNITY_EDITOR)
            CloseSelectFilePopup();
#endif
        }

        /// <summary>
        /// en:save text file to native
        /// cn:保持文件在本地
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="text"></param>
        public void SaveTextToFile(string fileName, string text)
        {
#if (UNITY_WEBGL && !UNITY_EDITOR)
            SaveText(fileName, text);
#endif
        }

        /// <summary>
        /// en:save texture file to native
        /// cn:保持图片到本地
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="texture"></param>
        public void SaveTextureToFile(string fileName, Texture2D texture) 
        {
#if (UNITY_WEBGL && !UNITY_EDITOR)
            SaveTextureToWebGLTexture(System.Convert.ToBase64String(texture.EncodeToPNG()), fileName,  texture.GetNativeTexturePtr());
#endif
        }


    }

}