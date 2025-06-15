using UnityEngine;

// Handle animal motion effects
// Script ngatur efek gerakan hewan
public class SimpleAnimalMotion : MonoBehaviour
{
    [Header("Float Effect")]
    public float floatAmplitude = 0.1f;    // Float height
    public float floatFrequency = 1f;      // Float speed

    [Header("Rotation Effect")]
    public float rotationAmplitude = 5f;    // Max rotation
    public float rotationFrequency = 1.5f;  // Rotation speed

    [Header("Scale Effect")]
    public float scaleAmplitude = 0.05f;    // Scale change
    public float scaleFrequency = 2f;       // Scale speed

    private Vector3 startPos;
    private Vector3 startScale;

    void Start()
    {
        // Save initial values
        startPos = transform.position;
        startScale = transform.localScale;
    }

    void Update()
    {
        // Calculate float
        float newY = startPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;

        // Calculate rotation
        float newZRot = Mathf.Sin(Time.time * rotationFrequency) * rotationAmplitude;

        // Calculate scale
        float scaleChange = 1 + Mathf.Sin(Time.time * scaleFrequency) * scaleAmplitude;

        // Apply transforms
        transform.position = new Vector3(startPos.x, newY, startPos.z);
        transform.rotation = Quaternion.Euler(0, 0, newZRot);
        transform.localScale = startScale * scaleChange;
    }
}
