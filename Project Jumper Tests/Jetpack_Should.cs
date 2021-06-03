using NUnit.Framework;
using Project_Jumper;
using System.Drawing;

namespace Project_Jumper_Tests
{
    class Jetpack_Should
    {
        private Map map;

        [SetUp]
        public void Setup()
        {
            map = new Map();
        }

        [Test]
        public void Fly()
        {
            var player = new Player(new Point(1, 1), 10, Gamemodes.Jetpack) { Flying = true };
            var prevY = player.Y;
            for (var i = 0; i < 1000; i++)
                player.Move(map, 10);
            Assert.IsTrue(prevY < player.Y);
        }

        [Test]
        public void FlyWithChangedGravity()
        {
            var player = new Player(new Point(1, 5), 10, Gamemodes.Jetpack, -1) { Flying = true };
            var prevY = player.Y;
            for (var i = 0; i < 1000; i++)
                player.Move(map, 10);
            Assert.IsTrue(prevY > player.Y);
        }

        [Test]
        public void StopWhenCollide()
        {
            var player = new Player(new Point(1, 14), 10, Gamemodes.Jetpack) { Flying = true };
            var prevY = player.Y;
            for (var i = 0; i < 1000; i++)
                player.Move(map, 10);
            Assert.IsTrue(prevY == player.Y);
        }

        [Test]
        public void StopWhenCollideWithChangedGravity()
        {
            var player = new Player(new Point(1, 1), 10, Gamemodes.Jetpack, -1) { Flying = true };
            var prevY = player.Y;
            for (var i = 0; i < 1000; i++)
                player.Move(map, 10);
            Assert.IsTrue(prevY == player.Y);
        }

        [Test]
        public void FallWhenNotFlying()
        {
            var player = new Player(new Point(1, 3), 10, Gamemodes.Jetpack) { Flying = false };
            var prevY = player.Y;
            for (var i = 0; i < 1000; i++)
                player.Move(map, 10);
            Assert.IsTrue(prevY > player.Y);
        }

        [Test]
        public void FallWhenNotFlyingWithChangedGravity()
        {
            var player = new Player(new Point(1, 3), 10, Gamemodes.Jetpack, -1) { Flying = false };
            var prevY = player.Y;
            for (var i = 0; i < 1000; i++)
                player.Move(map, 10);
            Assert.IsTrue(prevY < player.Y);
        }
    }
}
