using UnityEngine;

public enum CollectableType { ScoreTimeBonus, Floater }

public class Collectable : MonoBehaviour
{
    [Tooltip("Tipo de objeto: bonus de puntos/tiempo o flotador.")]
    public CollectableType collectableType;

    [Tooltip("Puntos que otorga al recogerse. Solo aplica si el tipo es ScoreTimeBonus.")]
    public int points = 10;

    [Tooltip("Tag del jugador.")]
    public string playerTag = "Player";

    [Tooltip("Efecto de particulas al recogerse. Solo aplica si el tipo es ScoreTimeBonus.")]
    public ParticleSystem collectibleVfx;

    [Tooltip("Sonido al recogerse. Solo aplica si el tipo es ScoreTimeBonus.")]
    public AudioClip collectibleSfx;

    // Detecta colisiones con el jugador y aplica el efecto correspondiente segun el tipo.
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        if (collectableType == CollectableType.Floater)
        {
            PlayerStatus playerStatus = other.GetComponent<PlayerStatus>();
            if (playerStatus != null)
            {
                playerStatus.ActivateFloater();
            }
        }
        else
        {
            GameManager.Instance?.AddScore(points);
            GameManager.Instance?.AddTime(10f);

            if (collectibleVfx != null)
            {
                Instantiate(collectibleVfx, transform.position, Quaternion.identity);
            }

            if (collectibleSfx != null)
            {
                AudioSource.PlayClipAtPoint(collectibleSfx, transform.position);
            }
        }

        Destroy(gameObject);
    }
}
