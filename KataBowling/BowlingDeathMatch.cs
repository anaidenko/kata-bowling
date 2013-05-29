using System;
using System.Linq;

namespace KataBowling
{
    public class BowlingDeathMatch
    {
        private readonly BowlingGame _game;

        public BowlingDeathMatch(params string[] playerNames)
        {
            _game = new BowlingGame(new ScoringStrategy());
            playerNames.ToList().ForEach(playerName => _game.PlayUnder(playerName));
        }

        public void Play()
        {
            var step = 0;
            var random = new Random();
            while (!_game.IsOver())
            {
                var pins = random.Next(10);
                Console.WriteLine("{0}: {1} rolled {2} pins.", ++step, _game.CurrentPlayer, pins);
                _game.Roll(_game.CurrentPlayer, pins);
            }

            PrintTotalScoresFor();
        }

        private void PrintTotalScoresFor()
        {
            Console.WriteLine();
            Console.WriteLine(new string('=', 34));
            _game.Players.ForEach(player =>
            {
                Console.WriteLine("{0}'s total score: {1}.", player.Name, player.Score());
            });
            Console.WriteLine(new string('=', 34));
        }
    }
}