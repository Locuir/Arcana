using UnityEngine;

public class BillBoard : MonoBehaviour
{

    public Transform Cam;



    private void Awake()
    {
        Cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = Cam.forward;
    }
}
