using MemoryPack;
using System.Text;
using SystemModule;
using SystemModule.Packets.ClientPackets;

namespace TestProject1
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //MemoryPackFormatterProvider.Register<Ability>(new AbilityFormatter());
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        [Test]
        public void EncodeAbility()
        {
            Ability ability = new Ability();
            ability.Level = 10;
            ability.DC = 1;
            ability.SC = 2;
            ability.MC = 3;
            var strs = EDCode.EncodeMessage(ability);
            Console.WriteLine(strs);
        }
    }
}