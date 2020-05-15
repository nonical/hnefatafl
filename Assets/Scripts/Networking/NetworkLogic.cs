using Mirror;
using UnityEngine;
using Open.Nat;

public class NetworkLogic : MonoBehaviour {
    Mapping port = new Mapping(Protocol.Tcp, 7777, 7777);
    NatDevice device;

    public class MoveMessage : MessageBase {
        public int originI;
        public int originJ;
        public int destI;
        public int destJ;
    }

    private async void Awake() {
        NatDiscoverer discoverer = new NatDiscoverer();

        device = await discoverer.DiscoverDeviceAsync();
        await device.CreatePortMapAsync(port);
    }

    private void Start() {
        SetupNetworkHandlers();
    }

    private void SetupNetworkHandlers() {
        NetworkClient.RegisterHandler<MoveMessage>(OnMoveClient);
        NetworkServer.RegisterHandler<MoveMessage>(OnMoveServer);
    }

    private void OnMoveServer(NetworkConnection conn, MoveMessage msg) {
        NetworkServer.SendToAll(msg);
    }

    private void OnMoveClient(NetworkConnection conn, MoveMessage msg) {
        var figure = GameMemory.Figures[msg.originI, msg.originJ];
        var tile = GameMemory.Tiles[msg.destI, msg.destJ];

        MovementLogic.MovePiece(figure, tile);
    }

    public void SendMoveMessage((int, int) origin, (int, int) destination) {
        var msg = new MoveMessage() {
            originI = origin.Item1,
            originJ = origin.Item2,
            destI = destination.Item1,
            destJ = destination.Item2
        };

        NetworkClient.Send(msg);
    }

    private async void OnDestroy() {
        await device.DeletePortMapAsync(port);
    }
}
