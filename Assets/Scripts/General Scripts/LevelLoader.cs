using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public Slider slider;

    public void levelLoader(int sceneIndex)
    {
        StartCoroutine(LoadAsynch(sceneIndex));
    }

    IEnumerator LoadAsynch(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            
            slider.value = progress;
            
            yield return null;
        }
    }

    
}
