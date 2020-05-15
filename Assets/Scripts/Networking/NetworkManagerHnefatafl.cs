using UnityEngine;
using Mirror;
using NetworkMessages;

public class NetworkManagerHnefatafl : NetworkManager {
    private PortForwarding portForwarding;

    public override void Start() {
        base.Start();
        portForwarding = GetComponent<PortForwarding>();
    }

    public override async void OnStartClient() {
        await portForwarding.OpenPort();
        base.OnStartClient();
    }

    public override async void OnStartHost() {
        await portForwarding.OpenPort();
        base.OnStartHost();
    }

    public override void OnClientConnect(NetworkConnection conn) {
        base.OnClientConnect(conn);
        NetworkClient.Send(new TeamMessage());
    }
}
