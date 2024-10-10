using System;
using UnityEngine;

namespace NoC.Studios.GeoPhysX
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] float m_moveSpeed = 30f;
        Rigidbody m_rigidBody;

        void Start()
        {
            m_rigidBody = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            if (Input.GetKey(KeyCode.A))
            {
                m_rigidBody.AddForce(Vector3.left * (m_moveSpeed * Time.fixedDeltaTime));
            }

            if (Input.GetKey(KeyCode.D))
            {
                m_rigidBody.AddForce(Vector3.right * (m_moveSpeed * Time.fixedDeltaTime));
            }
        }
    }
}
