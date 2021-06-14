using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualEffectManager : MonoBehaviour
{
    [Header("Attached Animator")]
    public Animator m_Animator;

    [Header("Camera Shake")]
    public float m_CameraShake = 0.0f;
    public float m_CameraShakeStrength = 4.0f;
    public Camera m_AttachedCamera;

    [Header("UI Elements")]
    public Button m_FireButton;

    private void ShakeCamera()
    {
        float x = Random.Range(0.0f, 360.0f);
        float y = Random.Range(0.0f, 360.0f);
        float z = Random.Range(0.0f, 360.0f);
        Vector3 shake = Quaternion.Euler(x, y, z) * Vector3.forward;
        m_AttachedCamera.transform.position += shake * m_CameraShakeStrength * m_CameraShake * Time.deltaTime;
    }

    public void Fire()
    {
        if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Fire"))
        {
            return;
        }

        m_Animator.SetTrigger("Fire");
    }

    private void Update()
    {
        m_FireButton.interactable = !m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Fire");

        if (Input.GetKeyDown(KeyCode.F))
        {
            Fire();
        }
    }

    private void LateUpdate()
    {
        ShakeCamera();
    }
}
