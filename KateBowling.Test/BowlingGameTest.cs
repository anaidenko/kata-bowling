using System;
using FluentAssertions;
using KataBowling;
using KataBowling.Test.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KataBowling.Test
{
    [TestClass]
    public class BowlingGameTest
    {
        private BowlingGame game;
        private Player player1;
        private Player player2;

        [TestInitialize]
        public void SetUp()
        {
            var scoringStrategy = new ScoringStrategy();
            game = new BowlingGame(scoringStrategy);
            player1 = new Player("Player1", game, scoringStrategy);
            player2 = new Player("Player2", game, scoringStrategy);
        }

        [TestMethod]
        public void TestJoinGame()
        {
            game.Join(player1);
            game.Join(player2);
            game.Players.Should().ContainInOrder(player1, player2);
        }

        [TestMethod]
        public void TestLooser()
        {
            game.Join(player1);
            20.Times(() => player1.Roll(0));
            player1.Score().Should().Be(0);
        }

        [TestMethod]
        public void TestAllOnes()
        {
            game.Join(player1);
            20.Times(() => player1.Roll(1));
            player1.Score().Should().Be(20);
        }

        [TestMethod]
        public void TestOneSpare()
        {
            game.Join(player1);
            RollSpare(player1);
            player1.Roll(3);
            17.Times(() => player1.Roll(0));
            player1.Score().Should().Be(16);
        }

        [TestMethod]
        public void TestOneStrike()
        {
            game.Join(player1);
            RollStrike(player1);
            player1.Roll(5);
            player1.Roll(2);
            16.Times(() => player1.Roll(0));
            player1.Score().Should().Be(24);
        }

        [TestMethod]
        public void TestPerfectPlayer()
        {
            game.Join(player1);
            12.Times(() => player1.Roll(10));
            player1.Score().Should().Be(300);
        }

        [TestMethod]
        public void Test2LoosersFinishedTheGame()
        {
            game.Join(player1);
            game.Join(player2);

            10.Times(() =>
            {
                Twice(() => player1.Roll(0));
                Twice(() => player2.Roll(0));
            });

            player1.Score().Should().Be(0);
            player2.Score().Should().Be(0);
        }

        [TestMethod]
        public void TestLooserVsPerfectPlayer()
        {
            game.Join(player1);
            game.Join(player2);

            10.Times(() =>
            {
                Twice(() => player1.Roll(0));
                player2.Roll(10);
            });

            player2.Roll(10);
            player2.Roll(10);

            player1.Score().Should().Be(0);
            player2.Score().Should().Be(300);
        }

        [TestMethod]
        public void TestGameIsOver()
        {
            game.Join(player1);
            20.Times(() => player1.Roll(0));
            game.IsOver().Should().BeTrue();
        }

        [TestMethod]
        public void TestGameIsNotOverOnLastSpare()
        {
            game.Join(player1);
            18.Times(() => player1.Roll(0));
            RollSpare(player1);
            game.IsOver().Should().BeFalse();
        }

        [TestMethod]
        public void TestGameIsNotOverOnFirstStrikeInLastFrame()
        {
            game.Join(player1);
            18.Times(() => player1.Roll(0));
            RollStrike(player1);
            game.IsOver().Should().BeFalse();
        }

        [TestMethod]
        public void TestGameIsNotOverOnSecondStrikeInLastFrame()
        {
            game.Join(player1);
            18.Times(() => player1.Roll(0));
            RollStrike(player1);
            RollStrike(player1);
            game.IsOver().Should().BeFalse();
        }

        [TestMethod]
        public void Should_track_player_turn()
        {
            game.Join(player1);
            game.Join(player2);

            game.CurrentPlayer.Should().Be(player1);

            Twice(() => player1.Roll(0));
            game.CurrentPlayer.Should().Be(player2);

            player2.Roll(0);
            game.CurrentPlayer.Should().Be(player2);

            player2.Roll(0);
            game.CurrentPlayer.Should().Be(player1);
        }

        [TestMethod]
        public void Should_track_current_frame()
        {
            game.Join(player1);
            game.CurrentFrame.Should().Be(0);
            player1.Roll(0);
            game.CurrentFrame.Should().Be(0);
            player1.Roll(0);
            game.CurrentFrame.Should().Be(1);
            player1.Roll(0);
            game.CurrentFrame.Should().Be(1);
            player1.Roll(0);
            game.CurrentFrame.Should().Be(2);
            player1.Roll(10);
            game.CurrentFrame.Should().Be(3);
            player1.Roll(6);
            game.CurrentFrame.Should().Be(3);
            player1.Roll(4);
            game.CurrentFrame.Should().Be(4);
        }

        #region Helper Methods

        private void RollSpare(Player player)
        {
            player.Roll(6);
            player.Roll(4);
        }

        private void RollStrike(Player player)
        {
            player.Roll(10);
        }

        private void Twice(Action action)
        {
            2.Times(action);
        }

        #endregion
    }
}