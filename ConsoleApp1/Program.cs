public class Player
{
    public string Username { get; set; }
    public decimal Balance { get; set; }
    public List<Transaction> TransactionHistory { get; set; }
    public List<GameResult> GameHistory { get; set; }

    public Player(string username, decimal balance)
    {
        Username = username;
        Balance = balance;
        TransactionHistory = new List<Transaction>();
        GameHistory = new List<GameResult>();
    }
}
public class Transaction
{
    public decimal Amount { get; set; }
    public DateTime Timestamp { get; set; }
    public TransactionType Type { get; set; }
    public Transaction(decimal amount, TransactionType type)
    {
        Amount = amount;
        Type = type;
        Timestamp = DateTime.Now;
    }
    public Transaction(decimal amount, TransactionType type, DateTime timestamp)
    {
        Amount = amount;
        Type = type;
        Timestamp = timestamp;
    }
}
public enum TransactionType
{
    Deposit,
    Withdrawal,
    Bet
}
public class GameResult
{
    public string GameName { get; set; }
    public decimal AmountWon { get; set; }
    public DateTime Timestamp { get; set; }

    public GameResult(string gameName, decimal amountWon, DateTime timestamp)
    {
        GameName = gameName;
        AmountWon = amountWon;
        Timestamp = timestamp;
    }
}
public class AccountManager
{
    private List<Player> players;

    public AccountManager()
    {
        players = new List<Player>();
    }

    public void RegisterPlayer(string username, decimal initialBalance)
    {
        if (PlayerExists(username))
        {
            Console.WriteLine("Игрок с таким именем пользователя уже существует.");
            return;
        }

        var player = new Player(username, initialBalance);
        players.Add(player);
        Console.WriteLine("Игрок успешно зарегистрировался.");
        player.Balance += 100;
        Console.WriteLine($"Бонус за регистрацию составил 100 fsp, Ваш текущий баланс - {player.Balance}.");
    }

    public void Deposit(string username, decimal amount)
    {
        var player = GetPlayer(username);
        if (player == null)
            return;

        player.Balance += amount;
        player.TransactionHistory.Add(new Transaction(amount, TransactionType.Deposit));
        Console.WriteLine($"Депозит успешен. Новый баланс: {player.Balance}");
    }

    public void Withdraw(string username, decimal amount)
    {
        var player = GetPlayer(username);
        if (player == null)
            return;

        if (player.Balance < amount)
        {
            Console.WriteLine("Недостаточно средств.");
            return;
        }

        player.Balance -= amount;
        player.TransactionHistory.Add(new Transaction(amount, TransactionType.Withdrawal));
        Console.WriteLine($"Ставка сделана. Новый баланс: {player.Balance}");
    }

    public void ViewTransactionHistory(string username)
    {
        var player = GetPlayer(username);
        if (player == null)
            return;

        Console.WriteLine($"История транзакций для {username}:");
        foreach (var transaction in player.TransactionHistory)
        {
            Console.WriteLine($"Type: {transaction.Type}, Amount: {transaction.Amount}, Timestamp: {transaction.Timestamp}");
        }
    }

    public void ViewGameHistory(string username)
    {
        var player = GetPlayer(username);
        if (player == null)
            return;

        Console.WriteLine($"История игр для {username}:");
        foreach (var gameResult in player.GameHistory)
        {
            Console.WriteLine($"Game: {gameResult.GameName}, Amount Won: {gameResult.AmountWon}, Timestamp: {gameResult.Timestamp}");
        }
    }

    public void WithdrawAllFunds(string username)
    {
        var player = GetPlayer(username);
        if (player == null)
            return;

        decimal amountToWithdraw = player.Balance;
        player.Balance = 0;
        player.TransactionHistory.Add(new Transaction(amountToWithdraw, TransactionType.Withdrawal));
        Console.WriteLine($"Вывод {amountToWithdraw} успешный. Новый баланс: {player.Balance}");
    }

    public Player GetPlayer(string username)
    {
        var player = players.Find(p => p.Username == username);
        if (player == null)
        {
            Console.WriteLine("Игрок не найден.");
            return null;
        }
        return player;
    }

    public bool PlayerExists(string username)
    {
        return players.Exists(p => p.Username == username);
    }
}


internal class Program
{
    static void Main(string[] args)
    {
        AccountManager accountManager = new AccountManager();
        Casino games = new Casino(accountManager);

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("1. Зарегистрация нового игрока");
            Console.WriteLine("2. Внесите деньги");
            Console.WriteLine("3. Вывод денег");
            Console.WriteLine("4. Просмотр истории транзакций");
            Console.WriteLine("5. Просмотр истории игр");
            Console.WriteLine("6. Выведите все средства");
            Console.WriteLine("7. Выбрать игру");
            Console.WriteLine("8. Выход");

            Console.Write("Введите свой выбор: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Введите имя игрока: ");
                    string username = Console.ReadLine();
                    Console.Write("Введите первоначальный баланс: ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal initialBalance))
                    {
                        Console.WriteLine("Неверный ввод баланса.");
                        break;
                    }
                    accountManager.RegisterPlayer(username, initialBalance);
                    break;

                case "2":
                    Console.Write("Введите имя игрока: ");
                    string depositUsername = Console.ReadLine();
                    Console.Write("Введите сумму для внесения: ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal depositAmount))
                    {
                        Console.WriteLine("Неверный ввод суммы.");
                        break;
                    }
                    accountManager.Deposit(depositUsername, depositAmount);
                    break;

                case "3":
                    Console.Write("Введите имя игрока: ");
                    string withdrawUsername = Console.ReadLine();
                    Console.Write("Введите сумму для вывода: ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal withdrawAmount))
                    {
                        Console.WriteLine("Неверный ввод суммы.");
                        break;
                    }
                    accountManager.Withdraw(withdrawUsername, withdrawAmount);
                    break;

                case "4":
                    Console.Write("Введите имя игрока: ");
                    string transactionHistoryUsername = Console.ReadLine();
                    accountManager.ViewTransactionHistory(transactionHistoryUsername);
                    break;

                case "5":
                    Console.Write("Введите имя игрока: ");
                    string gameHistoryUsername = Console.ReadLine();
                    accountManager.ViewGameHistory(gameHistoryUsername);
                    break;

                case "6":
                    Console.Write("Введите имя игрока: ");
                    string withdrawAllUsername = Console.ReadLine();
                    accountManager.WithdrawAllFunds(withdrawAllUsername);
                    break;
                case "7":

                    games.ChooseGame();
                    break;
                case "8":
                    exit = true;
                    break;

                default:
                    Console.WriteLine("Неверный выбор. Пожалуйста, введите число от 1 до 8.");
                    break;
            }

            Console.WriteLine();
        }
    }
}
}
