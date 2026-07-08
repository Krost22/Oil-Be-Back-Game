# Guia de uso de scripts - Oil Be Back

Este documento explica paso a paso como configurar y usar los scripts del proyecto.

---

## 1. Requisitos previos

- El jugador debe tener el tag `Player`.
- El jugador debe tener un `Collider` (no trigger) para que los triggers lo detecten.
- Para mostrar el tiempo se usa `TextMeshPro`. Si no lo tienes importado, cambia `TextMeshProUGUI` por `UnityEngine.UI.Text` en `GameManager.cs`.

---

## 2. Scripts creados

| Script | Funcion |
|--------|---------|
| `GameManager.cs` | Controla velocidad del mundo, puntuacion, temporizador, ralentizacion por aceite y Game Over. |
| `UIManager.cs` | Muestra la pantalla de Game Over. |
| `OilDebuff.cs` | Aplica ralentizacion al jugador al pisar un charco de aceite. |
| `Collectable.cs` | Otorga puntos y suma 15 segundos al temporizador. |

---

## 3. Configuracion basica de la escena

### 3.1 GameManager

1. Crea un GameObject vacio llamado `_GameManager`.
2. Arrastra el script `GameManager.cs`.
3. Configura los campos en el inspector:
   - `Base Game Speed`: velocidad normal del mundo (ej. `5`).
   - `Max Time`: tiempo inicial en segundos (ej. `60`).
   - `Oil Slowdown Vfx`: arrastra el `ParticleSystem` hijo del jugador.
   - `Time Text`: arrastra el texto de TextMeshPro del Canvas.

### 3.2 UIManager

1. Crea un GameObject vacio llamado `_UIManager`.
2. Arrastra el script `UIManager.cs`.
3. Mas adelante, en `ShowGameOver()`, activa el panel de derrota en lugar del `Debug.Log`.

### 3.3 Jugador

1. Crea o selecciona el objeto del jugador.
2. Asigna el tag `Player`.
3. Añade un `Collider` y un `Rigidbody` si es necesario.
4. Crea un `ParticleSystem` como hijo del jugador.
5. Manten el VFX apagado por defecto (`GameObject` desactivado).
6. Arrastra ese VFX al campo `Oil Slowdown Vfx` del `_GameManager`.

---

## 4. Configurar charco de aceite

1. Crea un plano o cubo alargado en el camino del jugador.
2. Añade un `Collider`.
3. Marca `Is Trigger`.
4. Arrastra el script `OilDebuff.cs`.
5. Configura:
   - `Slow Multiplier`: cuanto se reduce la velocidad (ej. `0.5` para ir a la mitad).
   - `Duration`: segundos que dura el efecto (ej. `3`).
   - `Player Tag`: `Player`.

Al entrar en contacto, el jugador se ralentiza y el VFX hijo se activa. Al terminar la duracion, la velocidad vuelve a la normal y el VFX se apaga.

---

## 5. Configurar collectible

1. Crea un objeto en la escena (esfera, moneda, etc.).
2. Añade un `Collider`.
3. Marca `Is Trigger`.
4. Arrastra el script `Collectable.cs`.
5. Configura:
   - `Points`: puntos que otorga (ej. `10`).
   - `Player Tag`: `Player`.
   - `Collectible Vfx`: efecto de particulas al recoger (opcional).
   - `Collectible Sfx`: sonido al recoger (opcional).

Al tocarlo, suma puntos, anade 15 segundos al temporizador, reproduce efectos y se destruye.

---

## 6. Iniciar y terminar la partida

### Iniciar

Desde un boton de inicio o desde `Awake` de un script de prueba:

```csharp
GameManager.Instance.StartGame();
```

Esto reinicia:
- `isGameOver = false`
- `score = 0`
- `remainingTime = maxTime`
- `currentGameSpeed = baseGameSpeed`
- Inicia el contador.

### Terminar

Llama cuando el jugador pierda:

```csharp
GameManager.Instance.EndGame();
```

Esto:
- Pone `isGameOver = true`.
- Pone `currentGameSpeed = 0`.
- Detiene el temporizador.
- Apaga el VFX de aceite.
- Llama a `UIManager.Instance.ShowGameOver()`.

---

## 7. Como mover escenarios y obstaculos

Los objetos que se mueven hacia el jugador deben leer `GameManager.Instance.currentGameSpeed`.

Ejemplo basico en `Update`:

```csharp
void Update()
{
    if (GameManager.Instance == null || GameManager.Instance.isGameOver) return;
    transform.Translate(Vector3.left * GameManager.Instance.currentGameSpeed * Time.deltaTime);
}
```

Al llamar `EndGame()`, `currentGameSpeed` se vuelve `0` y todo se detiene.

---

## 8. Notas importantes

- No crees mas de un `GameManager` ni `UIManager` por escena; usan patron Singleton.
- El temporizador sigue corriendo durante la ralentizacion por aceite.
- Si un nuevo charco de aceite se pisa mientras otro debuff esta activo, se reinicia la duracion.
- El VFX de aceite debe estar como hijo del jugador y apagado por defecto.
