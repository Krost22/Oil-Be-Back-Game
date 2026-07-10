using UnityEngine;

public class TestMover : MonoBehaviour
{
    [Tooltip("Direccion en la que se mueve el objeto.")]
    public Vector3 moveDirection = Vector3.left;

    [Tooltip("Limite para reiniciar la posicion del objeto y repetir el paso.")]
    public float resetPositionX = -10f;

    [Tooltip("Posicion inicial X tras reiniciar.")]
    public float startPositionX = 10f;

    void Update()
    {
        if (GameManager.Instance == null) return;
        if (GameManager.Instance.isGameOver) return;

        // Mueve el objeto usando la velocidad actual del GameManager.
        transform.Translate(moveDirection * GameManager.Instance.currentGameSpeed * Time.deltaTime);

        // Reinicia la posicion para simular escenario infinito.
        if (transform.position.x < resetPositionX)
        {
            transform.position = new Vector3(startPositionX, transform.position.y, transform.position.z);
        }
    }
}
