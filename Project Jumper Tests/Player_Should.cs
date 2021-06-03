using NUnit.Framework;
using Project_Jumper;
using System.Drawing;

namespace Project_Jumper_Tests
{
    public class Player_Should
    {
        private Map map;

        [SetUp]
        public void Setup()
        {
            map = new Map();
        }

        [Test]
        public void MoveRight()
        {
            var player = new Player(new Point(1, 1), 10) { MovingRight = true };
            var prevX = player.X;
            player.Move(map, 10);
            Assert.IsTrue(prevX < player.X);
        }

        [Test]
        public void MoveRightWithChangedGravity()
        {
            var player = new Player(new Point(1, 1), 10) { MovingRight = true, Gravity = -1 };
            var prevX = player.X;
            player.Move(map, 10);
            Assert.IsTrue(prevX < player.X);
        }

        [Test]
        public void MoveLeft()
        {
            var player = new Player(new Point(2, 1), 10) { MovingLeft = true };
            var prevX = player.X;
            player.Move(map, 10);
            Assert.IsTrue(prevX > player.X);
        }

        [Test]
        public void MoveLeftWithChangedGravity()
        {
            var player = new Player(new Point(2, 1), 10) { MovingLeft = true, Gravity = -1 };
            var prevX = player.X;
            player.Move(map, 10);
            Assert.IsTrue(prevX > player.X);
        }

        [Test]
        public void Fall()
        {
            var player = new Player(new Point(1, 2), 10) { Falling = true };
            var prevY = player.Y;
            for (var i = 0; i < 100; i++)
                player.Move(map, 10);
            Assert.IsTrue(prevY > player.Y);
        }

        [Test]
        public void FallWithChangedGravity()
        {
            var player = new Player(new Point(1, 2), 10) { Falling = true, Gravity = -1 };
            var prevY = player.Y;
            for (var i = 0; i < 100; i++)
                player.Move(map, 10);
            Assert.IsTrue(prevY < player.Y);
        }

        [Test]
        public void Jump()
        {
            var player = new Player(new Point(1, 2), 10) { Jumping = true };
            var prevY = player.Y;
            player.Move(map, 10);
            Assert.IsTrue(prevY < player.Y);
        }

        [Test]
        public void JumpWithChangedGravity()
        {
            var player = new Player(new Point(1, 2), 10) { Jumping = true, Gravity = -1 };
            var prevY = player.Y;
            player.Move(map, 10);
            Assert.IsTrue(prevY > player.Y);
        }

        [TestCase(3, 1, 0, 0)]
        [TestCase(3, 1, 1, 0)]
        [TestCase(1, 1, -1, 0)]
        [TestCase(1, 1, 0, -1)]
        [TestCase(3, 1, 1, -1)]
        [TestCase(1, 1, -1, -1)]
        public void StopWhenCollide(int x, int y, int dirX, int dirY)
        {
            var player = new Player(new Point(x, y), 10);
            if (dirX > 0) player.MovingRight = true;
            if (dirX < 0) player.MovingLeft = true;
            if (dirY < 0) player.Falling = true;
            var prevX = player.X;
            var prevY = player.Y;
            for (var i = 0; i < 100; i++)
                player.Move(map, 10);
            Assert.IsTrue(prevX == player.X && prevY == player.Y);
        }

        [TestCase(15, 7, 0, 0)]
        [TestCase(15, 7, 1, 0)]
        [TestCase(17, 14, -1, 0)]
        [TestCase(13, 7, 0, -1)]
        [TestCase(15, 7, 1, -1)]
        [TestCase(17, 14, -1, -1)]
        public void StopWhenCollideWithChangedGravity(int x, int y, int dirX, int dirY)
        {
            map.ChangeToNextLevel();
            var player = new Player(new Point(x, y), 10) { Gravity = -1 };
            if (dirX > 0) player.MovingRight = true;
            if (dirX < 0) player.MovingLeft = true;
            if (dirY < 0) player.Falling = true;
            var prevX = player.X;
            var prevY = player.Y;
            for (var i = 0; i < 100; i++)
                player.Move(map, 10);
            Assert.IsTrue(prevX == player.X && prevY == player.Y);
        }

        [Test]
        public void JumpOnJumpOrb()
        {
            var player = new Player(new Point(15, 4), 10) { TriggerTicks = 1 };
            var prevY = player.Y;
            player.ReactToOrbs(map, 10);
            player.Move(map, 10);
            Assert.IsTrue(prevY < player.Y);
        }

        [Test]
        public void ChangeGravityOnGravityOrb()
        {
            map.ChangeToNextLevel();
            var player = new Player(new Point(10, 4), 10) { TriggerTicks = 1 };
            player.ReactToOrbs(map, 10);
            Assert.IsTrue(player.Gravity == -1);
        }

        [Test]
        public void DieOnSpikes()
        {
            var player = new Player(new Point(5, 2), 10) { Falling = true };
            for (var i = 0; i < 100; i++)
                player.Move(map, 10);
            Assert.IsTrue(player.Dead);
        }

        [Test]
        public void DieOnSaws()
        {
            map.ChangeToNextLevel();
            var player = new Player(new Point(2, 9), 10) { Falling = true };
            for (var i = 0; i < 100; i++)
                player.Move(map, 10);
            Assert.IsTrue(player.Dead);
        }
    }
}
