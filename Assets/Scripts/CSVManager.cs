using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace HUBONE
{
    public class CSVManager : MonoBehaviour
    {
        private StreamWriter sw;

        private GameObject capsule;

        private LiDAR lidar;

        // Start is called before the first frame update
        void Start()
        {
            capsule = GameObject.Find("Capsule");

            GameObject lidarObj = GameObject.Find("LiDAR");
            lidar = lidarObj.GetComponent<LiDAR>();

            InitializeCSV();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                WriteCSV();
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                sw.Close();
            }
        }

        void InitializeCSV()
        {
            sw = new StreamWriter(@"SaveData.csv", false, Encoding.GetEncoding("Shift_JIS"));

            string[] s1 = { "x", "y", "z" };
            string s2 = string.Join(",", s1);
            sw.WriteLine(s2);

            Vector3 point = capsule.transform.position;
            string[] s_in = { $"{point.x}", $"{point.y}", $"{point.z}" };
            string s_out = string.Join(",", s_in);
            sw.WriteLine(s_out);
        }
        void WriteCSV()
        {
            Vector3[] positions = lidar.hitPositions.ToArray();
            foreach (Vector3 point in positions)
            {
                string[] s_in = { $"{point.x}", $"{point.y}", $"{point.z}" };
                string s_out = string.Join(",", s_in);
                sw.WriteLine(s_out);
            }
        }
    }

}
