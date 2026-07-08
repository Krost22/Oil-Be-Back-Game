using UnityEngine;

public class Collectable : MonoBehaviour
{
    [Tooltip("Puntos que otorga al recogerse.")]
    public int points = 10;

    [Tooltip("Tag del jugador.")]
    public string playerTag = "Player";

    [Tooltip("Efecto de particulas al recogerse.")]
    public ParticleSystem collectibleVfx;

    [Tooltip("Sonido al recogerse.")]
    public AudioClip collectibleSfx;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        GameManager.Instance?.AddScore(points);
        GameManager.Instance?.AddTime(15f);

        if (collectibleVfx != null)
        {
            Instantiate(collectibleVfx, transform.position, Quaternion.identity);
        }

        if (collectibleSfx != null)
        {
            AudioSource.PlayClipAtPoint(collectibleSfx, transform.position);
        }

        Destroy(gameObject);
    }
}
