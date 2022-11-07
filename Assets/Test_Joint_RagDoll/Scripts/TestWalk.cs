namespace TestRagDoll
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class TestWalk : MonoBehaviour
    {
        float timer;
        bool flag = false;
        Rigidbody rigidbody;
        // Start is called before the first frame update
        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        private void Update()
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0.3f;
                flag = !flag;
            }
            if (Input.GetKey(KeyCode.P))
            {
                if (flag)
                {
                    rigidbody.AddTorque(Vector3.right * Time.deltaTime * 50000, ForceMode.Acceleration);
                }
                else
                {
                    rigidbody.AddTorque(-Vector3.right * Time.deltaTime * 50000, ForceMode.Acceleration);
                }

            }
        }

    }
}
