using UnityEngine;

public class CameraCrashStyle : MonoBehaviour
{
    [Header("Objetivo")]
    public Transform target; 

    [Header("Posición (Estilo Crash)")]
    public Vector3 offset = new Vector3(0f, 2.5f, 6f); 
    public float suavizado = 5f; 

    void LateUpdate()
    {
        if (target == null) return;
        Vector3 posicionObjetivo = target.position + offset; 
        transform.position = Vector3.Lerp(transform.position, posicionObjetivo, suavizado * Time.deltaTime);
        transform.LookAt(target.position + Vector3.up * 1f);
    }
}