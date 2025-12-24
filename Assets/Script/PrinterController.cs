using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class PrinterController : MonoBehaviour
{
    [Header("Socket")]
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor paperSocket;

    [Header("Print Settings")]
    public GameObject printedPaperPrefab;
    public Transform outputPoint;
    public int maxPrintCount = 2;

    [Header("Paper")]
    public float paperMoveSpeed = 0.2f;
    public float paperMoveDuration = 1.5f;

    [Header("Audio")]
    public AudioSource printerAudio;
    public float audioStartTime = 0f;
    public float audioPlayDuration = 1.5f;

    private int currentPrintCount = 0;
    private GameObject currentPaperStack;
    private Coroutine audioRoutine;

    void Awake()
    {
        // 🔥 AUTO AMBIL AudioSource kalau lupa di Inspector
        if (printerAudio == null)
            printerAudio = GetComponent<AudioSource>();
    }

    void Start()
    {
        paperSocket.selectEntered.AddListener(OnPaperInserted);
        paperSocket.selectExited.AddListener(OnPaperRemoved);
    }

    void OnPaperInserted(SelectEnterEventArgs args)
    {
        currentPaperStack = args.interactableObject.transform.gameObject;
        currentPrintCount = 0;
    }

    void OnPaperRemoved(SelectExitEventArgs args)
    {
        currentPaperStack = null;
        currentPrintCount = 0;
    }

    public void Print()
    {
        Debug.Log("PRINT DIPANGGIL");

        if (currentPaperStack == null)
        {
            Debug.Log("Tidak ada kertas!");
            return;
        }

        if (currentPrintCount >= maxPrintCount)
        {
            Debug.Log("Kertas habis");
            return;
        }

        currentPrintCount++;

        // 🔊 AUDIO (ANTI GAGAL)
        if (printerAudio != null && printerAudio.clip != null)
        {
            printerAudio.Stop();
            printerAudio.time = audioStartTime;
            printerAudio.Play();

            if (audioRoutine != null)
                StopCoroutine(audioRoutine);

            audioRoutine = StartCoroutine(StopPrinterAudio());
        }
        else
        {
            Debug.LogWarning("AudioSource atau AudioClip belum ada!");
        }

        // 📄 SPAWN KERTAS
        GameObject paper = Instantiate(
            printedPaperPrefab,
            outputPoint.position,
            outputPoint.rotation
        );

        StartCoroutine(MovePaperOut(paper));

        if (currentPrintCount >= maxPrintCount)
        {
            Destroy(currentPaperStack);
            currentPaperStack = null;
        }
    }

    IEnumerator MovePaperOut(GameObject paper)
    {
        Rigidbody rb = paper.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;

        Vector3 startPos = outputPoint.position;
        Vector3 endPos = outputPoint.position + outputPoint.forward * 0.4f;

        float elapsed = 0f;

        while (elapsed < paperMoveDuration)
        {
            paper.transform.position =
                Vector3.Lerp(startPos, endPos, elapsed / paperMoveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        paper.transform.position = endPos;
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    IEnumerator StopPrinterAudio()
    {
        yield return new WaitForSeconds(audioPlayDuration);
        printerAudio.Stop();
    }
}
