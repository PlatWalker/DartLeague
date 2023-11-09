namespace DartLeague;

public class Player
{
    public Player(string name, int elo)
    {
        Name = name;
        Elo = elo;
    }

    public string Name { get; set; }
    public int Elo { get; set; }
}