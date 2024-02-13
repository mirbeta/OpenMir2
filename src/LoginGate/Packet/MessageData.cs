namespace LoginGate.Packet;

public struct MessageData
{
    public string ConnectionId;
    public string ClientIP;
    public byte[] Body;
    public int MsgLen => Body.Length;
}