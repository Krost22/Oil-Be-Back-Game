using UnityEngine;

public class OilDebuff : MonoBehaviour
{
    [Tooltip("Multiplicador de ralentizacion. 0.5f = mitad de velocidad.")]
    public float slowMultiplier = 0.5f;

    [Tooltip("Duracion de la ralentizacion en segundos.")]
    public float duration = 3f;

    [Tooltip("Tag del jugador.")]
    public string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        GameManager.Instance?.ApplyOilSlowdown(slowMultiplier, duration);
    }
}
