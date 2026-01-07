using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private TMP_Text _textComponent;
    [SerializeField] private float _typingSpeed = 0.05f;

    public UnityEvent OnTypewriterComplete;

    private Coroutine _typingCoroutine;

    private void Start()
    {
        if (_textComponent == null)
            _textComponent = GetComponent<TMP_Text>();
    }

    public void Run(string textToType)
    {
        if (_typingCoroutine != null)
            StopCoroutine(_typingCoroutine);
        _typingCoroutine = StartCoroutine(TypeText(textToType));
    }

    private IEnumerator TypeText(string textToType)
    {
        _textComponent.text = textToType;
        _textComponent.ForceMeshUpdate();

        int totalVisibleCharacters = _textComponent.textInfo.characterCount;

        _textComponent.maxVisibleCharacters = 0;

        for (int i = 0; i <= totalVisibleCharacters; i++)
        {
            _textComponent.maxVisibleCharacters = i;

            yield return new WaitForSeconds(_typingSpeed);
        }

        _typingCoroutine = null;

        yield return new WaitForSeconds(2);
        OnTypewriterComplete?.Invoke();
    }

    public IEnumerator Skip()
    {
        if ( _typingCoroutine != null)
        {
            StopCoroutine(_typingCoroutine);
            _typingCoroutine = null;
            _textComponent.maxVisibleCharacters = int.MaxValue;
            yield return new WaitForSeconds(2);
            OnTypewriterComplete?.Invoke();
        }
    }
}
