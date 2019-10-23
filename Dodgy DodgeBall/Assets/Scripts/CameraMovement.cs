using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform m_pivot = null;
    public Transform m_reference = null;
    // Update is called once per frame
    void LateUpdate()
    {
        if(m_pivot != null)
        {
            var tempRotation = transform.rotation;
            transform.position = new Vector3(m_pivot.position.x,
                                             m_pivot.position.y + 3,
                                             m_pivot.position.z - 5);
            transform.rotation = m_reference.transform.rotation;
        }
    }
}
