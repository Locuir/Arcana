using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerCaam : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float senX;
    public float senY;

    public Transform Orientation;

    public float xRotation;
    public float yRotation;




    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
   



    // Update is called once per frame
    void Update()
    {
        float  mouseX = Mouse.current.delta.x.ReadValue() * senX * Time.deltaTime;
        float  mouseY = Mouse.current.delta.y.ReadValue() * senY * Time.deltaTime;
        

        xRotation -= mouseY;
        yRotation += mouseX;

        xRotation =  Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        Orientation.rotation = Quaternion.Euler(0, yRotation, 0);

    }


}
