using Mirror;
using NetworkMessages;
using Tags;
using UnityEngine;

public class NetworkHandlers : MonoBehaviour {
    private void Start() {
        SetupNetworkHandlers();
    }

    private void SetupNetworkHandlers() {
        NetworkClient.RegisterHandler<MoveMessage>(OnMoveClient);
        NetworkClient.RegisterHandler<TeamMessage>(OnTeamClient);

        NetworkServer.RegisterHandler<MoveMessage>(OnMoveServer);
        NetworkServer.RegisterHandler<TeamMessage>(OnTeamServer);
    }

    private void OnTeamServer(NetworkConnection conn, TeamMessage msg) {
        if (conn.identity.netId == 1) return;

        var team = GameMemory.teamTag != TeamTag.Attackers ? TeamTag.Attackers : TeamTag.Defenders;

        NetworkServer.SendToClientOfPlayer(conn.identity, new TeamMessage() { teamTag = team });
    }

    private void OnTeamClient(NetworkConnection conn, TeamMessage msg) {
        GameMemory.teamTag = msg.teamTag;
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
}
