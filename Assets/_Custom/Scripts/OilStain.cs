using UnityEngine;

public class OilStain : MonoBehaviour
{
    [Tooltip("Multiplicador de ralentizacion. 0.5f = mitad de velocidad.")]
    public float slowMultiplier = 0.5f;

    [Tooltip("Duracion de la ralentizacion en segundos.")]
    public float duration = 3f;

    [Tooltip("Tag del jugador.")]
    public string playerTag = "Player";

    // Aplica ralentizacion al jugador si no es invulnerable.
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        PlayerStatus playerStatus = other.GetComponent<PlayerStatus>();
        if (playerStatus == null) return;

        playerStatus.HitByOil(slowMultiplier, duration);
    }
}
