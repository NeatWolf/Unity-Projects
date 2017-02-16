using UnityEngine;
using System.Collections;

public class SpaceDust : MonoBehaviour {

    [SerializeField]
    private int particleCount;
    [SerializeField]
    private float particleSize;
    [SerializeField]
    private float clusterSize;

    private Transform _location;
    private ParticleSystem _ps;
    private ParticleSystem.Particle[] _points;
    private float _clusterSizeSqr;

	void Start ()
    {
        _ps = GetComponent<ParticleSystem>();
        _location = transform;
        _clusterSizeSqr = clusterSize * clusterSize;
	}
	
	void Update ()
    {
	    if(_points == null)
        {
            CreateParticles();
        }

        for(int i = 0; i < particleCount; i++)
        {
            if ((_points[i].position - _location.position).sqrMagnitude > _clusterSizeSqr)
            {
                _points[i].position = Random.insideUnitSphere * clusterSize + _location.position;
            }
        }

        _ps.SetParticles(_points, _points.Length);
	}

    private void CreateParticles()
    {
        _points = new ParticleSystem.Particle[particleCount];

        for (int i = 0; i < particleCount; i++)
        {
            _points[i].position = Random.insideUnitSphere * clusterSize + _location.position;
            _points[i].startColor = new Color(1, 1, 1, 1);
            _points[i].startSize = particleSize;
        }
    }
}
