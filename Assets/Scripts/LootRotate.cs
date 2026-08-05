using UnityEngine;

public class LootRotate: MonoBehaviour
{

    [Header("Rotation")]
    public float RotationSpeed = 100f;


    [Header("Floating")]
    public float FloatHeight = 0.2f;
    public float FloatSpeed = 2f;

    private Vector3 StartPosition;


    void Start()
    {
        StartPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * RotationSpeed * Time.deltaTime, Space.World);

        float newY = StartPosition.y + Mathf.Sin(Time.time * FloatSpeed) * FloatHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
