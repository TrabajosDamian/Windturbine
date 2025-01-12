using UnityEngine;
using Obi;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(ObiSolver))]
public class WindTurbine : MonoBehaviour
{
    private float vel = 2.2f;
    private float velocity = 3.3f;
    public TextManager textManager;
    ObiSolver solver;
    //public Slider slider;
    public ObiEmitter emitter;
    public static WindTurbine Instance = null;
    void Awake()
    {
        solver = GetComponent<Obi.ObiSolver>();
        Instance = this;
        //slider.onValueChanged.AddListener(ChangeVelocityWind);
    }
    private void Start()
    {
        // slider = GetComponent<Slider>();
        /// textManager = GetComponent<TextManager>();
    }
    void OnEnable()
    {
        solver.OnCollision += Solver_OnCollision;
    }

    void OnDisable()
    {
        solver.OnCollision -= Solver_OnCollision;
    }

    public void Solver_OnCollision(object sender, Obi.ObiSolver.ObiCollisionEventArgs e)
    {
        var world = ObiColliderWorld.GetInstance();
        foreach (Oni.Contact contact in e.contacts)
        {
            if (contact.distance < 0.01)
            {
                ObiColliderBase collider = world.colliderHandles[contact.other].owner;
                if (collider != null)
                {
                    //Debug.Log("CHOCAMOS: " + collider.tag);
                    //ObiActor actor = pa.actor;
                    if (collider.gameObject.CompareTag("WindTurbine") && vel >= 2.2f && vel <= 8)
                    {
                        //Debug.Log("VELOCIDAD: " + actor.solver.velocities);

                        var parent = collider.gameObject.transform;
                        Vector3 vectorTurbine = parent.forward;
                        Vector3 vectorWind = emitter.transform.forward;

                        float a = Vector3.Cross(vectorTurbine, vectorWind).magnitude;

                        var b = 1 - a;

                        if (a == 0)
                        {
                            Debug.Log("Está en la posición óbtima");
                        }
                        else if (a == 1f)
                        {
                            Debug.Log("Está en la peor posicion posible");
                        }
                        else
                        {
                            Debug.Log("No está en la posicion obtima: " + a + " vectores: " + Vector3.Cross(vectorTurbine, vectorWind));
                        }

                        var rotation = 0.085f * vel - 0.18f;
                        parent.GetChild(0).Rotate(-rotation, 0, 0);
                        parent.GetChild(1).Rotate(-rotation, 0, 0);
                        parent.GetChild(2).Rotate(-rotation, 0, 0);

                        double energy = 0f;
                        double AreaWindturbine = Mathf.Pow(45, 2) * Mathf.PI;

                        energy = (1.225f * 0.5 * Mathf.Pow(velocity, 3) * AreaWindturbine * b) / 1000;
                        float speed = 0.9f * velocity + 10;
                        textManager.WriterMessageEnergy("Potencia generada (KW): " + Math.Round(energy, 2));
                        textManager.WriterMessageSpeed("Velocidad de giro: " + Math.Round(speed, 2));
                    }
                }
            }
        }
    }



}
