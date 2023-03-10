using UnityEngine;
using Unity.Collections;
using Unity.Networking.Transport;

public static class TransportHelper {
    public static void SendString(NetworkDriver driver, NetworkPipeline pipeline, NetworkConnection connection, string str) {

        driver.BeginSend(pipeline, connection, out var writer);

        // Creates a transportable string
        FixedString4096Bytes formmatedStr = new FixedString4096Bytes(str);
        writer.WriteFixedString4096(formmatedStr);

        driver.EndSend(writer);
    }

    public static string ReceiveString(DataStreamReader stream) {
        FixedString4096Bytes str = stream.ReadFixedString4096();
		return str.ToString();
    }
}
