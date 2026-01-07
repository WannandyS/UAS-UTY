using UnityEngine;
using UnityEngine.Events;

public class DiscardFire : MonoBehaviour
{
    private int _count;
    
    public int count;
    public UnityEvent OnTrigger;

    public void Trigger()
    {
        if (_count < count)
        {
            _count++;
            return;
        }
        OnTrigger?.Invoke();
    }
}
