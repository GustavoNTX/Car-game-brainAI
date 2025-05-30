using UnityEngine;

public class LaserEliminador : MonoBehaviour
{
    public float startDelay = 3f;           // Tempo antes de começar a andar
    public float speed = 1f;                // Velocidade do laser
    public float lifetime = 30f;            // Quanto tempo o laser sobrevive

    private float timer = 0f;
    private bool ativo = false;

    void Update()
    {
        timer += Time.deltaTime;

        if (!ativo && timer >= startDelay)
        {
            ativo = true;
        }

        if (ativo)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;

            // Destroi todos os carros atrás da linha do laser
            foreach (var carro in GameObject.FindGameObjectsWithTag("Car"))
            {
                if (carro.activeSelf && carro.transform.position.x < transform.position.x)
                {
                    carro.SetActive(false);
                }
            }
        }

        if (timer >= lifetime + startDelay)
        {
            Destroy(gameObject);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.down * 100, transform.position + Vector3.up * 100);
    }
}
