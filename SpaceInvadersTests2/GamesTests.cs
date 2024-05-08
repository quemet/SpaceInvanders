using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaceInvaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Tests
{
    [TestClass()]
    public class GamesTests
    {
        public Games games = new Games();
        [TestMethod()]
        public void CreateShipTest()
        {
            Ship ship = games.CreateShip();

            Assert.AreEqual(ship.isBulletActive, false);
            Assert.AreEqual(ship.x, 0);
        }

        [TestMethod()]
        public void CreationEnnemyTest()
        {
            List<Ennemy> ennemies = games.CreationEnnemy();

            Assert.AreEqual(ennemies.Count, 10);
            Assert.AreEqual(ennemies[0].id, 0);
        }
    }
}