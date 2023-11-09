using System.Reflection;
using System.Text.Json;
using DartLeague;

var match = new ELOMatch();

var jsonFilePath = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "PlayersElo.json");
const int startElo = 1000;
Console.WriteLine(Assembly.GetExecutingAssembly().Location);
var jsonData = File.ReadAllText(jsonFilePath);
var dartLeaguePlayersElo = JsonSerializer.Deserialize<DartLeaguePlayersElo>(jsonData);

Console.WriteLine("Add players to the game, if you added all already write 'dupa' as player name");

while (true)
{
    string? addedPlayer;
    while (true)
    {
        Console.WriteLine("");
        Console.WriteLine("Name of player:");
        addedPlayer = Console.ReadLine();

        if (addedPlayer is null)
        {
            Console.WriteLine("You need to write anything");
        }else if(addedPlayer is "dupa")
        {
            CalculateElo();
            Environment.Exit(0);
        }
        else
        {
            break;
        }
    }
    
    int intPlace;
    while (true)
    {
        Console.WriteLine($"Place of {addedPlayer}:");
        var place = Console.ReadLine();

        var isInt = int.TryParse(place, out intPlace);

        if (isInt)
        {
            break;
        }

        Console.WriteLine("value must be int");
    }

    dartLeaguePlayersElo.Players ??= new List<Player>();
    
    if (dartLeaguePlayersElo is null)
    {
        throw new Exception("error during deserializing json");
    }
    
    if (dartLeaguePlayersElo.Players.All(player => player.Name != addedPlayer))
    {
        Console.WriteLine("There is no such player in database do you want to create new one?(Y/N)");
        var answer = Console.ReadLine();

        switch (answer)
        {
            case "N" or "No" or "no" or "n":
                continue;
            case "Y" or "Yes" or "yes" or "y":
                dartLeaguePlayersElo.Players.Add(new Player(addedPlayer, startElo));
                match.addPlayer(addedPlayer, intPlace, dartLeaguePlayersElo.Players.Find(player => player.Name == addedPlayer).Elo);
                continue;
            default:
                Console.WriteLine("that is wrong answer.");
                break;
        }
    }
    else
    {
        match.addPlayer(addedPlayer, intPlace, dartLeaguePlayersElo.Players.Find(player => player.Name == addedPlayer).Elo);
    }
}

void CalculateElo()
{
    var jsonFilePath = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "PlayersElo.json");
    
    match.calculateELOs();
    
    Console.WriteLine("New elo:");
    
    foreach (var currentPlayer in dartLeaguePlayersElo.Players)
    {
        Console.WriteLine("Name: " + currentPlayer.Name + " New elo: " + currentPlayer.Elo);
        
        if (match.players.All(player => currentPlayer.Name != player.name)) continue;
        
        currentPlayer.Elo = match.getELO(currentPlayer.Name);
    }
    File.WriteAllText(jsonFilePath, JsonSerializer.Serialize(dartLeaguePlayersElo));
}