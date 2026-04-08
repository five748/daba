public class AwardData
{
    public AwardData()
    {

    }
    public AwardData(int _id, int _num)
    {
        id = _id;
        num = _num;
    }
    public int id;
    public int num;
    public string ToStr()
    {
        return id + "_" + num;
    }
}
