using UnityEngine;
using Mirror;
using NetworkMessages;

public class NetworkManagerHnefatafl : NetworkManager {
    public override void OnClientConnect(NetworkConnection conn) {
        base.OnClientConnect(conn);

        NetworkClient.Send(new TeamMessage());
    }
}
