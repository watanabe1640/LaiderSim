using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HUBONE.Visualize;

namespace HUBONE
{
    public class LiDAR : MonoBehaviour
    {
        private TestDrawMesh Indirectmesh;
        private List<Vector3> hitPositions;

        public float thetaRange;
        public float phirange;

        public float deltatheta;
        public float deltaphi;
        // Start is called before the first frame update
        void Start()
        {
            hitPositions = new List<Vector3>();
            GameObject drawIndirectObj = GameObject.Find("DrawIndirect");
            Indirectmesh = drawIndirectObj.GetComponent<TestDrawMesh>();
        }

        // Update is called once per frame
        void Update()
        {
            Ray2CSV();
        }

        void Ray2CSV()
        {
            hitPositions.Clear();

            Vector3 forward = this.transform.forward;
            Debug.DrawRay(this.transform.position, forward, Color.blue);

            SphericalCoordinates forvec = Convert2SphericalCoordinates(forward);


            SphericalCoordinates rayvec = Convert2SphericalCoordinates(forward);

            
            for (int i = 0; i < phirange/deltaphi; i++)
            {
                rayvec.phi = forvec.phi - (phirange/deltaphi)/2 + i * deltaphi;
                for (int j = 0; j < thetaRange/deltatheta; j++)
                {
                    rayvec.theta = forvec.theta - (thetaRange / deltatheta)/2 + j * deltatheta;
                    Debug.DrawRay(this.transform.position,
                        Convert2CatesianCoordinates(rayvec),
                        Color.blue);
                    RaycastHit hit;
                    if (Physics.Raycast(this.transform.position,Convert2CatesianCoordinates(rayvec),out hit, 10f))
                    {
                        hitPositions.Add(hit.point);
                    }
                }
            }
            Indirectmesh.DrawmeshIndirect(hitPositions);

            
        }
        // OK
        public SphericalCoordinates Convert2SphericalCoordinates(Vector3 cartesian)
        {
            SphericalCoordinates spherical = new SphericalCoordinates();

            spherical.radius = (float)Mathf.Sqrt(
                cartesian.x * cartesian.x +
                cartesian.y * cartesian.y +
                cartesian.z * cartesian.z
                );

            spherical.phi = Mathf.Asin(cartesian.y / spherical.radius) * Mathf.Rad2Deg;

            spherical.theta = Mathf.Atan(cartesian.x / cartesian.z) * Mathf.Rad2Deg;

            return spherical;
        }
        // yokunai

        public Vector3 Convert2CatesianCoordinates(SphericalCoordinates spherical)
        {
            Vector3 catesian = new Vector3(
                Mathf.Cos(spherical.phi * Mathf.Deg2Rad) * Mathf.Sin(spherical.theta * Mathf.Deg2Rad),
                Mathf.Sin(spherical.phi * Mathf.Deg2Rad),
                Mathf.Cos(spherical.phi * Mathf.Deg2Rad) * Mathf.Cos(spherical.theta * Mathf.Deg2Rad)
                ) * spherical.radius;
            return catesian;
        }
        private void OnDisable()
        {
            Indirectmesh.Disable();
        }
    }
    public class SphericalCoordinates
    {
        public float radius { get; set; }
        public float theta { get; set; }
        public float phi { get; set; }

        public SphericalCoordinates()
        {
            this.radius = 0;
            this.theta = 0;
            this.phi = 0;
        }

        SphericalCoordinates(float radius, float theta, float phi)
        {
            this.radius = radius;
            this.theta = theta;
            this.phi = phi;
        }
    }


}
