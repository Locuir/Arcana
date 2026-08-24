using UnityEngine;

public class FirstObjectiveUI : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) ||
            Input.GetMouseButtonDown(1))
        {
            gameObject.SetActive(false);
        }
    }
}