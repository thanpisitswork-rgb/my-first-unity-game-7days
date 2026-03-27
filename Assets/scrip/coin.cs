using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value = 1;
    public AudioClip coinSound;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 🔊 เล่นเสียง
            AudioSource audio = other.GetComponent<AudioSource>();
            if (audio != null && coinSound != null)
            {
                audio.PlayOneShot(coinSound);
            }

            // ➕ เพิ่มคะแนน
            FindAnyObjectByType<UIManager>().AddScore(value);

            // 🪙 spawn เหรียญใหม่
            CoinSpawner spawner = FindFirstObjectByType<CoinSpawner>();
            if (spawner != null)
            {
                spawner.currentCoins--; // ลดก่อน
                spawner.SpawnCoin();    // spawn ใหม่
            }

            // 💥 ลบเหรียญ
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // 🌀 หมุนเหรียญ
        transform.Rotate(0, 100 * Time.deltaTime, 0);
    }
}