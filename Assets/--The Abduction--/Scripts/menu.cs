using Michsky.UI.Dark;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class menu : MonoBehaviour
{
    public UIDissolveEffect effect;
    public float effectLocation = 1f;
    public GameObject loading;

    private void Start()
    {
        Time.timeScale = 1f;
    }

    public void StartEffect(float duration = 1f)
    {
        loading.SetActive(true);
        StopAllCoroutines(); 
        StartCoroutine(EffectCoroutine(duration));
    }

    private IEnumerator EffectCoroutine(float duration)
    {
        float elapsed = 0f;
        float startValue = 1f;
        float endValue = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

           
            effectLocation = Mathf.Lerp(startValue, endValue, t);
            effect.location = Mathf.Lerp(startValue, endValue, t);

            yield return null;
        }

        effectLocation = endValue; 
        effect.location = endValue;
        SceneManager.LoadScene(1);
    }


}
