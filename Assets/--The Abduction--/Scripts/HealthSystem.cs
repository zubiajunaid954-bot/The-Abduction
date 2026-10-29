using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class HealthSystem : MonoBehaviour
{
    public Slider healthSlider;
    public string gameOverScene = "GameOverScene";
    private bool isGameOver = false; // prevent multiple scene loads

    void Update()
    {
        if (healthSlider == null)
        {
            Debug.LogError("?? Health Slider is NOT assigned!");
            return;
        }

        Debug.Log("Current Health: " + healthSlider.value);

        // Trigger Game Over when health is very low
        if (healthSlider.value <= 0.1f && !isGameOver)
        {
            isGameOver = true;
            Debug.Log("?? Health reached zero ? Loading GameOverScene in 2 seconds...");
            StartCoroutine(LoadGameOverSceneAfterDelay(2f)); // 2 seconds delay
        }
    }

    private IEnumerator LoadGameOverSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(gameOverScene);
    }
}
