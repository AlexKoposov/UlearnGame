using NUnit.Framework;
using Project_Jumper;
using System.Drawing;

namespace Project_Jumper_Tests
{
    class Ball_Should
    {
        private Map map;

        [SetUp]
        public void Setup()
        {
            map = new Map();
        }

        [Test]
        public void NotJump()
        {
            var player = new Player(new Point(1, 1), 10, Gamemodes.Ball) { Jumping = true };
            var prevY = player.Y;
            for (var i = 0; i < 10000; i++)
                player.Move(map, 10);
            Assert.IsTrue(prevY < player.Y);
        }

        [Test]
        public void NotJumpWithChangedGravity()
        {
            var player = new Player(new Point(1, 14), 10, Gamemodes.Ball, -1) { Jumping = true };
            var prevY = player.Y;
            for (var i = 0; i < 10000; i++)
                player.Move(map, 10);
            Assert.IsTrue(prevY > player.Y);
        }

        [Test]
        public void ChamgeGravity()
        {
            var player = new Player(new Point(1, 1), 10, Gamemodes.Ball) { Jumping = true };
            player.Move(map, 10);
            Assert.IsTrue(player.Gravity == -1);
        }

        [Test]
        public void ChamgeGravityWithChangedGravity()
        {
            var player = new Player(new Point(1, 14), 10, Gamemodes.Ball, -1) { Jumping = true };
            player.Move(map, 10);
            Assert.IsTrue(player.Gravity == 1);
        }

        [Test]
        public void NotChamgeGravityWhenFalling()
        {
            var player = new Player(new Point(1, 2), 10, Gamemodes.Ball) { Jumping = true, Falling = true };
            player.Move(map, 10);
            Assert.IsTrue(player.Gravity == 1);
        }
    }
}
