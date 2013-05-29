using System;

namespace KataBowling
{
    public class Player
    {
        private static readonly int MaxFramesCount = 9;

        private readonly BowlingGame _game;
        private readonly ScoringStrategy _scoringStrategy;

        public Player(string playerName, BowlingGame game, ScoringStrategy scoringStrategy)
        {
            _game = game;
            _scoringStrategy = scoringStrategy;

            Name = playerName;
        }

        public string Name { get; set; }

        public void Roll(int pinsKnockedDown)
        {
            _game.Roll(this, pinsKnockedDown);
        }

        public int Score()
        {
            return _game.Score(this);
        }

        public bool HasFinishedFrame(int frame, PlayerRolls playerRolls)
        {
            var frameIndex = FrameIndex(playerRolls, frame);

            if (_scoringStrategy.IsStrike(playerRolls, frameIndex))
            {
                frameIndex++;
                if (frame == MaxFramesCount) frameIndex += 2;
            }
            else if (_scoringStrategy.IsSpare(playerRolls, frameIndex))
            {
                frameIndex += 2;
                if (frame == MaxFramesCount) frameIndex++;
            }
            else
            {
                frameIndex += 2;
            }

            var playerFinished = playerRolls.Count >= frameIndex;
            return playerFinished;
        }

        public override string ToString()
        {
            return "Player: " + Name;
        }

        private int FrameIndex(PlayerRolls playerRolls, int frame)
        {
            int frameIndex = 0;

            while (frame-- > 0)
            {
                if (_scoringStrategy.IsStrike(playerRolls, frameIndex))
                {
                    frameIndex++;
                }
                else
                {
                    frameIndex += 2;
                }
            }

            return frameIndex;
        }
    }
}