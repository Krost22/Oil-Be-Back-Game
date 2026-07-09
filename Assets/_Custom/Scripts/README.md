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
| `GameManager.cs` | Controla velocidad del mundo, puntuacion, temporizador, ralentizacion por aceite y Game Over. Requiere un `AudioSource`. |
| `UIManager.cs` | Muestra la pantalla de Game Over. |
| `PlayerStatus.cs` | Gestiona el estado del jugador: ralentizacion por aceite e inmunidad del flotador. |
| `OilStain.cs` | Aplica ralentizacion al jugador al pisar un charco de aceite estatico. |
| `Collectable.cs` | Gestiona objetos recogibles: bonus de puntos/tiempo o flotador. |
| `TrackSpawner.cs` | Genera secciones de tobogan infinitas en el eje X positivo. |

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
4. Crea un `ParticleSystem` como hijo del jugador para el aceite.
5. Manten el VFX de aceite apagado por defecto (`GameObject` desactivado).
6. Arrastra ese VFX al campo `Oil Slowdown Vfx` del `_GameManager`.
7. Crea un `GameObject` hijo del jugador para el efecto visual del flotador (por ejemplo, `Orbs_gold`).
8. Manten el efecto del flotador apagado por defecto (`GameObject` desactivado).
9. Arrastra el script `PlayerStatus.cs` al jugador.
10. Configura `PlayerStatus`:
    - `Slow Multiplier`: cuanto se reduce la velocidad al pisar aceite (ej. `0.5`).
    - `Slow Duration`: segundos que dura la ralentizacion (ej. `3`).
    - `Floater Duration`: segundos que dura la inmunidad del flotador (ej. `5`).
    - `Floater Vfx`: arrastra el `GameObject` hijo del jugador que contiene el efecto del flotador.
    - `Floater Sfx`: arrastra el `AudioClip` que sonara al recoger el flotador.
    - `Is Invulnerable`: se marca automaticamente como `true` mientras dura el flotador. Indica que el jugador no puede ser afectado por charcos de aceite.

### 3.4 GameManager (AudioSource)

1. Selecciona el `_GameManager`.
2. Añade un componente `AudioSource` (el script `GameManager` lo requiere).
3. Configura el volumen y demas ajustes del `AudioSource`.

### 3.5 TrackSpawner

1. Crea un GameObject vacio llamado `_TrackSpawner`.
2. Colocalo en la posicion donde debe comenzar el tobogan.
3. Arrastra el script `TrackSpawner.cs`.
4. Configura:
   - `Track Prefabs`: lista de prefabs de secciones del tobogan.
   - `Segment Length`: longitud de cada seccion en el eje X (debe coincidir con el tamano real de los prefabs).
   - `Max Active Segments`: cuantas secciones mantener activas (ej. `5`).
   - `Initial Segments`: cuantas secciones generar al inicio (ej. `3`).
   - `Player Transform`: arrastra el jugador (opcional, si esta vacio se busca por tag `Player`).

---

## 4. Configurar charco de aceite (OilStain)

1. Crea un plano o cubo alargado en el camino del jugador.
2. Añade un `Collider`.
3. Marca `Is Trigger`.
4. Arrastra el script `OilStain.cs`.
5. Configura:
   - `Slow Multiplier`: cuanto se reduce la velocidad (ej. `0.5` para ir a la mitad).
   - `Duration`: segundos que dura el efecto (ej. `3`).
   - `Player Tag`: `Player`.

Al entrar en contacto, el script busca `PlayerStatus` en el jugador y le pide que aplique la ralentizacion. Si `PlayerStatus.isInvulnerable` es `true`, el charco no tiene efecto. El charco **no se destruye**.

---

## 5. Configurar collectible (bonus o flotador)

1. Crea un objeto en la escena (esfera, moneda, flotador, etc.).
2. Añade un `Collider`.
3. Marca `Is Trigger`.
4. Arrastra el script `Collectable.cs`.
5. Selecciona el `Collectable Type`:
   - `Score Time Bonus`: otorga puntos y suma 15 segundos.
   - `Floater`: activa la inmunidad temporal del flotador.
6. Configura segun el tipo:
   - `Score Time Bonus`: `Points`, `Collectible Vfx`, `Collectible Sfx`.
   - `Floater`: no requiere configuracion adicional en este script.

Al tocarlo, aplica el efecto correspondiente y el objeto se destruye.

---

## 6. Configurar generacion infinita del tobogan (TrackSpawner)

### Crear prefabs de secciones

1. Crea un prefab para cada seccion del tobogan.
2. Cada seccion debe tener la misma longitud en el eje X (la que indiques en `Segment Length`).
3. Dentro de cada seccion puedes incluir:
   - Trozos de tobogan.
   - Obstaculos con `OilStain.cs`.
   - Monedas u objetos con `Collectable.cs` (tipo `Score Time Bonus`).
   - Flotadores con `Collectable.cs` (tipo `Floater`).
4. Asegurate de que los elementos internos esten bien posicionados dentro de la seccion.

### Configurar el spawner

1. Selecciona `_TrackSpawner`.
2. Arrastra los prefabs de secciones al campo `Track Prefabs`.
3. Ajusta `Segment Length` al tamano real de cada prefab en X.
4. Ajusta `Max Active Segments` segun cuantas secciones quieras mantener cargadas.
5. Ajusta `Initial Segments` para las primeras secciones visibles al inicio.

### Comportamiento

- El spawner genera secciones aleatorias una tras otra en el eje X positivo.
- Evita repetir el mismo prefab dos veces seguidas.
- Mantiene activas `Max Active Segments` secciones por delante del jugador.
- Destruye las secciones mas antiguas cuando se supera el limite.
- Se detiene si el juego termina (`GameManager.isGameOver`).

---

## 7. Iniciar y terminar la partida

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

## 8. Como mover escenarios y obstaculos

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

## 9. Notas importantes

- No crees mas de un `GameManager` ni `UIManager` por escena; usan patron Singleton.
- El temporizador sigue corriendo durante la ralentizacion por aceite.
- Si un nuevo charco de aceite se pisa mientras otro debuff esta activo, se reinicia la duracion.
- El VFX de aceite debe estar como hijo del jugador y apagado por defecto.
- `PlayerStatus.cs` debe estar en el mismo GameObject que el jugador (tag `Player`).
- El efecto del flotador debe ser un `GameObject` hijo del jugador y estar apagado por defecto; `PlayerStatus` lo activa y desactiva.
- `GameManager` requiere un componente `AudioSource`; el sonido del flotador se reproduce desde ahi.
- Los charcos de aceite con `OilStain.cs` **no se destruyen** al pisarse.
- Los objetos recogibles con `Collectable.cs` **se destruyen** al tocarlos.
- `PlayerStatus.isInvulnerable` indica si el jugador es inmune a los charcos de aceite.
- Mientras `isInvulnerable` sea `true`, los charcos de aceite no afectan al jugador.
- El flotador activa `isInvulnerable` durante `Floater Duration` segundos.
- Todas las secciones del tobogan deben tener la misma longitud en X y coincidir con `TrackSpawner.Segment Length`.
- `TrackSpawner` genera secciones en el eje X positivo y destruye las mas antiguas automaticamente.
