using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float acceleration = 5f;
    public float reverseAcceleration = 3f;
    public float turnStrength = 100f;

    [Header("Sensor Settings")]
    public int sensorCount = 17; // 17 sensores + velocidade = 18 inputs
    public float maxSensorDistance = 10f;
    public float sensorStep = 0.2f; // distância percorrida por passo

    [HideInInspector] public double fitness = 0f;
    [HideInInspector] public bool reachedGoal = false;

    private Rigidbody2D rb;
    private Transform[] sensors;
    private bool isDead = false;
    private Vector2 lastPosition;
    private float idleTime = 0f;

    [HideInInspector] public NeuralNetwork brain;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        sensors = new Transform[sensorCount];
        float startAngle = -90f;
        float angleStep = 180f / (sensorCount - 1);

        for (int i = 0; i < sensorCount; i++)
        {
            GameObject sensorObj = new GameObject($"Sensor_{i}");
            sensorObj.transform.parent = transform;
            sensorObj.transform.localPosition = Vector3.zero;
            sensorObj.transform.localRotation = Quaternion.Euler(0, 0, startAngle + i * angleStep);
            sensors[i] = sensorObj.transform;
        }
    }

    void Start()
    {
        if (brain == null)
            brain = new NeuralNetwork();

        lastPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (isDead || reachedGoal || NavigationMapGenerator.Instance == null)
            return;

        int waveCost = NavigationMapGenerator.Instance.GetWaveValue(transform.position);
        int maxCost = NavigationMapGenerator.Instance.maxWaveValue;
        fitness += (waveCost >= 0)
            ? (maxCost - waveCost) * Time.fixedDeltaTime * 1.5f + Time.fixedDeltaTime * 7f
            : Time.fixedDeltaTime;

        float moved = Vector2.Distance(transform.position, lastPosition);
        if (moved < 0.05f)
        {
            idleTime += Time.fixedDeltaTime;
            if (idleTime > 3f)
            {
                Die();
                return;
            }
        }
        else idleTime = 0f;
        lastPosition = transform.position;

        double[] inputs = new double[18];
        for (int i = 0; i < sensorCount; i++)
        {
            inputs[i] = SimulateSensor(sensors[i].position, sensors[i].up);
        }

        float speed = rb.linearVelocity.magnitude;
        float maxSpeed = acceleration + reverseAcceleration;
        inputs[17] = Mathf.Clamp01(speed / maxSpeed);

        double[] outputs = brain.FeedForward(inputs);

        bool goF = outputs[0] > 0.5f;
        bool goB = outputs[1] > 0.5f;
        bool turnL = outputs[2] > 0.5f;
        bool turnR = outputs[3] > 0.5f;

        float move = 0f;
        if (goF) move += acceleration;
        if (goB) move -= reverseAcceleration;
        rb.linearVelocity = transform.up * move;

        float turn = (turnL ? 1f : 0f) + (turnR ? -1f : 0f);
        rb.MoveRotation(rb.rotation + turn * turnStrength * Time.fixedDeltaTime);
    }

    double SimulateSensor(Vector2 origin, Vector2 direction)
    {
        Vector2 pos = origin;
        float distance = 0f;

        while (distance < maxSensorDistance)
        {
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0.01f);
            if (hit.collider != null && hit.collider.CompareTag("Wall"))
                break;

            pos += direction * sensorStep;
            distance += sensorStep;
        }

        Debug.DrawLine(origin, origin + direction * distance, Color.cyan);
        return Mathf.Clamp01(distance / maxSensorDistance);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Wall"))
            Die();
    }

    public void ReachGoal()
    {
        reachedGoal = true;
        fitness += 1000f;
        Die();
    }

    private void Die()
    {
        isDead = true;
        gameObject.SetActive(false);
    }
}
