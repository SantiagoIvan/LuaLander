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

    
    public event EventHandler<OnCoinCollectedEventArgs> OnCoinCollected;
    public event EventHandler<OnFuelCollectedEventArgs> OnFuelCollected;
    public event EventHandler OnLowFuel;
    public event EventHandler OnOutOfFuel;
    public event EventHandler<OnLandingEventArgs> OnLanding;
    
    

    private Rigidbody2D landerRigidbody2D;
    private State state;
    private float fuelThreshold = 20f; // Umbral para emitir evento de low fuel

    // Lander stats
    [SerializeField] private int rotationRate = 50;
    [SerializeField] private int accelerationRate;
    [SerializeField] private float maxFuelAmount = 120f;
    [SerializeField] private float maxTurboAmount = 40f;
    [SerializeField] private float turbo = 40f;

    [SerializeField] private float softLandingVelocityMagitude = 3.5f; // Maxima velocidad permitida al aterrizar
    [SerializeField] private float minDotVector = 0.9f; // Minimo producto cartesiano entre el vector canonico y global y el transform.y del lander para considerar que esta vertical
    [SerializeField] private float fuelAmount = 100f; // Cantidad de combustible inicial
    [SerializeField] private float fuelConsumptionRate = 10f; // Cantidad de combustible consumido por segundo al aplicar fuerza
    [SerializeField] private float NORMAL_GRAVITY_SCALE = 1f;
    [SerializeField] private float PAD_MOVEMENT_THRESHOLD = 0.4f; // Umbral para considerar que el pad de movimiento esta siendo presionado


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
        this.maxFuelAmount = GameManager.getStartingFuelLimit();
        this.NORMAL_GRAVITY_SCALE = GameManager.getStartingGravity();
        this.fuelAmount = Mathf.Min(this.maxFuelAmount, this.fuelAmount);
        this.accelerationRate = GameManager.getAccRate();
        Debug.Log("Lander has spawned with following stats: Fuel=" + this.fuelAmount + ", Gravity=" + this.NORMAL_GRAVITY_SCALE + ", AccRate=" + this.accelerationRate);
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
                if (fuelAmount < 0)
                {
                    // Podria emitir evento aca para largar humito o algo asi
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

                if (GameInput.Instance.isUpActionPressed() || GameInput.Instance.getMovementInputVector2().y > this.PAD_MOVEMENT_THRESHOLD)
                {
                    landerRigidbody2D.AddForce(accelerationRate * transform.up * Time.fixedDeltaTime);
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
        if(targetCollision.relativeVelocity.magnitude > softLandingVelocityMagitude)
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
        if (fuelAmount > 0)
        {
            fuelAmount -= fuelConsumptionRate * Time.fixedDeltaTime;
        }
        
        if (this.fuelAmount <= 0)
        {
            OnOutOfFuel?.Invoke(this, EventArgs.Empty);
            return;
        }
        if (this.fuelAmount < this.fuelThreshold)
        {
            OnLowFuel?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.TryGetComponent<Fuel>(out Fuel fuel))
        {
            this.fuelAmount = Math.Min(this.maxFuelAmount, this.fuelAmount + fuel.getFuelAmount());
            Debug.Log("Fuel collected! New fuel level: " + this.fuelAmount);
            fuel.getConsumed();
            OnFuelCollected?.Invoke(this, new OnFuelCollectedEventArgs{ landerFuelUpdated = this.fuelAmount });
        }
        if (other.gameObject.TryGetComponent<Coin>(out Coin coin))
        {
            Debug.Log("Coin collected!");
            // You can add coin collection logic here
            OnCoinCollected?.Invoke(this, new OnCoinCollectedEventArgs(coin.getValue())); // O puedo directamente hablarle al gamemanager para que sume puntos.
            coin.getCollected();
        }
    }

    public float getFuelAmount()
    {
        return this.fuelAmount;
    }
    public float getFuelThreshold() { return this.fuelThreshold; }
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
        return this.maxFuelAmount;
    }
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
        this.OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { newState= newState });
    }
    private void GameManager_OnTimeOut(object sender, EventArgs e)
    {
        this.failLanding(LandingResult.TimeOut);
    }
    public bool isFuelLow()
    {
        return this.fuelAmount < this.fuelThreshold;
    }
}
