 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [Header("Attached Camera")]
    public Camera m_AttachedCamera;

    [System.Serializable]
    public struct CameraState
    {
        public float fieldOfView;
        public Vector3 position;
        public Vector3 eulerAngles;
        public float lookDistance;
    }

    [Header("States")]
    public CameraState[] m_States;
    public int m_CurrentStateIndex;
    
    [Header("Motion")]
    public float m_FieldOfViewLerp = 8.0f;
    public float m_PositionLerp = 8.0f;
    public float m_RotationLerp = 8.0f;

    [Header("UI Elements")]
    public Text m_CameraReadout;
    public Button[] m_CameraButtons;

    private Vector3 m_LookPosition;

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < m_States.Length; i++)
        {
            DrawGizmoCamera(m_States[i]);
        }
    }

    private void DrawGizmoCamera(CameraState _state)
    {
        float radius = 0.25f;
        Quaternion rot = Quaternion.Euler(_state.eulerAngles);

        Gizmos.color = new Color(1.0f, 1.0f, 0.1f, 1.0f);
        DrawGizmoRing(_state.position, radius, Vector3.forward, Vector3.up, rot, 36);
        DrawGizmoRing(_state.position, radius, Vector3.up, Vector3.forward, rot, 36);
        DrawGizmoRing(_state.position, radius, Vector3.up, Vector3.right, rot, 36);

        Vector3[] arrowPoints = new Vector3[]
        {
            new Vector3(0.0f, 0.0f, radius),
            new Vector3(0.0f, 0.0f, _state.lookDistance - 0.1f),
            new Vector3(0.0f, 0.0f, _state.lookDistance),
            new Vector3(-0.1f, 0.0f, _state.lookDistance - 0.1f),
            new Vector3(0.1f, 0.0f, _state.lookDistance - 0.1f)
        }; 

        for (int i = 0; i < arrowPoints.Length; i++)
        {
            arrowPoints[i] = _state.position + Quaternion.Euler(_state.eulerAngles) * arrowPoints[i];
        }

        Gizmos.color = new Color(1.0f, 1.0f, 0.1f, 0.25f);
        Gizmos.DrawLine(arrowPoints[0], arrowPoints[1]);
        Gizmos.DrawLine(arrowPoints[2], arrowPoints[3]);
        Gizmos.DrawLine(arrowPoints[2], arrowPoints[4]);
        Gizmos.DrawLine(arrowPoints[3], arrowPoints[4]);
    }

    private void DrawGizmoRing(Vector3 _pos, float _radius, Vector3 _fwd, Vector3 _up, Quaternion _rot, int _vertices)
    {
        Vector3 point1 = _fwd * _radius;
        Vector3 point2 = point1;
        Quaternion rotationStep = Quaternion.Euler(_up * 360.0f / (float)_vertices);
        for (int i = 0; i <= _vertices; i++)
        {
            point2 = rotationStep * point1;
            Gizmos.DrawLine(_pos + (_rot * point1), _pos + (_rot * point2));
            point1 = point2;
        }
    }

    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            m_AttachedCamera.fieldOfView = m_States[m_CurrentStateIndex].fieldOfView;
            transform.position = m_States[m_CurrentStateIndex].position;
            
            m_LookPosition = Quaternion.Euler(m_States[m_CurrentStateIndex].eulerAngles) * Vector3.forward * m_States[m_CurrentStateIndex].lookDistance;
            transform.rotation = Quaternion.LookRotation(m_LookPosition, Vector3.up);        
        }   
    }

    public void ChangeIndex(int _newIndex)
    {
        m_CurrentStateIndex = _newIndex;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            m_CurrentStateIndex = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            m_CurrentStateIndex = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            m_CurrentStateIndex = 2;
        }

        for (int i = 0; i < m_CameraButtons.Length; i++)
        {
            m_CameraButtons[i].interactable = i != m_CurrentStateIndex; 
        }

        m_CameraReadout.text = "- " + (m_CurrentStateIndex + 1) + " -";
    }

    private void LateUpdate()
    {
        m_AttachedCamera.fieldOfView = Mathf.Lerp(m_AttachedCamera.fieldOfView, m_States[m_CurrentStateIndex].fieldOfView, m_FieldOfViewLerp * Time.unscaledDeltaTime);
        transform.position = Vector3.Lerp(transform.position, m_States[m_CurrentStateIndex].position, m_PositionLerp * Time.unscaledDeltaTime);

        Vector3 targetLook = Quaternion.Euler(m_States[m_CurrentStateIndex].eulerAngles) * Vector3.forward * m_States[m_CurrentStateIndex].lookDistance;
        m_LookPosition = Vector3.Lerp(m_LookPosition, targetLook, m_RotationLerp * Time.unscaledDeltaTime);
        transform.rotation = Quaternion.LookRotation(m_LookPosition, Vector3.up);
    }
}
