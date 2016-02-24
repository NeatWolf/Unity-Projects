using UnityEngine;
using System.Collections;

public class SpaceDust : MonoBehaviour {

    public int particleCount;
    public float particleSize;
    public float clusterSize;

    private Transform location;
    private ParticleSystem ps;
    private ParticleSystem.Particle[] points;
    private float clusterSizeSqr;

	void Start ()
    {
        ps = GetComponent<ParticleSystem>();
        location = transform;
        clusterSizeSqr = clusterSize * clusterSize;
	}
	
	void Update ()
    {
	    if(points == null)
        {
            CreateParticles();
        }

        for(int i = 0; i < particleCount; i++)
        {
            if ((points[i].position - location.position).sqrMagnitude > clusterSizeSqr)
            {
                points[i].position = Random.insideUnitSphere * clusterSize + location.position;
            }
        }

        ps.SetParticles(points, points.Length);
	}

    private void CreateParticles()
    {
        points = new ParticleSystem.Particle[particleCount];

        for (int i = 0; i < particleCount; i++)
        {
            points[i].position = Random.insideUnitSphere * clusterSize + location.position;
            points[i].startColor = new Color(1, 1, 1, 1);
            points[i].startSize = particleSize;
        }
    }
}
