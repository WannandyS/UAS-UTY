using UnityEngine;
using UnityEngine.Video;
using UnityEngine.XR.Interaction.Toolkit;

public class PowerButtonSimple : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public MeshRenderer screenRenderer;
    public Material screenOffMaterial;
    public Material screenOnMaterial;

    private bool isOn = false;

    void Start()
    {
        // layar mati di awal
        screenRenderer.material = screenOffMaterial;
        videoPlayer.playOnAwake = false;
        videoPlayer.Stop();
    }

    public void PowerOn()
    {
        if (isOn) return;
        isOn = true;

        // siapkan video dulu
        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += OnVideoPrepared;
    }

    void OnVideoPrepared(VideoPlayer vp)
    {
        // ganti material SETELAH video siap
        screenRenderer.material = screenOnMaterial;
        vp.Play();

        // lepas event biar tidak double
        vp.prepareCompleted -= OnVideoPrepared;
    }
}
