using System.Collections.Generic;
using System.Linq;

namespace KataBowling
{
    public class BowlingGame
    {
        private readonly ScoringStrategy _scoringStrategy;
        private readonly Dictionary<Player, PlayerRolls> _rolls = new Dictionary<Player, PlayerRolls>();

        public BowlingGame(ScoringStrategy scoringStrategy)
        {
            _scoringStrategy = scoringStrategy;
            Players = new List<Player>();
        }

        #region Properties

        public List<Player> Players { get; private set; }
        public Player CurrentPlayer { get; private set; }
        public int CurrentFrame { get; private set; }

        #endregion

        public void Join(Player player)
        {
            Players.Add(player);
            _rolls.Add(player, new PlayerRolls());

            if (CurrentPlayer == null)
            {
                CurrentPlayer = player;
            }
        }

        public void Roll(Player player, int pinsKnockedDown)
        {
            var playerRolls = _rolls[player];
            playerRolls.Add(pinsKnockedDown);

            if (player.HasFinishedFrame(CurrentFrame, playerRolls))
            {
                ChangeTurnTo(Next(player));
            }
        }

        public int Score(Player player)
        {
            return _scoringStrategy.Calculate(_rolls[player]);
        }

        public Player PlayUnder(string playerName)
        {
            var player = new Player(playerName, this, _scoringStrategy);
            Join(player);
            return player;
        }

        public bool IsOver()
        {
            var isOver = Players.All(player =>
            {
                return player.HasFinishedFrame(9, _rolls[player]);
            });

            return isOver;
        }

        #region Private Methods

        private void ChangeTurnTo(Player nextPlayer)
        {
            CurrentPlayer = nextPlayer;

            if (CurrentPlayer == Players[0])
            {
                CurrentFrame++;
            }
        }

        private Player Next(Player player)
        {
            var nextPlayerIndex = Players.IndexOf(player) + 1;
            if (nextPlayerIndex >= Players.Count)
            {
                nextPlayerIndex = 0;
            }
            return Players[nextPlayerIndex];
        }

        #endregion
    }
}
