using UnityEngine;

public class Floater : MonoBehaviour
{
    [Header("Settings")]
    public float amplitude = 0.5f;
    public float frequency = 1f;

    private Vector3 _startPos;

    void Start()
    {
        _startPos = transform.localPosition;
    }

    void Update()
    {
        float newY = _startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;

        transform.localPosition = new Vector3(_startPos.x, newY, _startPos.z);
    }
}