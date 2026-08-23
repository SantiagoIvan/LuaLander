using UnityEngine;
using UnityEngine.InputSystem;
using System;
public class Lander : MonoBehaviour
{
    // Singleton
    public static Lander Instance { get; private set; }
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    // Emite un evento por cada tecla, el Lander visual escucha y prende alguna de las particulas del thruster correspondiente.
    public event EventHandler OnUpForce;
    public event EventHandler OnLeftForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnBeforeForceApplied;
    public event EventHandler OnTurboPressed;

    public event EventHandler<OnCoinCollectedEventArgs> OnCoinCollected;
    public event EventHandler<OnFuelCollectedEventArgs> OnFuelCollected;
    public event EventHandler OnTurboCollected; //TODO falta el prefab de turbo Y actualizar onTrigger2D
    public event EventHandler OnLowFuel;
    public event EventHandler OnOutOfFuel;
    public event EventHandler OnLowTurbo;
    public event EventHandler OnOutOfTurbo;
    public event EventHandler<OnLandingEventArgs> OnLanding;


    private Rigidbody2D landerRigidbody2D;
    private State state;

    // Lander stats
    private float FUEL_THRESHOLD = 20f; // Umbral para emitir evento de low fuel
    private float TURBO_THRESHOLD = 5f; // Umbral para emitir evento de low turbo
    [SerializeField] private float STARTING_FUEL;
    [SerializeField] private float STARTING_TURBO;

    // Consumo de recursos
    [SerializeField] private float TURBO_CONSUMPTION_RATE = 5f;
    [SerializeField] private float FUEL_CONSUMPTION_RATE = 10f; // Cantidad de combustible consumido por segundo al aplicar fuerza
    private float currentSpeedMultiplier = 1f; // Cuando se activa el turbo, se lo modifico.

    // Fisica
    [SerializeField] private int rotationRate = 50;
    [SerializeField] private int accelerationRate;
    [SerializeField] private float softLandingVelocityMagitude = 3.5f; // Maxima velocidad permitida al aterrizar
    [SerializeField] private float minDotVector = 0.9f; // Minimo producto cartesiano entre el vector canonico y global y el transform.y del lander para considerar que esta vertical
    [SerializeField] private float NORMAL_GRAVITY_SCALE = 1f;
    [SerializeField] private float PAD_MOVEMENT_THRESHOLD = 0.4f; // Umbral para considerar que el pad de movimiento esta siendo presionado
    [SerializeField] private float TURBO_SPEED_MULTIPLIER = 3f;
    private float timeOnTriggerArea = 0f; // Tiempo que estas sobre el triggerArea para agarrar objetos, como llaves o cajas que se agarran OnTriggerEnter2D.

    // Fuel y Turbo comparten exactamente la misma logica (amount/max/threshold + OnLow/OnOutOf).
    // Se maneja con ResourceMeter para no duplicar esa logica dos veces.
    private ResourceMeter fuelMeter;
    private ResourceMeter turboMeter;

