using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiDAR : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray2CSV();
    }

    void Ray2CSV()
    {
        //RaycastHit hit;
        
        Vector3 forward = this.transform.forward;
        Debug.DrawRay(this.transform.position, forward , Color.blue);

        SphericalCoordinates forvec = Convert2SphericalCoordinates(forward);

        
        SphericalCoordinates rayvec = Convert2SphericalCoordinates(forward);
        
        for (int i = 0;i < 10; i++)
        {
            rayvec.phi = forvec.phi + i * 0.05f;
            for (int j = 0; j < 10; j++)
            {
                rayvec.theta = forvec.theta + j * 0.05f;
                Debug.DrawRay(this.transform.position,
                    Convert2CatesianCoordinates(rayvec),
                    Color.blue);
            }
        }
        

        /*
        if (Physics.Raycast(front_ray,out hit))
        {
            //Debug.DrawRay(this.transform.position, forward * 10,Color.blue);
        }
        */
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

        spherical.phi = Mathf.Asin(cartesian.y / spherical.radius);

        spherical.theta = Mathf.Atan(cartesian.x / cartesian.z);
        
        return spherical;
    }
    // yokunai

    public Vector3 Convert2CatesianCoordinates(SphericalCoordinates spherical)
    {
        Vector3 catesian = new Vector3(
            Mathf.Cos(spherical.phi) * Mathf.Sin(spherical.theta),
            Mathf.Sin(spherical.phi),
            Mathf.Cos(spherical.phi) * Mathf.Cos(spherical.theta)
            ) * spherical.radius;
        return catesian;
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

