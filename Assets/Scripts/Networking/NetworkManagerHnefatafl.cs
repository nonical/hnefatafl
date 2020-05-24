using System;
using System.Threading.Tasks;
using Mirror;
using NetworkMessages;

public class NetworkManagerHnefatafl : NetworkManager {
    private PortForwarding portForwarding;

    public static Action ClientConnect;
    public static Action ClientDisconnect;

    public override void Start() {
        base.Start();
        portForwarding = GetComponent<PortForwarding>();
    }

    public override void OnStartHost() {
        Task.Run(portForwarding.OpenPort).Wait();
        base.OnStartHost();
    }

    public override void OnClientConnect(NetworkConnection conn) {
        base.OnClientConnect(conn);
        NetworkClient.Send(new TeamMessage());
    }

    public override void OnServerConnect(NetworkConnection conn) {
        base.OnServerConnect(conn);
        if (conn.connectionId != 0) ClientConnect?.Invoke();
    }

    public override void OnServerDisconnect(NetworkConnection conn) {
        base.OnServerDisconnect(conn);
        ClientDisconnect?.Invoke();
    }

    public override void OnClientDisconnect(NetworkConnection conn) {
        base.OnClientDisconnect(conn);
        ClientDisconnect?.Invoke();
    }
}
