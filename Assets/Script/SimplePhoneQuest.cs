using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SimplePhoneRinging : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip ringtone;
    public AudioClip caller;
    public AudioClip hangup;

    [Header("XR")]
    public XRGrabInteractable grab;
    public XRSocketInteractor socket;

    [Header("Delay Settings")]
    public float callerDelay = 1f;
    public float hangupDelay = 1f;
    public float hangupLoopDelay = 2f;

    private bool isRinging = false;
    private bool isCallerPlaying = false;
    private bool callerFinished = false;
    private bool gameStarted = false;

    private Coroutine callerCoroutine;
    private Coroutine hangupCoroutine;

    void Start()
    {
        Invoke(nameof(InitGame), 0.1f);
    }

    void InitGame()
    {
        StartRinging();
        gameStarted = true;

        grab.selectEntered.AddListener(_ => OnPickedUp());
        socket.selectEntered.AddListener(_ => OnSocketed());
    }

    // 🔔 RINGING
    void StartRinging()
    {
        StopAllCoroutines();

        audioSource.Stop();
        audioSource.clip = ringtone;
        audioSource.loop = true;
        audioSource.Play();

        isRinging = true;
        isCallerPlaying = false;
        callerFinished = false;
    }

    // ✋ DIANGKAT
    void OnPickedUp()
    {
        if (!gameStarted) return;
        if (!isRinging) return;
        if (socket.hasSelection) return;

        audioSource.Stop();
        audioSource.loop = false;
        isRinging = false;

        callerCoroutine = StartCoroutine(PlayCaller());
    }

    // 📞 CALLER
    IEnumerator PlayCaller()
    {
        yield return new WaitForSeconds(callerDelay);

        audioSource.clip = caller;
        audioSource.loop = false;
        audioSource.Play();

        isCallerPlaying = true;

        yield return new WaitForSeconds(caller.length);

        isCallerPlaying = false;
        callerFinished = true;

        hangupCoroutine = StartCoroutine(PlayHangupLoop());
    }

    // ☎️ HANGUP LOOP
    IEnumerator PlayHangupLoop()
    {
        yield return new WaitForSeconds(hangupDelay);

        while (true)
        {
            audioSource.clip = hangup;
            audioSource.loop = false;
            audioSource.Play();

            yield return new WaitForSeconds(hangup.length);
            yield return new WaitForSeconds(hangupLoopDelay);
        }
    }

    // 📦 DISOCKET
    void OnSocketed()
    {
        if (!gameStarted) return;

        // Jika caller belum selesai → reset
        if (isCallerPlaying && !callerFinished)
        {
            StartRinging();
            return;
        }

        // Jika sudah selesai → quest selesai
        if (callerFinished)
        {
            StopAllCoroutines();
            audioSource.Stop();

            CompleteQuest();
        }
    }

    void CompleteQuest()
    {
        Debug.Log("QUEST SELESAI");
        // 👉 taruh logic quest selesai di sini
    }
}
