using System.Threading.Tasks;
using Mirror;
using Open.Nat;
using UnityEngine;

public class PortForwarding : MonoBehaviour {
    private NatDevice natDevice;
    private Mapping mapping;
    private int port;

    public int lifetime = 5;
    public string description = "Hnefatafl Host Port";

    private void Start() {
        port = GetComponent<TelepathyTransport>().port;
        mapping = new Mapping(Protocol.Tcp, port, port, lifetime, description);
    }

    public async Task OpenPort() {
        var discoverer = new NatDiscoverer();

        natDevice = await discoverer.DiscoverDeviceAsync();
        await natDevice.CreatePortMapAsync(mapping);

        Debug.Log($"Port {port} is ready to receive connections!");
    }
}
