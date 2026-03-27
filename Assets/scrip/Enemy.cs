using UnityEngine; // ใช้ระบบหลักของ Unity

public class Enemy : MonoBehaviour // คลาสศัตรู
{
    // ====== ตั้งค่า ======

    public float speed = 2f; // ความเร็วในการเดิน
    public float stopDistance = 1.5f; // ระยะหยุด (ตอนนี้ยังไม่ได้ใช้)

    // ====== ตัวแปร ======

    private Transform player; // เก็บตำแหน่ง player

    // ====== เริ่มต้น ======

    void Start()
    {
        // หา player จาก tag
        GameObject p = GameObject.FindGameObjectWithTag("Player");

        if (p != null)
        {
            player = p.transform; // เก็บ transform ไว้ใช้
        }
        else
        {
            Debug.LogWarning("ไม่เจอ Player!");
        }
    }

    // ====== ทำงานทุกเฟรม ======

    void Update()
    {
        // ถ้า player หาย (กัน error)
        if (player == null) return;

        // ====== คำนวณทิศทาง ======

        Vector3 direction = player.position - transform.position; // หาทิศไปหา player
        direction.y = 0; // ไม่ให้ลอยขึ้น-ลง

        // ====== เดินเข้าหา player ======

        float distance = direction.magnitude; // ระยะห่าง

        if (distance > stopDistance) // ถ้ายังไม่ถึงระยะหยุด
        {
            transform.position += direction.normalized * speed * Time.deltaTime;
            // เดินเข้าหา player
        }

        // ====== ล็อคให้อยู่บนพื้น ======

        transform.position = new Vector3(
            transform.position.x,
            2f, // ความสูง fix (ระวังถ้าพื้นไม่เท่ากัน)
            transform.position.z
        );

        // ====== หันหน้าหาผู้เล่น ======

        Vector3 lookPos = player.position - transform.position; // มองไปที่ player
        lookPos.y = 0; // ไม่ให้ก้ม-เงย

        if (lookPos != Vector3.zero)
        {
            transform.forward = lookPos; // หันหน้าไปหา player
        }

        // ❌ ของเดิมคุณหันเข้ากล้อง → ผมแก้ให้หันหา player (ธรรมชาติกว่า)
    }

    // ====== ชนผู้เล่น ======

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // ถ้าชน player
        {
            // เรียก Game Over
            FindFirstObjectByType<UIManager>().GameOver();
        }
    }
}