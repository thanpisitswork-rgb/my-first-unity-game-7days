using System.Collections; // ใช้สำหรับ Coroutine (ตอนนี้ยังไม่ได้ใช้ แต่เผื่ออนาคต)
using System.Collections.Generic; // ใช้กับ List / Dictionary (ตอนนี้ยังไม่ได้ใช้)
using UnityEngine; // ใช้ฟังก์ชันหลักของ Unity

// บังคับว่า GameObject นี้ต้องมี CharacterController
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    // ====== ส่วนตั้งค่า (ปรับได้ใน Inspector) ======

    public Camera playerCamera; // กล้องของผู้เล่น

    public float walkSpeed = 6f; // ความเร็วเดิน
    public float runSpeed = 12f; // ความเร็ววิ่ง
    public float jumpPower = 7f; // แรงกระโดด
    public float gravity = 10f; // ค่าแรงโน้มถ่วง

    public float lookSpeed = 2f; // ความเร็วหมุนกล้อง
    public float lookXLimit = 45f; // จำกัดมุมก้ม-เงย

    public float defaultHeight = 2f; // ความสูงปกติ
    public float crouchHeight = 1f; // ความสูงตอนย่อ
    public float crouchSpeed = 3f; // ความเร็วตอนย่อ

    // ====== ตัวแปรภายใน ======

    private Vector3 moveDirection = Vector3.zero; // เก็บทิศทางการเคลื่อนที่
    private float rotationX = 0; // มุมกล้องแกน X (ก้ม-เงย)
    private CharacterController characterController; // ตัวควบคุมการชน

    public bool canMove = true; // อนุญาตให้ผู้เล่นขยับหรือไม่

    // ====== เริ่มต้น ======
    void Start()
    {
        // ดึง CharacterController จาก GameObject
        characterController = GetComponent<CharacterController>();

        // ตอนเริ่มเกมยังไม่ให้ขยับ
        canMove = false;

        // ปลดล็อกเมาส์ (ใช้ตอนอยู่หน้าเมนู)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // ====== ทำงานทุกเฟรม ======
    void Update()
    {
        // แสดงค่า Y ของตัวละครใน Console (ใช้ debug)
        Debug.Log("Y = " + transform.position.y);

        // ถ้าตกต่ำกว่า -20 ถือว่าแพ้
        if (transform.position.y < -20f)
        {
            Debug.Log("ตกแล้ว!");

            // หา UIManager แล้วเรียก GameOver
            FindFirstObjectByType<UIManager>().GameOver();
        }

        // ====== การเคลื่อนที่ ======

        // ทิศทางด้านหน้าและด้านขวาของตัวละคร
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // เช็คว่ากด Shift เพื่อวิ่งไหม
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        // ถ้า canMove = false จะไม่ขยับเลย
        float speed = canMove ? (isRunning ? runSpeed : walkSpeed) : 0;

        // รับ input จากปุ่ม WASD
        float inputX = Input.GetAxis("Horizontal"); // ซ้าย-ขวา
        float inputZ = Input.GetAxis("Vertical");   // หน้า-หลัง

        // คำนวณทิศทางการเคลื่อนที่
        Vector3 move = (forward * inputZ + right * inputX) * speed;

        // เก็บค่าแกน Y เดิมไว้ (ใช้กับกระโดด/แรงโน้มถ่วง)
        float movementDirectionY = moveDirection.y;

        // อัปเดตทิศทางใหม่
        moveDirection = move;

        // ใส่ค่า Y กลับเข้าไป
        moveDirection.y = movementDirectionY;

        // ====== กระโดด ======

        // ถ้ากดกระโดด และอยู่บนพื้น
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower; // กระโดดขึ้น
        }
        else
        {
            moveDirection.y = movementDirectionY; // คงค่าเดิม
        }

        // ====== แรงโน้มถ่วง ======

        if (characterController.isGrounded)
        {
            moveDirection.y = -2f; // กดให้ติดพื้น (กันเด้ง)
        }
        else
        {
            moveDirection.y -= gravity * Time.deltaTime; // ตกลง
        }

        // ====== ย่อ (Crouch) ======

        if (Input.GetKey(KeyCode.R) && canMove)
        {
            characterController.height = crouchHeight; // ลดความสูง
            walkSpeed = crouchSpeed; // เดินช้าลง
            runSpeed = crouchSpeed;  // วิ่งช้าลง
        }
        else
        {
            characterController.height = defaultHeight; // กลับปกติ
            walkSpeed = 6f;
            runSpeed = 12f;
        }

        // ====== เคลื่อนที่จริง ======

        characterController.Move(moveDirection * Time.deltaTime);
        // ใช้ Move ของ CharacterController เพื่อขยับตัว

        // ====== หมุนกล้อง ======

        if (canMove)
        {
            // หมุนขึ้น-ลง
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;

            // จำกัดมุม
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

            // หมุนกล้อง
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

            // หมุนตัวละครซ้าย-ขวา
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }
}