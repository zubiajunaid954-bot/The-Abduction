using UnityEngine;

public class Show : MonoBehaviour
{
    public GameObject Key;

    public bool k;

    private void Update()
    {
        if(k == true)
        {
            shw();
            k = false;
        }
    }

    public void shw()
    { Key.SetActive(true); }

}
