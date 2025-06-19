using UnityEngine;

// Script buat burung biar keliatan terbang, padahal cuma 1 gambar doang
public class BirdFly : MonoBehaviour
{
    [Header("Gerakan Horizontal")]
    public float speed = 2f; // buat ngatur seberapa cepet burungnya jalan ke kanan

    [Header("Gerakan Naik-Turun (Gelombang)")]
    public float amplitude = 0.5f; // tinggi gelombangnya, makin gede makin naik-turun
    public float frequency = 2f;   // seberapa cepet naik-turunnya, makin gede makin cepet goyang

    private Vector3 startPos; // buat nyimpen posisi awal burung

    void Start()
    {
        startPos = transform.position; // simpen posisi awal, biar nanti naik-turunnya dari sini
    }

    void Update()
    {
        // gerakin burung ke kanan terus, tiap frame nambah dikit
        float moveX = speed * Time.deltaTime;

        // buat efek naik-turun, pake sinus biar halus kaya burung beneran
        float moveY = Mathf.Sin(Time.time * frequency) * amplitude;

        // update posisi burung, X-nya maju, Y-nya naik-turun, Z-nya tetep aja
        transform.position = new Vector3(
            transform.position.x + moveX, // maju ke kanan
            startPos.y + moveY,           // naik-turun dari posisi awal
            transform.position.z          // z-nya ga usah diapa-apain
        );
    }
}
