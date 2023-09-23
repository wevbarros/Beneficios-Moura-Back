public class Level
{
    public int Id { get; set; }
    public string Descricao { get; set; }
    public int Cod { get; set; }

    public ICollection<User> Users { get; set; }
}
