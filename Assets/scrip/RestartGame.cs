using UnityEngine; // ใช้ระบบหลักของ Unity
using UnityEngine.SceneManagement; // ใช้สำหรับโหลด/เปลี่ยน Scene

public class RestartGame : MonoBehaviour // คลาสสำหรับรีสตาร์ทเกม
{
    // ฟังก์ชันนี้จะถูกเรียกจากปุ่ม UI (OnClick)
    public void Restart()
    {
        // แสดง log ใน Console เพื่อเช็คว่าปุ่มถูกกดจริง
        Debug.Log("CLICKED!!!"); 
        Debug.Log("RESTART WORKING");

        // รีเซ็ต TimeScale กลับเป็นปกติ (กันกรณีเกมเคย pause ไว้)
        Time.timeScale = 1f;

        // โหลด Scene ปัจจุบันใหม่ (รีสตาร์ทเกม)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}