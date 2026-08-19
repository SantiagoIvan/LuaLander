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
- Lo mismo sucede con los turbo

[!IMPORTANT]
Debido a que hay distintos tipos de Pickups, cree una clase abstracta Pickupable (con un monto y un public event OnPickedUp)) de las cuales heredan Coin, Fuel y Turbo.
Pickupable emite un evento OnPickup, el cual escucha la clase abstracta PickupableVisual y reacciona con un metodo abstracto (OnPickedUp), el cual sobreescriben las clases que heredan.
De esa forma, creamos Coin -> Pickupable y CoinVisual -> PickupableVisual. Le configuro el monto que obtenes y sobreescribo el listener en CoinVisual. 
Actualmente solo loguea pero puede disparar alguna animacion en particular, ya la base esta hecha

[!IMPORTANT]
Todos los nuevos pickups deben heredar de Pickupable y su visual de PickupableVisual

## CameraShake
Cuando te moris la camara hace una vibracion re zarpada

## Alarmas
Hay alarmas que se disparan cuando tenes poco combustible, tiempo o turbo. Para la de combustible, se pone el borde de la pantalla en rojo y solo se ve el centro. Para ello use una Imagen que recubre todo el Canvas y se activa cuando detecta el evento
OnLowFuel, y cuando agarras nuevo combustible, escucha ese evento para saber si la nueva cantidad esta encima del threshold. Para la imagen tuve que crear una con los bordes rojos y el centro transparente, y le agregue 
la animacion cambiando el alpha para que titile.
