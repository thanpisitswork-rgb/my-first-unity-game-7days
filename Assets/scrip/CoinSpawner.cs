using UnityEngine; // ใช้ระบบหลักของ Unity

public class CoinSpawner : MonoBehaviour // คลาสสำหรับสุ่มเกิดเหรียญ
{
    // ====== PREFAB ======

    public GameObject coinPrefab; // ตัวเหรียญที่จะ spawn

    // ====== พื้นที่ spawn ======

    public float minX = -10f; // ขอบเขตซ้าย
    public float maxX = 10f;  // ขอบเขตขวา
    public float minZ = -10f; // ขอบเขตล่าง
    public float maxZ = 10f;  // ขอบเขตบน
    public float y = 1f; // ความสูงของเหรียญ

    // ====== เงื่อนไข ======

    public float minDistanceFromPlayer = 5f; // ระยะห่างจาก player ขั้นต่ำ
    public float minDistanceBetweenCoins = 2f; // ระยะห่างระหว่างเหรียญ

    public int maxAttempts = 20; // จำนวนครั้งสูงสุดในการสุ่ม (กัน loop ค้าง)

    // ====== จำนวนเหรียญ ======

    public int startCoinCount = 5; // จำนวนเหรียญตอนเริ่มเกม
    public int maxCoins = 5; // จำนวนเหรียญสูงสุดในฉาก
    public int currentCoins = 0; // จำนวนเหรียญปัจจุบัน

    // ====== เริ่มเกม ======

    void Start()
    {
        // สร้างเหรียญตอนเริ่มตามจำนวนที่กำหนด
        for (int i = 0; i < startCoinCount; i++)
        {
            SpawnCoin(); // เรียกฟังก์ชัน spawn
        }
    }

    // ====== ฟังก์ชัน spawn ======

    public void SpawnCoin()
    {
        // ถ้าเหรียญเต็มแล้ว ไม่ต้อง spawn เพิ่ม
        if (currentCoins >= maxCoins) return;

        Debug.Log("SPAWN CALLED"); // debug ว่าฟังก์ชันถูกเรียก

        // หา player จาก tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // ลองสุ่มตำแหน่งหลายครั้ง (กันสุ่มแล้วชนเงื่อนไขตลอด)
        for (int i = 0; i < maxAttempts; i++)
        {
            // สุ่มตำแหน่ง X,Z
            float randomX = Random.Range(minX, maxX);
            float randomZ = Random.Range(minZ, maxZ);

            // สร้างตำแหน่ง spawn
            Vector3 spawnPos = new Vector3(randomX, y, randomZ);

            // ====== เช็คระยะจาก player ======

            if (player != null) // ถ้ามี player อยู่
            {
                float distToPlayer = Vector3.Distance(spawnPos, player.transform.position);

                if (distToPlayer < minDistanceFromPlayer)
                    continue; // ใกล้เกิน → ข้ามแล้วสุ่มใหม่
            }

            // ====== เช็คระยะจากเหรียญอื่น ======

            // ตรวจสอบ collider รอบๆ จุด spawn
            Collider[] colliders = Physics.OverlapSphere(spawnPos, minDistanceBetweenCoins);

            bool tooClose = false; // flag ว่าใกล้เกินไหม

            foreach (Collider col in colliders)
            {
                if (col.CompareTag("Coin")) // ถ้าเจอเหรียญอื่น
                {
                    tooClose = true;
                    break;
                }
            }

            if (tooClose)
                continue; // ใกล้เกิน → สุ่มใหม่

            // ====== ผ่านทุกเงื่อนไข ======

            Instantiate(coinPrefab, spawnPos, Quaternion.identity); // สร้างเหรียญ
            currentCoins++; // เพิ่มจำนวนเหรียญ

            return; // ออกจากฟังก์ชัน (ไม่ต้องสุ่มต่อ)
        }

        // ถ้าสุ่มครบแล้วยังไม่ได้ตำแหน่ง
        Debug.Log("หาที่วางเหรียญไม่ได้ 😢");
    }
}