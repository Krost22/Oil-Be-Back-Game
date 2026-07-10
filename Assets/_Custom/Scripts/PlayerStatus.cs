using System.Collections;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [Header("Oil Slowdown")]
    [Tooltip("Multiplicador de ralentizacion al pisar aceite. 0.5f = mitad de velocidad.")]
    public float slowMultiplier = 0.5f;

    [Tooltip("Duracion de la ralentizacion por aceite en segundos.")]
    public float slowDuration = 3f;

    [Header("Floater Power-up")]
    [Tooltip("Duracion de la inmunidad del flotador en segundos.")]
    public float floaterDuration = 5f;

    [Tooltip("GameObject hijo del jugador que se activa mientras dura el flotador.")]
    public GameObject floaterVfx;

    [Tooltip("Sonido que reproduce al activar el flotador.")]
    public AudioClip floaterSfx;

    public bool isInvulnerable;             // Indica si el jugador es inmune a los charcos de aceite.
    public bool isSlowed;                   // Indica si el jugador esta bajo efecto de ralentizacion por aceite.
    private Coroutine floaterCoroutine;     // Referencia a la corrutina del flotador.

    // Asegura que los estados y el VFX del flotador esten apagados al iniciar.
    private void Awake()
    {
        isInvulnerable = false;
        isSlowed = false;
        SetFloaterVfxActive(false);
    }

    // Aplica la ralentizacion por aceite si el jugador no es invulnerable.
    public void HitByOil()
    {
        if (isInvulnerable) return;
        GameManager.Instance?.ApplyOilSlowdown(slowMultiplier, slowDuration);
    }

    // Sobrecarga que permite a otros scripts pasar valores personalizados de ralentizacion.
    public void HitByOil(float customSlowMultiplier, float customDuration)
    {
        if (isInvulnerable) return;
        GameManager.Instance?.ApplyOilSlowdown(customSlowMultiplier, customDuration);
    }

    // Activa la inmunidad del flotador, reiniciando la duracion si ya estaba activa.
    // Tambien cancela cualquier ralentizacion activa para que la gota retroceda.
    public void ActivateFloater()
    {
        if (floaterCoroutine != null)
        {
            StopCoroutine(floaterCoroutine);
        }

        GameManager.Instance?.RestoreGameSpeed();
        floaterCoroutine = StartCoroutine(FloaterRoutine());
    }

    // Reinicia el estado del jugador (usado al comenzar una nueva partida).
    public void ResetStatus()
    {
        if (floaterCoroutine != null)
        {
            StopCoroutine(floaterCoroutine);
        }
        floaterCoroutine = null;
        isInvulnerable = false;
        isSlowed = false;
        SetFloaterVfxActive(false);
    }

    // Corrutina que mantiene activa la inmunidad del flotador durante su duracion.
    private IEnumerator FloaterRoutine()
    {
        isInvulnerable = true;
        SetFloaterVfxActive(true);
        PlayFloaterSound();
        yield return new WaitForSeconds(floaterDuration);
        isInvulnerable = false;
        SetFloaterVfxActive(false);
        floaterCoroutine = null;
    }

    // Activa o desactiva el GameObject del flotador.
    private void SetFloaterVfxActive(bool active)
    {
        if (floaterVfx == null) return;
        floaterVfx.SetActive(active);
    }

    // Reproduce el sonido del flotador usando el AudioSource del GameManager.
    private void PlayFloaterSound()
    {
        if (floaterSfx == null || GameManager.Instance == null) return;

        AudioSource source = GameManager.Instance.GetComponent<AudioSource>();
        if (source != null)
        {
            source.PlayOneShot(floaterSfx);
        }
    }
}
