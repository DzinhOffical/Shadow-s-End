using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Transform cameraTransform; // Camera chính
    public float[] parallaxFactors;   // Mảng chứa các hệ số parallax cho từng lớp nền (0-1)
    public Transform[] backgrounds;   // Mảng chứa các Transform của các lớp nền

    private Vector3 lastCameraPosition; // Vị trí trước đó của camera

    void Start()
    {
        // Lấy vị trí ban đầu của camera
        lastCameraPosition = cameraTransform.position;
    }

    void Update()
    {
        // Tính toán khoảng cách camera đã di chuyển
        float deltaX = cameraTransform.position.x - lastCameraPosition.x;

        // Di chuyển từng lớp nền theo hệ số parallax tương ứng
        for (int i = 0; i < backgrounds.Length; i++)
        {
            float parallax = deltaX * parallaxFactors[i];
            backgrounds[i].position += new Vector3(parallax, 0, 0);
        }

        // Cập nhật lại vị trí của camera
        lastCameraPosition = cameraTransform.position;
    }
}
