using UnityEngine;
using UnityEngine.InputSystem;
using System;
public class Lander : MonoBehaviour
{
    // Emite un evento por cada tecla, el Lander visual escucha y prende alguna de las particulas del thruster correspondiente.
    public event EventHandler OnUpForce;
    public event EventHandler OnLeftForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnBeforeForceApplied;

    private Rigidbody2D landerRigidbody2D;
    [SerializeField] private int rotationRate = 50;
    [SerializeField] private int accelerationRate = 1000;
    [SerializeField] private float softLandingVelocityMagitude = 3.5f; // Maxima velocidad permitida al aterrizar
    [SerializeField] private float minDotVector = 0.9f; // Minimo producto cartesiano entre el vector canonico y global y el transform.y del lander para considerar que esta vertical
    [SerializeField] private float fuelAmount = 100f; // Cantidad de combustible inicial
    [SerializeField] private float fuelConsumptionRate = 10f; // Cantidad de combustible consumido por segundo al aplicar fuerza
    [SerializeField] private float maxFuelAmount = 120f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Para referencias externas
    private void Start()
    {
        Debug.Log("Lander script has started. Initial fuel is " + fuelAmount);
    }

    // Para referencias locales
    private void Awake()
    {
        Debug.Log("Lander script has awakened.");
        landerRigidbody2D = GetComponent<Rigidbody2D>();
    }

    // FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    // No corre en cada update, sino en intervalos fijos de tiempo. Se recomienda usarlo para código de física.
    // Estamos analizando si la tecla esta siendo presionada (isPressed), por lo tanto podemos realizarlo en FixedUpdate.
    // Diferente es si estamos analizando si la tecla fue presionada (wasPressed), en ese caso debemos hacerlo en Update.

    // Otra ventaja de usar FixedUpdate es que no depedemos del framerate, por lo tanto si el framerate es bajo, no se vera afectado el movimiento del lander.
    private void FixedUpdate()
    {
        OnBeforeForceApplied?.Invoke(this, EventArgs.Empty);

        // Al presionar la tecla de Up, se aplica una fuerza enn la direccion que esta apuntando el lander. (Vector2.up relativo al sprite, no global).
        // Para ello vamos a usar el componete Transform del lander, que nos permite obtener la direccion hacia donde esta apuntando el lander.

        /*
        •	Time.deltaTime: tiempo (en segundos) transcurrido desde el último frame — varía según la tasa de frames. Se usa en Update() para que el movimiento/animaciones sean independientes del framerate. Ej.: transform.Translate(velocidad * Time.deltaTime * Vector3.up);
        •	Time.fixedDeltaTime: intervalo fijo del motor de física (por defecto 0.02 s). Se usa en FixedUpdate() y en cálculos de física para mantener la simulación determinista y estable. Ej.: rb.MovePosition(rb.position + velocidad * Time.fixedDeltaTime);
        */
        if (fuelAmount < 0)
        {
            // Podria emitir evento aca para largar humito o algo asi
            Debug.Log("Out of fuel!");
            return;
        }
        
        Debug.Log("Fuel remaining: " + fuelAmount);

        if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.leftArrowKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            this.consumeFuel();
        }

        if (Keyboard.current.upArrowKey.isPressed)
        {
            landerRigidbody2D.AddForce(accelerationRate * transform.up * Time.fixedDeltaTime);
            OnUpForce?.Invoke(this, EventArgs.Empty);
            
        }
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            landerRigidbody2D.AddTorque(rotationRate * Time.fixedDeltaTime);
            OnLeftForce?.Invoke(this, EventArgs.Empty);
        }
        if (Keyboard.current.rightArrowKey.isPressed)
        {
            landerRigidbody2D.AddTorque(-rotationRate * Time.fixedDeltaTime);
            OnRightForce?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnCollisionEnter2D(Collision2D targetCollision)
    {
        // Para saber si aterrizamos sobre un LandingPad, necesitamos analizar el TIPO del objeto, la velocidad y el angulo
        // Podemos decir que estalla si lo que choco NO FUE UN LANDING PAD o si la velocidad fue mayor a la permitida.
        if(!targetCollision.gameObject.TryGetComponent(out LandingPad landingPad) || 
            targetCollision.relativeVelocity.magnitude > softLandingVelocityMagitude)
        {
            Debug.Log("Lander has crashed!");
            return;
        }

        // Analizo si el lander esta vertical con producto cartesiano entre el vector canonico global y el transform.y del lander
        // Si da 1, el lander esta vertical, si da 0, esta perpendicular. Trabaja con vectores unitarios, por lo tanto el producto cartesiano es el coseno del angulo entre ambos vectores.
        float dotProduct = Vector2.Dot(Vector2.up, transform.up);
        if(!(dotProduct > minDotVector))
        {
            Debug.Log("Lander is not vertical. Landing failed.");
            return;
        }
        
        Debug.Log("Successful landing! Calculating score...");

        // Para el score voy a usar tanto la incliacion como la velocidad y las voy a promediar. Cuanto mas cerca esten del limite, menos peso tendra
        float score = ScoreCalculator.getScore(targetCollision.relativeVelocity.magnitude, dotProduct, softLandingVelocityMagitude, minDotVector, landingPad.getLanderMultiplier());
        Debug.Log("Score: " + score);
    }

    private void consumeFuel()
    {
        if (fuelAmount > 0)
        {
            fuelAmount -= fuelConsumptionRate * Time.fixedDeltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.TryGetComponent<Fuel>(out Fuel fuel))
        {
            this.fuelAmount = Math.Min(this.maxFuelAmount, this.fuelAmount + fuel.getFuelAmount());
            Debug.Log("Fuel collected! New fuel level: " + this.fuelAmount);
            fuel.getConsumed();
        }
    }
}
