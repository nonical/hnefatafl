using System.Threading.Tasks;
using Mirror;
using NetworkMessages;

public class NetworkManagerHnefatafl : NetworkManager {
    private PortForwarding portForwarding;

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
}
