using UnityEngine;

public class CoinMove : MonoBehaviour
{
    public float speed = 3f;

    void Update()
    {
        // Bergerak ke kiri
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        // Hancurkan coin jika sudah keluar layar (posisi kiri)
        if (transform.position.x < -10f)
        {
            Destroy(gameObject);
        }
    }
}