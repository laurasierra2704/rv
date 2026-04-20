using UnityEngine;
using System.Net.Sockets;
using System.Threading;
using System;

public class GameController : MonoBehaviour
{
    [Header("Personajes")]
    public CharacterController personaje1;
    public CharacterController personaje2;

    [Header("Configuracion")]
    public float speed = 5f;
    public string serverIP = "127.0.0.1";
    public int serverPort = 5006;

    [Header("Motor ESP32")]
    public string espIP = "10.131.207.114";
    public int espPort = 5010;

    private float _joyX, _joyY;
    private float _mpuX, _mpuY;
    private bool _btnPresionado = false;
    private bool _modoJoystick = true;

    private readonly object _lock = new object();
    private TcpClient _client;
    private Thread _thread;
    private bool _running;
    private UdpClient _udpMotor;

    void Start()
    {
        _udpMotor = new UdpClient();
        _running = true;
        _thread = new Thread(ReceiveLoop) { IsBackground = true };
        _thread.Start();
    }

    void ReceiveLoop()
    {
        while (_running)
        {
            try
            {
                _client = new TcpClient(serverIP, serverPort);
                Debug.Log("Conectado a Python!");
                var stream = _client.GetStream();
                var buf = new byte[64];
                var sb = new System.Text.StringBuilder();

                while (_running)
                {
                    int n = stream.Read(buf, 0, buf.Length);
                    if (n == 0) break;

                    sb.Append(System.Text.Encoding.UTF8.GetString(buf, 0, n));
                    string raw = sb.ToString();
                    int nl;

                    while ((nl = raw.IndexOf('\n')) >= 0)
                    {
                        string line = raw.Substring(0, nl).Trim();
                        raw = raw.Substring(nl + 1);
                        Parse(line);
                    }
                    sb.Clear();
                    sb.Append(raw);
                }
            }
            catch (Exception e)
            {
                Debug.Log($"Error: {e.Message}");
                Thread.Sleep(1000);
            }
        }
    }

    void Parse(string line)
    {
        var ci = System.Globalization.CultureInfo.InvariantCulture;

        if (line == "BTN:1")
        {
            lock (_lock) { _btnPresionado = true; }
            return;
        }

        if (line.StartsWith("JOY:"))
        {
            var p = line.Substring(4).Split(',');
            if (p.Length < 2) return;
            if (float.TryParse(p[0], System.Globalization.NumberStyles.Float, ci, out float x) &&
                float.TryParse(p[1], System.Globalization.NumberStyles.Float, ci, out float y))
            {
                lock (_lock) { _joyX = x; _joyY = y; }
            }
            return;
        }

        if (line.StartsWith("MPU:"))
        {
            var p = line.Substring(4).Split(',');
            if (p.Length < 2) return;
            if (float.TryParse(p[0], System.Globalization.NumberStyles.Float, ci, out float x) &&
                float.TryParse(p[1], System.Globalization.NumberStyles.Float, ci, out float y))
            {
                lock (_lock) { _mpuX = x; _mpuY = y; }
            }
            return;
        }
    }

    void Update()
    {
        float joyX, joyY, mpuX, mpuY;
        bool btn;

        lock (_lock)
        {
            joyX = _joyX; joyY = _joyY;
            mpuX = _mpuX; mpuY = _mpuY;
            btn = _btnPresionado;
            _btnPresionado = false;
        }

        if (btn)
        {
            _modoJoystick = !_modoJoystick;
            Debug.Log(_modoJoystick ? "Modo: Joystick" : "Modo: MPU");
        }

        if (_modoJoystick)
        {
            personaje1?.Move(new Vector3(joyX, 0, joyY) * speed * Time.deltaTime);
        }
        else
        {
            personaje2?.Move(new Vector3(mpuX, 0, mpuY) * speed * Time.deltaTime);
        }
    }

 public void VibrarMotor()
{
    try
    {
        byte[] data = System.Text.Encoding.UTF8.GetBytes("VIBRAR\n");
        _udpMotor.Send(data, data.Length, espIP, espPort);
        Debug.Log("Vibracion enviada a ESP32");
    }
    catch (Exception e)
    {
        Debug.Log($"Error enviando vibracion: {e.Message}");
    }
}

    void OnDestroy()
    {
        _running = false;
        _client?.Close();
        _udpMotor?.Close();
        _thread?.Join(500);
    }
}