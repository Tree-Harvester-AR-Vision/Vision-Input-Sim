using UnityEngine;
using Unity.Collections;
using Unity.Networking.Transport;

public static class TransportHelper {
    public static void SendString(NetworkDriver driver, NetworkPipeline pipeline, NetworkConnection connection, string str)
    {

        DataStreamWriter writer = new DataStreamWriter();
        
        driver.BeginSend(pipeline, connection, out writer);

        // Creates a transportable string
        FixedString4096Bytes formatedString = new FixedString4096Bytes(str);
        writer.WriteFixedString4096(formatedString);

        driver.EndSend(writer);
    }

    public static string ReceiveString(DataStreamReader stream) {
        FixedString4096Bytes str = stream.ReadFixedString4096();
		return str.ToString();
    }
}
