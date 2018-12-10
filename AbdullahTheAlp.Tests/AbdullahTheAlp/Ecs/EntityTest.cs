using NUnit.Framework;
using DeenGames.AbdullahTheAlp.Ecs;

namespace DeenGames.AbdullahTheAlp.Tests.Ecs
{
    [TestFixture]
    public class EntityTests
    {
        [Test]
        public void SetOverridesPreviouslySetValue()
        {
            var entity = new Entity();
            var type = typeof(IntComponent);

            var first = new IntComponent(17);
            entity.Set(first);
            Assert.That(entity.Get(type), Is.EqualTo(first));
            
            var second = new IntComponent(120);
            entity.Set(second);
            Assert.That(entity.Get(type), Is.EqualTo(second));
        }
    }

    class IntComponent : AbstractComponent
    {
        public int Value { get; private set; }
        public IntComponent(int value)
        {
            this.Value = value;
        }
    }
}