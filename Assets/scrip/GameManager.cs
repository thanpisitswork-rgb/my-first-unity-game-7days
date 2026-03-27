using UnityEngine; // ใช้ฟังก์ชันหลักของ Unity

public class GameManager : MonoBehaviour // คลาสจัดการระบบหลักของเกม
{
    public static int score = 0; // คะแนนรวมของผู้เล่น (static = ใช้ร่วมกันทุกที่)

    public GameObject winText; // UI ข้อความชนะ
    public GameObject restartButton; // ปุ่ม Restart

    void Start()
    {
        score = 0; // รีเซ็ตคะแนนตอนเริ่มเกมใหม่

        winText.SetActive(false); // ซ่อนข้อความชนะ
        restartButton.SetActive(false); // ซ่อนปุ่มรีสตาร์ท
    }

    // ฟังก์ชันเพิ่มคะแนน
    public void AddScore(int amount)
    {
        score += amount; // เพิ่มคะแนนตามจำนวนที่รับมา

        Debug.Log("Score: " + score); // แสดงคะแนนใน Console (debug)

        // ถ้าคะแนนถึง 10 ถือว่าชนะ
        if (score >= 10)
        {
            winText.SetActive(true); // แสดงข้อความชนะ
            restartButton.SetActive(true); // แสดงปุ่มรีสตาร์ท

            Time.timeScale = 0f; // หยุดเวลาในเกม (เหมือน pause)
        }
    }
}