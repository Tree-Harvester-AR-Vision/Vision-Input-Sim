using UnityEngine;
using Unity.Networking.Transport;

public class UDPClient : MonoBehaviour {

    public NetworkDriver m_Driver;
    public NetworkConnection m_Connection;

    private NetworkPipeline m_RelPL;
    private NetworkPipeline m_FragPL;
    private bool SendStage;

    void Start() {
        m_Driver = NetworkDriver.Create();
        m_RelPL = m_Driver.CreatePipeline(typeof(ReliableSequencedPipelineStage));
        m_FragPL = m_Driver.CreatePipeline(typeof(FragmentationPipelineStage));
        m_Connection = default;

        var endpoint = NetworkEndPoint.LoopbackIpv4;
        endpoint.Port = 7000;
        m_Connection = m_Driver.Connect(endpoint);
    }

    void Update() {
        m_Driver.ScheduleUpdate().Complete();

        if (!m_Connection.IsCreated) {
            Debug.LogError("Something went wrong during connect");
            return;
        }

        if (!SendStage) {
            StartupSequence();
        } else { SendSequence(); }
    }

    public void OnDestroy() {
        m_Driver.Dispose();
    }

    private void StartupSequence() {
        NetworkEvent.Type cmd;
		while ((cmd = m_Connection.PopEvent(m_Driver, out DataStreamReader stream)) != NetworkEvent.Type.Empty) {
            if (cmd == NetworkEvent.Type.Connect) {
                Debug.Log("Now connected to server");

                // Sends type of client to server
                TransportHelper.SendString(m_Driver, m_RelPL, m_Connection, "transmitter");
                SendStage = true;

			} else if (cmd == NetworkEvent.Type.Disconnect) {
                Debug.Log("Client got disconnected from server");
                m_Connection = default;
			}
		}
    }

    private void SendSequence() {
        foreach(InputTree tree in TreeDetection.Trees.Values) {
            BoundingBox bBox = tree.boundingBox;
            if (bBox.Width != 0 && bBox.Height != 0) {
                TransportHelper.SendString(m_Driver, m_FragPL, m_Connection, tree.JsonSerialize());
            }
        }
    }
}
