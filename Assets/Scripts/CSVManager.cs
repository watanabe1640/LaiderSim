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

        public GameObject capsule;

        public int startCSVnum;
        int startnum; 

        private LiDAR lidar;

        private int time_count;

        // Start is called before the first frame update
        void Start()
        {
            time_count = 0;

            GameObject lidarObj = GameObject.Find("LiDAR");
            lidar = lidarObj.GetComponent<LiDAR>();

            startnum = 0;
        }

        // Update is called once per frame
        void Update()
        {
            if (time_count == 60)
            {
                InitializeCSV();
                MoveCapsule();
                WriteCSV();
                sw.Close();
                time_count = 0;
                startnum++;
            }
            time_count++;
        }

        void InitializeCSV()
        {
            sw = new StreamWriter($@"Assets/CSVs/capsule{startCSVnum+startnum}.csv", false, Encoding.GetEncoding("Shift_JIS"));

            string s2 = "x,y,z";
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
        void MoveCapsule()
        {
            Vector3 position = new Vector3(
                Random.Range(-0.4f, 0.4f), 
                Random.Range(0.45f, 2.1f),
                Random.Range(-0.4f, 0.4f));
            Vector3 rot = new Vector3(
                Random.Range(0.0f, 360.0f),
                Random.Range(0.0f, 360.0f),
                Random.Range(0.0f, 360.0f));
            capsule.transform.position = position;
            capsule.transform.rotation = Quaternion.Euler(rot);
        }
    }

}
