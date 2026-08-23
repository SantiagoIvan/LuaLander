# 🚀🚀🚀 Lua Lander
Juego 2D que empece con los assets de un tutorial y termine resolviendo por mi cuenta, agregando un muchas cosas nuevas.
Consiste en estacionar la nave en una de las plataformas dentro del limite de tiempo.

## Features
- Cuanto mas perpendicular al suelo mas despacio estaciones, mayor sera la puntuacion.
- Tenes que estacionar en las plataformas, de las cuales hay 2 tipos, cada una con su multiplicador de puntuacion.
- Te moves con WASD o con las flechitas.
- Moverse consume combustible.
- Le agregue la mecanica de Turbo, se activa con la tecla Space y consume su propio recurso. Solo se activa cuando la nave se mueve en forma recta. Larga una llama azul cuando se activa
- Animacion nueva de humo cuando te quedas sin nafta + sonido de motor desgastado.
- Cuando te moris la camara vibra
- Hay distintos tipos de alarmas, para el timer, turbo y combustible.
- Proximamente:
  - Distintos tipos de motor cada uno con su velocidad
  - Armas
  - Asteroides
  - Mercado para intercambiar las monedas
  - Zonas de distinta gravedad
  - Obstaculos
  - Zonas para agarrar objetos clave (te quedas un par de segundos en un circulito para agarrarlo)
  - Caniones enemigos
  - Y muchos mas
   
## Pickups
- Poder agarrar moneditas que te dan mas puntuacion.
- Hay distintos tipos de Fuel, cada uno con su propia cantidad de combustible. Restaura la barra de Fuel del jugador al consumirla.
- Lo mismo sucede con los turbo.
- Tambien hay pickups de area: te tenes que quedar parado un par de segundos arriba de un circulo para agarrarlos (por ejemplo, objetos clave).

### Arquitectura: Pickupable + PickUpArea + PickUpReward

El sistema separa dos cosas que antes estaban mezcladas en una sola jerarquia de herencia: **como se recolecta** el objeto, y **que te da** al recolectarlo.

- **`Pickupable`** (MonoBehaviour): la base de cualquier objeto recolectable. Expone el evento `OnPickedUp` y el metodo `getPickedUp()`, que aplica la recompensa, dispara el evento y se autodestruye (`destroySelf()`). No hereda la recompensa, la **compone**: tiene un campo `[SerializeField] protected PickUpReward reward`.
- **`PickUpArea`** (hereda de `Pickupable`): agrega la mecanica de "quedate parado encima X segundos para recolectar". Escucha `onTriggerStay2D()` / `onTriggerExit2D()` (invocados desde `Lander`, que es quien detecta la colision real), acumula un timer interno y, al superar `timeToPickup`, llama al `getPickedUp()` heredado — con lo cual aplica exactamente la misma recompensa que usaria un `Pickupable` instantaneo.
- **`PickUpReward`** (MonoBehaviour abstracta): define **que** se otorga. Tiene un campo `amount` configurable en el Inspector y un metodo abstracto `apply()` que cada subtipo implementa. Ya existen `Fuel`, `Turbo`, `Coin` y `Key`.
- **`PickupableVisual`** / **`PickUpAreaVisual`**: reaccionan a `OnPickedUp` (y, en el caso de area, tambien a `OnAreaStay`/`OnAreaExit` para el circulo de progreso) para disparar particulas/animaciones. Se subclasifican solo cuando el comportamiento visual realmente difiere entre pickups (por ejemplo, un color de particula distinto por tipo) — no hace falta crear una subclase vacia si no aporta nada.

Esto evita la explosion de subclases que tendriamos si cada combinacion de "tipo de recolectable" x "tipo de recompensa" necesitara su propia clase (ej. `FuelPickupArea`, `FuelPickupInstant`, etc. repitiendo la logica de dar fuel). Cualquier `PickUpReward` sirve tanto para un `Pickupable` suelto como para un `PickUpArea`.

### Como agregar un nuevo pickup

1. Crear el Prefab (o un Prefab Variant de uno existente si solo cambia el sprite, como se hizo para la llave a partir de `PickUpArea`).
2. Agregarle el componente `Pickupable` (si se recolecta al tocarlo) o `PickUpArea` (si hay que quedarse encima un tiempo).
3. Agregar al mismo GameObject un componente de recompensa que herede de `PickUpReward`. Si ya existe uno que sirva (`Fuel`, `Turbo`, `Coin`, `Key`), reutilizarlo. Si no, crear una clase nueva heredando de `PickUpReward` e implementar `apply()`.
4. Arrastrar ese componente de recompensa al campo `reward` del `Pickupable`/`PickUpArea` en el Inspector.
5. Configurar el `amount` deseado en el Inspector del componente de recompensa.
6. (Opcional) Agregarle una visual heredando de `PickupableVisual` o `PickUpAreaVisual` solo si necesita particulas/animacion especifica al recolectarse — si no, no hace falta subclasificar nada.

## CameraShake
Cuando te moris la camara hace una vibracion re zarpada

## Alarmas
Hay alarmas que se disparan cuando tenes poco combustible, tiempo o turbo. Para la de combustible, se pone el borde de la pantalla en rojo y solo se ve el centro. Para ello use una Imagen que recubre todo el Canvas y se activa cuando detecta el evento
OnLowFuel, y cuando agarras nuevo combustible, escucha ese evento para saber si la nueva cantidad esta encima del threshold. Para la imagen tuve que crear una con los bordes rojos y el centro transparente, y le agregue 
la animacion cambiando el alpha para que titile.
