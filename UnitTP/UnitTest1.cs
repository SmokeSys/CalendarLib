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
            //Assert
        }
    }
}
