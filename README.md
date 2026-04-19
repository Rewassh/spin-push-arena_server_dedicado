# 🎮 Spin Push Arena - Servidor Dedicado

## 📌 Descripción general

**Spin Push Arena** es un videojuego multijugador para **2 jugadores** desarrollado en **Unity**, en el cual ambos participantes interactúan en tiempo real dentro de una arena cerrada utilizando un servidor dedicado proporcionado en clase.

Cada jugador controla un personaje con movimiento basado en **rotación continua**, generando una dinámica estratégica donde el objetivo es **empujar al oponente fuera del área de juego**.

🏆 El primer jugador en ganar **3 rondas** es declarado ganador.

---

## 🧠 Mecánica del juego

### 👥 Jugadores

* **Player 1** → Cilindro
* **Player 2** → Cápsula

### 🎮 Movimiento

* El personaje rota automáticamente sobre su eje.
* Mantener presionada la tecla `W` permite avanzar en la dirección actual.
* Al soltar la tecla, el personaje vuelve a girar.

### 💥 Interacción

* Si un jugador en movimiento colisiona con el otro, lo empuja.
* El jugador que salga del área pierde la ronda.

### 🏁 Condición de victoria

* El primero en alcanzar **3 puntos** gana la partida.

---

## 🌐 Comunicación cliente-servidor

El proyecto utiliza exclusivamente el **servidor entregado en clase**, sin modificaciones.

### 📡 Tipo de comunicación

* Protocolo: **HTTP**
* Método: **Polling periódico**
* Frecuencia: ~cada **0.1 segundos**

---

### 📤 Envío de datos (POST)

Cada cliente envía su posición al servidor:

```http
POST /server/{game_id}/{player_id}
```

**Body (JSON):**

```json
{
  "posX": float,
  "posY": float,
  "posZ": float
}
```

---

### 📥 Recepción de datos (GET)

Cada cliente consulta la posición del otro jugador:

```http
GET /server/{game_id}/{other_player_id}
```

**Respuesta:**

```json
{
  "posX": float,
  "posY": float,
  "posZ": float
}
```

---

### 🔄 Sistema de Polling

Se implementaron **corrutinas en Unity** para:

* Enviar periódicamente la posición del jugador local
* Consultar periódicamente la posición del jugador remoto
* Evitar bloqueos del juego durante solicitudes HTTP

Esto permite una sincronización continua sin afectar el rendimiento.

---

## 👥 Jugadores simultáneos

* El juego soporta **exactamente 2 jugadores** conectados simultáneamente.
* Cada jugador selecciona su rol desde el menú:

  * Player 1
  * Player 2
* Ambos deben ingresar el **mismo Game ID** para conectarse a la misma partida.

---

## 🔄 Sincronización

* Basada en intercambio de posiciones mediante polling.
* Se utiliza **interpolación (Lerp)** para suavizar el movimiento del jugador remoto.
* La **rotación no se sincroniza** debido a limitaciones del servidor.

---

## 🎯 Interacción entre jugadores

* Empuje mediante colisiones
* Competencia por sacar al oponente del área
* Sistema de rondas

---

## 🕹️ Flujo del juego

### ▶️ Inicio

* Menú principal
* Selección de jugador (Player 1 o Player 2)
* Ingreso de Game ID

### ⏳ Espera

* Mensaje: *Waiting for opponent...*
* El juego espera hasta que ambos jugadores estén conectados

### ⚔️ Partida

* Mensaje: *Fight!*
* Movimiento y colisiones activadas
* Sistema de puntaje en tiempo real

### 🏆 Final

* Un jugador alcanza 3 puntos
* Se muestra el ganador
* Panel final con opciones:

  * Play Again
  * Back to Menu

---

## 🖥️ UI / UX

* Indicador de rol del jugador
* Puntaje en tiempo real
* Estado del juego (Waiting / Fight / Game Over)
* Panel final con ganador
* Botones interactivos

---

## 🌍 Diseño del entorno

* Arena circular simple
* Límite visible del área de juego
* Jugadores diferenciados por forma
* Cámara fija en tercera persona

---

## ⚙️ Instrucciones para ejecutar

### 📋 Requisitos

* Unity
* Servidor dedicado en ejecución
* Puerto: **5005**

### ▶️ Pasos

1. Ejecutar el servidor:

   ```
   http://127.0.0.1:5005
   ```
2. Ejecutar **dos instancias del juego** (Editor + Build o dos builds)
3. Configurar:

   * Mismo Game ID
   * Un jugador como Player 1
   * Otro como Player 2
4. Iniciar la partida

---

## ⚠️ Limitaciones conocidas

* El servidor solo sincroniza posiciones, no rotación
* El puntaje se calcula localmente
* Puede haber pequeños desfases entre clientes
* El sistema de empuje no es físicamente perfecto

---

## 📦 Repositorio

Incluye:

* Proyecto completo de Unity
* Scripts de red
* Lógica del juego
* UI y escenas

---

## 🎥 Video de demostración

https://youtu.be/PtVxRD3F33U

---

## 👥 Integrantes

* Diva Catalina Rodríguez Acosta
* Juan Sebastián Muñoz Molina
