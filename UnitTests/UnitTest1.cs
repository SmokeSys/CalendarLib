using System;
using CalendarLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CalLibTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void EventCharactericChangeTest()
        {
            DateTime tempTime = DateTime.Now;
            CalendarEvent n = new CalendarEvent(EventType.Другое, tempTime, tempTime.AddHours(12), EventPlace.Security, 1);

            User tempUser = new User("Petrov", (Post)2, (Department)2);
            n.AddMember(tempUser);

            int temp = n.Change(tempUser, "priority", 3);
            Assert.AreEqual(-1, temp);

            tempUser = new User("Pertov", (Post)0, Department.Research);
            temp = n.Change(tempUser, "PriOrity", "strg");
            Assert.AreEqual(0, temp);

            temp = n.Change(tempUser, "Description", "dadada");
            Assert.AreEqual(1, temp);
            Assert.AreEqual("dadada", n.Description);
        }
        
        [TestMethod]
        public void SortChecker()
        {
            CalendarL c = new CalendarL();

            CalendarEvent _1 = new CalendarEvent(EventType.Другое , new DateTime(2020, 01, 01, 0, 0, 0), new DateTime(2020, 01, 01, 12, 0, 0), EventPlace.IT, 1);
            CalendarEvent _2 = new CalendarEvent(EventType.Другое, new DateTime(2020, 01, 01, 4, 0, 0), new DateTime(2020, 01, 01, 16, 0, 0), EventPlace.Security, 1);
            CalendarEvent _3 = new CalendarEvent(EventType.Другое, new DateTime(2019, 12, 31, 16, 0, 0), new DateTime(2020, 01, 01, 4, 0, 0), EventPlace.Finance, 1);

            c.AddEvent(_1);
            c.AddEvent(_2);
            c.AddEvent(_3);

            Assert.AreEqual(EventPlace.Finance, c.Events[0].EventPlace);
            Assert.AreEqual(EventPlace.IT, c.Events[1].EventPlace);
            Assert.AreEqual(EventPlace.Security, c.Events[2].EventPlace);
        }
        
    }
}
