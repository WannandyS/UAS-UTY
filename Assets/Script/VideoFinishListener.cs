using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class VideoFinishListener : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    public UnityEvent OnVideoFinished;
    public UnityEvent OnVideoError;

    private void Awake()
    {
        if (videoPlayer == null)
            videoPlayer = GetComponent<VideoPlayer>();
    }

    private void OnEnable()
    {
        // Subscribe to the event
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoFinishedInvoker;
            videoPlayer.errorReceived += OnVideoErrorInvoker;
        }
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks or double-calling
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoFinishedInvoker;
            videoPlayer.errorReceived -= OnVideoErrorInvoker;
        }
    }

    private void OnVideoFinishedInvoker(VideoPlayer vp)
    {
        Debug.Log("Video finished playing!");
        OnVideoFinished?.Invoke();
    }

    private void OnVideoErrorInvoker(VideoPlayer vp, string message)
    {
        Debug.LogError($"Video Error: {message}");
        OnVideoError?.Invoke();
    }
}