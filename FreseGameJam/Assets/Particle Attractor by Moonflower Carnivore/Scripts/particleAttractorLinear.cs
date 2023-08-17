using System.Collections;
using UnityEngine;
[RequireComponent(typeof(ParticleSystem))]
public class particleAttractorLinear : MonoBehaviour {
	ParticleSystem ps;
	ParticleSystem.Particle[] m_Particles;
	public Transform target;
	public float speed = 5f;
	int numParticlesAlive;
	public float destroyThreshold = 0.1f;
	[SerializeField] float particleDestroyCounter;
	private int counter = 0;
	[SerializeField] float acceleration = 0.1f;

	void Start () {
		ps = GetComponent<ParticleSystem>();
		if (!GetComponent<Transform>()){
			GetComponent<Transform>();
		}
	}
	void Update () {
		m_Particles = new ParticleSystem.Particle[ps.main.maxParticles];
		numParticlesAlive = ps.GetParticles(m_Particles);
		float step = speed * Time.deltaTime;
		for (int i = 0; i < numParticlesAlive; i++) {

			m_Particles[i].position = Vector3.LerpUnclamped(m_Particles[i].position, target.position, step);
			if (Vector3.Distance(m_Particles[i].position, target.position) < destroyThreshold)
			{
				counter++;
				m_Particles[i].remainingLifetime = 0;
				Debug.Log(m_Particles[i]);
				if (counter == particleDestroyCounter && Vector3.Distance(m_Particles[i].position, target.position) < destroyThreshold)
				{
					
					Destroy(this.gameObject);
					break;

				}
			}
		}
		ps.SetParticles(m_Particles, numParticlesAlive);
		speed += acceleration * Time.deltaTime;
	}
}
