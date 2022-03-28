using System;

namespace TemplateMethod
{
    public abstract class Game
    {
        public void Run()
        {
            Start();
            while (!HaveWinner)
            {
                TakeTurn();
            }
            Console.WriteLine($"Player {WinningPlayer} wins!");
        }

        protected int currentPlayer;
        protected readonly int numberOfPlayers;
        protected abstract void Start();

        protected Game(int numberOfPlayers)
        {
            this.numberOfPlayers = numberOfPlayers;
        }


        protected abstract void TakeTurn();
        protected abstract bool HaveWinner { get; }
        protected abstract int WinningPlayer { get; }
    }

    public class Chess : Game
    {
        private int turn = 1;
        private int maxTurns = 10;
        
        public Chess() : base(2)
        {

        }

        protected override bool HaveWinner => turn == maxTurns;

        protected override int WinningPlayer => currentPlayer;

        protected override void Start()
        {
            Console.WriteLine($"Starting a game of chess with {numberOfPlayers} players");
        }

        protected override void TakeTurn()
        {
            Console.WriteLine($"Turn {turn++} taken by player {currentPlayer}");
            currentPlayer = (currentPlayer + 1) % numberOfPlayers;
        }
    }


    class Program
    {
        static void Main()
        {
            var chess = new Chess();
            chess.Run();
        }
    }
}
