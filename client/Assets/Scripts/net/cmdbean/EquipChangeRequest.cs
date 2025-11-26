public class EquipChangeRequest:Request
{
    public int id;
    public int pos;
    public EquipChangeRequest()
    {
        this.serviceName = "/api/equip/use";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
    public EquipChangeRequest(int id, int selectPos):this()
    {
        this.id = id;
        this.pos = selectPos;
    }
}