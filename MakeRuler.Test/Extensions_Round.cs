using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MakeRuler.Extensions.Test
{
    [TestFixture]
    public class Extensions_Round
    {
        [Test]
        public void PositiveNumbers()
        {
            Assert.AreEqual(1, (0.5).RoundMax(), "(0.5).RoundMax(), 1");
            Assert.AreEqual(0, (0.5).RoundMin(), "(0.5).RoundMin(), 0");
            Assert.AreEqual(2, (1.5).RoundMax(), "(1.5).RoundMax(), 2");
            Assert.AreEqual(1, (1.5).RoundMin(), "(1.5).RoundMin(), 1");
        }

    }
}
