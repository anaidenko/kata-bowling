using System;

namespace KataBowling
{
    public class ScoringStrategy
    {
        public int Calculate(PlayerRolls playerRolls)
        {
            int score = 0;
            var frameIndex = 0;
            var rolls = playerRolls.Safe;

            for (var frame = 0; frame < 10; frame++)
            {
                if (IsStrike(rolls, frameIndex))
                {
                    score += 10 + rolls[frameIndex + 1] + rolls[frameIndex + 2];
                    frameIndex++;
                }
                else if (IsSpare(rolls, frameIndex))
                {
                    score += 10 + rolls.Safe[frameIndex + 2];
                    frameIndex += 2;
                }
                else
                {
                    score += rolls[frameIndex] + rolls[frameIndex + 1];
                    frameIndex += 2;
                }
            }

            return score;
        }

        public bool IsStrike(PlayerRolls playerRolls, int frameIndex)
        {
            return playerRolls.Safe[frameIndex] == 10;
        }

        public bool IsSpare(PlayerRolls playerRolls, int frameIndex)
        {
            return playerRolls.Safe[frameIndex] + playerRolls.Safe[frameIndex + 1] == 10;
        }
    }
}