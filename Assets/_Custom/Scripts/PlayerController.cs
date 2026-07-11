using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidadCaminar = 4f;
    public float velocidadCorrer = 7f;
    public float velocidadRotacion = 10f;

    [Header("Configuración de Salto")]
    public float fuerzaSalto = 4f;
    public LayerMask capaSuelo;

    [Header("Límites del Mapa")]
    public float limiteMinimoZ = -34f;

    private Rigidbody rb;
    private Animator anim;
    public bool esSuelo;
    private bool vivo = true;
    GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.FindAnyObjectByType<GameManager>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!vivo) return;

        var teclado = Keyboard.current;
        if (teclado == null) return; // no hay teclado conectado

        esSuelo = Physics.Raycast(transform.position, Vector3.down, 1.0f, capaSuelo);
        anim.SetBool("isGrounded", esSuelo);

        float horizontal = 0f;
        float vertical = 0f;

        if (teclado.aKey.isPressed || teclado.leftArrowKey.isPressed) horizontal -= 1f;
        if (teclado.dKey.isPressed || teclado.rightArrowKey.isPressed) horizontal += 1f;
        if (teclado.sKey.isPressed || teclado.downArrowKey.isPressed) vertical -= 1f;
        if (teclado.wKey.isPressed || teclado.upArrowKey.isPressed) vertical += 1f;

        Vector3 direccion = new Vector3(horizontal, 0f, vertical).normalized;

        bool estaCorriendo = teclado.leftShiftKey.isPressed && direccion.magnitude > 0;
        float velocidadActual = estaCorriendo ? velocidadCorrer : velocidadCaminar;

        if (direccion.magnitude > 0)
        {
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, velocidadRotacion * Time.deltaTime);

            Vector3 movimiento = direccion * velocidadActual * Time.deltaTime;
            Vector3 nuevaPosicion = transform.position + movimiento;

            // Restringe el movimiento para que Z no sea menor a limiteMinimoZ
            nuevaPosicion.z = Mathf.Max(nuevaPosicion.z, limiteMinimoZ);

            rb.MovePosition(nuevaPosicion);
        }

        float velocidadParaAnim = direccion.magnitude * velocidadActual;
        anim.SetFloat("Speed", velocidadParaAnim);

        if (esSuelo && teclado.spaceKey.wasPressedThisFrame)
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
            anim.SetTrigger("Jump");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EjecutarMuerte();
            gameManager.EndGame();

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EjecutarMuerte();
        }
    }

    private void EjecutarMuerte()
    {
        if (!vivo) return;

        vivo = false;
        anim.SetTrigger("Die");
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        this.enabled = false;
    }
}