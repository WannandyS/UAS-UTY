using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectDestroyer : MonoBehaviour
{
    private GameObject FindRootObject(string objectName)
    {
        Scene activeScene = SceneManager.GetActiveScene();

        GameObject[] rootObjects = activeScene.GetRootGameObjects();

        foreach (GameObject obj in rootObjects)
        {
            if (obj.name == objectName)
            {
                return obj;
            }
        }

        return null;
    }

    public void DestroyIt()
    {
        Destroy(gameObject);
    }

    public void DestroyRootObject(string name)
    {
        var obj = FindRootObject(name);
        if (obj != null)
        {
            Destroy(obj);
        }
    }
}
