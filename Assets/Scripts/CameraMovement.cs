using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    [SerializeField]  Transform CameraPosition;



    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = CameraPosition.position;
        transform.rotation = CameraPosition.rotation;
    }
}
