using UnityEngine;
using System.Runtime.InteropServices;

public static class HapticManager
{
    /// <summary>
    ///     Triggers a vibration for the specified duration (in seconds).
    /// </summary>
    public static void TriggerHaptic(float durationSeconds)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
            WebGLVibrate(durationSeconds);
#elif UNITY_ANDROID && !UNITY_EDITOR
            AndroidVibrate(durationSeconds);
#else
        // Other platforms: do nothing
#endif
    }

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void Vibrate(float milliseconds);

    private static void WebGLVibrate(float durationSeconds)
    {
        Vibrate(durationSeconds * 1000f); // convert seconds to ms
    }
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
    private static void AndroidVibrate(float durationSeconds)
    {
        try
        {
            using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                var context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
                var vibrator = context.Call<AndroidJavaObject>("getSystemService", "vibrator");

                if (vibrator.Call<bool>("hasVibrator"))
                {
                    vibrator.Call("vibrate", (long)(durationSeconds * 1000f));
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("HapticManager AndroidVibrate error: " + e);
        }
    }
#endif
}