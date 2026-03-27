using UnityEngine; // ใช้ระบบหลักของ Unity
using TMPro; // ใช้ TextMeshPro สำหรับ UI ข้อความ

public class UIManager : MonoBehaviour // คลาสจัดการ UI ทั้งเกม
{
    // ====== UI ELEMENTS ======

    public TextMeshProUGUI scoreText; // ข้อความแสดงคะแนน
    public GameObject winText; // ข้อความ "ชนะ"
    public GameObject startText; // ข้อความ "กดเริ่ม"
    public GameObject restartButton; // ปุ่ม Restart
    public GameObject gameOverText; // ข้อความ Game Over

    public TextMeshProUGUI timerText; // ข้อความแสดงเวลา

    // ====== REFERENCE ======

    public PlayerMovement player; // อ้างอิงไปยัง PlayerMovement

    // ====== STATE ======

    private bool gameStarted = false; // เกมเริ่มหรือยัง
    private bool isGameRunning = false; // เกมกำลังนับเวลาอยู่ไหม

    private int score = 0; // คะแนนปัจจุบัน
    private float timeElapsed = 0f; // เวลาที่ผ่านไป

    // ====== เริ่มเกม ======

    void Start()
    {
        // หา PlayerMovement ในฉาก (ตัวแรกที่เจอ)
        player = FindFirstObjectByType<PlayerMovement>();

        // ซ่อน UI ที่ยังไม่ใช้
        gameOverText.SetActive(false); // ซ่อน Game Over
        winText.SetActive(false); // ซ่อน Win
        restartButton.SetActive(false); // ซ่อนปุ่มรีสตาร์ท

        // แสดงข้อความเริ่มเกม
        startText.SetActive(true);

        // หยุดเวลาไว้ก่อน (เกมยังไม่เริ่ม)
        Time.timeScale = 0f;

        // รีเซ็ตเวลา
        timeElapsed = 0f;

        // ❗ ยังไม่เริ่มนับเวลา
        isGameRunning = false;
    }

    // ====== ทำงานทุกเฟรม ======

    void Update()
    {
        // ====== เริ่มเกมเมื่อกดปุ่ม ======
        if (!gameStarted && Input.anyKeyDown)
        {
            gameStarted = true; // ตั้งสถานะว่าเริ่มแล้ว

            startText.SetActive(false); // ซ่อนข้อความเริ่ม

            // ล็อกเมาส์เข้ากลางจอ
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Time.timeScale = 1f; // ให้เวลาเดิน (เริ่มเกม)

            isGameRunning = true; // เริ่มนับเวลา

            if (player != null)
            {
                player.canMove = true; // ปลดล็อกการควบคุม
            }
        }

        // ====== ระบบจับเวลา ======
        if (isGameRunning)
        {
            timeElapsed += Time.deltaTime; // เพิ่มเวลาทีละ frame

            int minutes = Mathf.FloorToInt(timeElapsed / 60); // นาที
            int seconds = Mathf.FloorToInt(timeElapsed % 60); // วินาที

            // แสดงเวลาแบบ 00:00
            timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        }
    }

    // ====== เพิ่มคะแนน ======

    public void AddScore(int amount)
    {
        score += amount; // เพิ่มคะแนน

        scoreText.text = "Score: " + score; // อัปเดต UI

        // ถ้าคะแนนถึง 10 = ชนะ
        if (score >= 10)
        {
            isGameRunning = false; // หยุดเวลา

            winText.SetActive(true); // แสดง Win
            restartButton.SetActive(true); // แสดงปุ่ม Restart

            Time.timeScale = 0f; // หยุดเกม

            // ปลดล็อกเมาส์
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (player != null)
            {
                player.enabled = false; // ปิดการควบคุมผู้เล่น
            }
        }
    }

    // ====== แพ้ ======

    public void GameOver()
    {
        isGameRunning = false; // หยุดเวลา

        gameOverText.SetActive(true); // แสดง Game Over
        winText.SetActive(false); // ซ่อน Win

        restartButton.SetActive(true); // แสดงปุ่ม Restart

        Time.timeScale = 0f; // หยุดเกม

        // ปลดล็อกเมาส์
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (player != null)
        {
            player.enabled = false; // ปิดการควบคุม
        }
    }
}