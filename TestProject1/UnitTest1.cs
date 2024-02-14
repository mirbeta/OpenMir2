using OpenMir2;
using OpenMir2.Packets.ClientPackets;
using System.Text;

namespace GameSrvTest
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
            byte[] s = new byte[2] { 154, 1 };
            BitConverter.ToUInt16(s);

            Ability ability = new Ability();
            ability.Level = 38;
            ability.DC = 4167;
            ability.SC = 6411;
            ability.MC = 0;
            ability.AC = 0;
            ability.Exp = 16230;
            ability.MaxExp = 2000000;
            ability.ExpCount = 0;
            ability.ExpMaxCount = 0;
            ability.HP = 78;
            ability.MP = 128;
            ability.MaxHP = 350;
            ability.MaxMP = 410;
            ability.HandWeight = 46;
            ability.MaxHandWeight = 46;
            ability.MaxWearWeight = 44;
            ability.MaxWeight = 411;
            ability.Weight = 89;
            ability.WearWeight = 7;
            ability.MAC = 2051;
            string strs = EDCode.EncodeMessage(ability);

            //^dhgdg`kwhhgdgqivdh?dzihJehcddhknchddDdKjdqodKiig\>d^<
            //^dhgdg`kwhhgdgqivdh?dzihJehcddhknchddDdKjdqodKiig\>d^<
            Console.WriteLine(strs);
        }
    }
}