namespace SystemModule.Data;

public record struct ProcessMessage
{
    public int wIdent;
    public int wParam;
    public int nParam1;
    public int nParam2;
    public int nParam3;
    public int ActorId;
    public bool LateDelivery;
    public string Msg;
}