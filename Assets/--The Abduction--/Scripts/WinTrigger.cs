using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    public GameObject WinPanel,Ai;
 
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            WinPanel.SetActive(true);
            Ai.SetActive(false);
        }
    }
}