    // Para referencias locales
    private void Awake()
    {
        landerRigidbody2D = GetComponent<Rigidbody2D>();
        Instance = this;
        landerRigidbody2D.gravityScale = 0f; // Para que empiece congelado el Lander hasta que uno toque una tecla de movimiento.
        this.state = State.WaitingToStart;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Para referencias externas
    private void Start()
    {
        float maxFuelAmount = GameManager.getStartingFuelLimit();
        float maxTurboAmount = GameManager.getStartingTurboLimit();
        this.NORMAL_GRAVITY_SCALE = GameManager.getStartingGravity();
        this.accelerationRate = GameManager.getAccRate();

        // Creamos los dos ResourceMeter con los valores iniciales/serializados de arriba
        this.fuelMeter = new ResourceMeter(STARTING_FUEL, maxFuelAmount, this.FUEL_THRESHOLD, FUEL_CONSUMPTION_RATE);
        this.turboMeter = new ResourceMeter(STARTING_TURBO, maxTurboAmount, this.TURBO_THRESHOLD, TURBO_CONSUMPTION_RATE);

        // Re-emitimos los eventos internos del ResourceMeter como los eventos publicos que ya
        // conocen el resto de los scripts (SoundManager, StatsUI, LowFuelUI, etc), asi no hay
        // que tocar ningun otro componente que ya escuche Lander.Instance.OnLowFuel, etc.
        this.fuelMeter.OnLow += (sender, e) => OnLowFuel?.Invoke(this, EventArgs.Empty);
        this.fuelMeter.OnOutOf += (sender, e) => OnOutOfFuel?.Invoke(this, EventArgs.Empty);
        this.turboMeter.OnLow += (sender, e) => OnLowTurbo?.Invoke(this, EventArgs.Empty);
        this.turboMeter.OnOutOf += (sender, e) => OnOutOfTurbo?.Invoke(this, EventArgs.Empty);

        GameManager.Instance.OnTimeOut += GameManager_OnTimeOut;
    }
    public State getState()
    {
        return this.state;
    }
    // FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    // No corre en cada update, sino en intervalos fijos de tiempo. Se recomienda usarlo para código de física.
    // Estamos analizando si la tecla esta siendo presionada (isPressed), por lo tanto podemos realizarlo en FixedUpdate.
    // Diferente es si estamos analizando si la tecla fue presionada (wasPressed), en ese caso debemos hacerlo en Update.
    // Otra ventaja de usar FixedUpdate es que no depedemos del framerate, por lo tanto si el framerate es bajo, no se vera afectado el movimiento del lander.
    private void FixedUpdate()
    {
        OnBeforeForceApplied?.Invoke(this, EventArgs.Empty);
        currentSpeedMultiplier = 1;
        // Al presionar la tecla de Up, se aplica una fuerza enn la direccion que esta apuntando el lander. (Vector2.up relativo al sprite, no global).
        // Para ello vamos a usar el componete Transform del lander, que nos permite obtener la direccion hacia donde esta apuntando el lander.
        /*
        •	Time.deltaTime: tiempo (en segundos) transcurrido desde el último frame — varía según la tasa de frames. Se usa en Update() para que el movimiento/animaciones sean independientes del framerate. Ej.: transform.Translate(velocidad * Time.deltaTime * Vector3.up);
        •	Time.fixedDeltaTime: intervalo fijo del motor de física (por defecto 0.02 s). Se usa en FixedUpdate() y en cálculos de física para mantener la simulación determinista y estable. Ej.: rb.MovePosition(rb.position + velocidad * Time.fixedDeltaTime);
        */
        switch (this.state)
        {
            case State.WaitingToStart:
                if (GameInput.Instance.isUpActionPressed() ||
                    GameInput.Instance.isLeftActionPressed() ||
                    GameInput.Instance.isRightActionPressed() ||
                    GameInput.Instance.getMovementInputVector2() != Vector2.zero
                    )
                {
                    // Aca no consumo fiel porque al cambiar el estado, entra en el condicional de abajo y terminaria consumiendo 2 veces en un mismo frame.
                    this.landerRigidbody2D.gravityScale = NORMAL_GRAVITY_SCALE;
                    this.setState(State.Normal);
                }
                break;
            case State.Normal:
                if (fuelMeter.GetAmount() < 0)
                {
                    return;
                }
                if (GameInput.Instance.isUpActionPressed() ||
                    GameInput.Instance.isLeftActionPressed() ||
                    GameInput.Instance.isRightActionPressed() ||
                    GameInput.Instance.getMovementInputVector2().y > this.PAD_MOVEMENT_THRESHOLD
                    )
                {
                    this.consumeFuel();
                }
                // Solo voy a consumir turbo si se presiono la tecla de turbo y la fecha para adelante. No voy a aplicar turbo en las rotaciones.
                if (GameInput.Instance.isLanderTurboActionPressed() && GameInput.Instance.isUpActionPressed() && this.getTurboAmount() > 0)
                {
                    this.consumeTurbo();
                    currentSpeedMultiplier = TURBO_SPEED_MULTIPLIER;
                    OnTurboPressed?.Invoke(this, EventArgs.Empty);

                }
                if (GameInput.Instance.isUpActionPressed() || GameInput.Instance.getMovementInputVector2().y > this.PAD_MOVEMENT_THRESHOLD)
                {
                    landerRigidbody2D.AddForce(accelerationRate * transform.up * Time.fixedDeltaTime * currentSpeedMultiplier);
                    OnUpForce?.Invoke(this, EventArgs.Empty);
                }
                if (GameInput.Instance.isLeftActionPressed() || GameInput.Instance.getMovementInputVector2().x < -this.PAD_MOVEMENT_THRESHOLD)
                {
                    landerRigidbody2D.AddTorque(rotationRate * Time.fixedDeltaTime);
                    OnLeftForce?.Invoke(this, EventArgs.Empty);
                }
                if (GameInput.Instance.isRightActionPressed() || GameInput.Instance.getMovementInputVector2().x > this.PAD_MOVEMENT_THRESHOLD)
                {
                    landerRigidbody2D.AddTorque(-rotationRate * Time.fixedDeltaTime);
                    OnRightForce?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
        }
    }
    private void OnCollisionEnter2D(Collision2D targetCollision)
    {
        // Analizo si el lander esta vertical con producto cartesiano entre el vector canonico global y el transform.y del lander
        // Si da 1, el lander esta vertical, si da 0, esta perpendicular. Trabaja con vectores unitarios, por lo tanto el producto cartesiano es el coseno del angulo entre ambos vectores.
        float dotProduct = Vector2.Dot(Vector2.up, transform.up);

        // Para saber si aterrizamos sobre un LandingPad, necesitamos analizar el TIPO del objeto, la velocidad y el angulo
        // Podemos decir que estalla si lo que choco NO FUE UN LANDING PAD o si la velocidad fue mayor a la permitida.
        if (!targetCollision.gameObject.TryGetComponent(out LandingPad landingPad))
        {
            this.failLanding(LandingResult.WrongLandingArea);
            return;
        }
        if (targetCollision.relativeVelocity.magnitude > softLandingVelocityMagitude)
        {
            this.failLanding(LandingResult.TooFastLanding);
            return;
        }
        if (!(dotProduct > minDotVector))
        {
            this.failLanding(LandingResult.TooSteepLanding);
            return;
        }

        Debug.Log("Successful landing! Calculating score...");
        // Para el score voy a usar tanto la incliacion como la velocidad y las voy a promediar. Cuanto mas cerca esten del limite, menos peso tendra
        float landingScore = ScoreCalculator.getScore(targetCollision.relativeVelocity.magnitude, dotProduct, softLandingVelocityMagitude, minDotVector, landingPad.getLanderMultiplier());
        Debug.Log("Score: " + landingScore);
        this.setState(State.GameOver);
        GameManager.Instance.landed(landingScore);
        OnLanding?.Invoke
            (this,
                new OnLandingEventArgs
                {
                    landingResult = LandingResult.Success,
                    landingSpeed = targetCollision.relativeVelocity.magnitude,
                    landingAngle = dotProduct,
                    multiplier = landingPad.getLanderMultiplier(),
                    finalScore = GameManager.Instance.getScore()
                }
            );
    }
    private void consumeFuel()
    {
        fuelMeter.Consume(Time.fixedDeltaTime);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<Fuel>(out Fuel fuel))
        {
            fuelMeter.Add(fuel.getAmount());
            Debug.Log("Fuel collected! New fuel level: " + fuelMeter.GetAmount());
            fuel.getPickedUp();
            OnFuelCollected?.Invoke(this, new OnFuelCollectedEventArgs { landerFuelUpdated = fuelMeter.GetAmount() });
        }
        if (other.gameObject.TryGetComponent<Turbo>(out Turbo turbo))
        {
            turboMeter.Add(turbo.getAmount());
            Debug.Log("Turbo collected! New fuel level: " + turboMeter.GetAmount());
            turbo.getPickedUp();
            OnTurboCollected?.Invoke(this, EventArgs.Empty);
        }
        if (other.gameObject.TryGetComponent<Coin>(out Coin coin))
        {
            // You can add coin collection logic here
            OnCoinCollected?.Invoke(this, new OnCoinCollectedEventArgs((int)coin.getAmount())); // O puedo directamente hablarle al gamemanager para que sume puntos.
            coin.getPickedUp();
            Debug.Log("Coin collected!");
        }
    }
    public float getFuelAmount()
    {
        return fuelMeter.GetAmount();
    }
    public float getFuelThreshold() { return fuelMeter.GetThreshold(); }
    public float getSpeedX()
    {
        return this.landerRigidbody2D.linearVelocity.x;
    }
    public float getSpeedY()
    {
        return this.landerRigidbody2D.linearVelocity.y;
    }
    public float getMaxFuelAmount()
    {
        return fuelMeter.GetMaxAmount();
    }
    public bool isFuelLow()
    {
        return fuelMeter.IsLow();
    }

    // --- Equivalentes de Turbo, misma API que Fuel para mantener simetria ---
    public float getTurboAmount()
    {
        return turboMeter.GetAmount();
    }
    public float getTurboThreshold()
    {
        return turboMeter.GetThreshold();
    }
    public float getMaxTurboAmount()
    {
        return turboMeter.GetMaxAmount();
    }
    public bool isTurboLow()
    {
        return turboMeter.IsLow();
    }
    public void consumeTurbo()
    {
        turboMeter.Consume(Time.fixedDeltaTime);
    }

    public ResourceMeter GetFuelMeter() => fuelMeter;
    public ResourceMeter GetTurboMeter() => turboMeter;

    private void failLanding(LandingResult result)
    {
        Debug.Log("Lander has crashed!");
        GameManager.Instance.failLanding();
        this.setState(State.GameOver);
        OnLanding?.Invoke
        (this,
            new OnLandingEventArgs
            {
                landingResult = result,
                landingSpeed = 0,
                landingAngle = 0,
                multiplier = 0,
                finalScore = 0
            }
        );
        return;
    }
    private void setState(State newState)
    {
        this.state = newState;
        this.OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { newState = newState });
    }
    private void GameManager_OnTimeOut(object sender, EventArgs e)
    {
        this.failLanding(LandingResult.TimeOut);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        this.timeOnTriggerArea += Time.deltaTime;
        if(other.gameObject.TryGetComponent<PickUpArea>(out PickUpArea pickUpArea))
        {
            pickUpArea.onTriggerStay2D();
            Debug.Log("Time on trigger area: " + this.timeOnTriggerArea);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        this.timeOnTriggerArea = 0f;
        Debug.Log("Left trigger area.");
        if(other.gameObject.TryGetComponent<PickUpArea>(out PickUpArea pickUpArea))
        {
            pickUpArea.onTriggerExit2D();
        }
    }
}