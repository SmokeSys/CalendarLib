using System;
using System.Collections;
using System.Collections.Generic;

namespace CalendarLib
{
    /// <summary>
    /// From (0)highest to (3)lowest
    /// </summary>
    public enum Post
    {
        Director, Superviser, Manager, Employee
    }

    public enum Department
    {
        Marketing, IT, Security, Development, Research, Finance, Transport
    }

    /// <summary>
    /// User's might, creating when logging in
    /// </summary>
    public class User
    {
        public string Name { get; }

        /// <summary>
        /// (director) 0 - highest, (employee) 3 - lowest
        /// </summary>
        public Post Post { get; }
        public Department Department { get; }

        public User(string name, Post p, Department d)
        {
            Name = name; Post = p; Department = d;
        }

        public override string ToString()
        {
            return String.Format("{0} , {1} in {2} Department", Name, Post, Department);
        }
    }


    public enum EventType
    {
        Событие, Мероприятие, Планерка, Совещание, Командировка, Семинар, Другое
    }

    public enum EventPlace
    {
        Conference, Marketing, IT, Security, Development, Research, Finance, Transport
    }
    /// <summary>
    /// Event
    /// </summary>
    class CalendarEvent
    {

        #region Fields
        public string Description { get { return _description; } }
        private string _description = "";
        public EventType EventType { get { return _eventtype; } }
        private EventType _eventtype;
        public DateTime StartTime { get { return _starttime; } }
        private DateTime _starttime;
        public DateTime EndTime { get { return _endtime; } }
        private DateTime _endtime;
        public EventPlace EventPlace { get{ return _eventplace; } }
        private EventPlace _eventplace;
        public List<User> MemberName { get { return _members; } } //uvedomlenie 
        private List<User> _members;

        /// <summary>
        /// From 0(highest) to 3(lowest). Default Priority = 3
        /// </summary>
        public int Priority { get; } = 3;
        #endregion

        #region Constructors
        public CalendarEvent(EventType et, DateTime s, DateTime e, EventPlace ep)
        {
            _eventtype = et; _starttime = s; _endtime = e; _eventplace = ep; _members = new List<User>();
        }

        public CalendarEvent(EventType et, DateTime s, DateTime e, EventPlace ep, List<User> n)
        {
            _eventtype = et; _starttime = s; _endtime = e; _eventplace = ep;
            if (n.Count > 0) _members = n;
        }

        public CalendarEvent(EventType et, DateTime s, DateTime e, EventPlace ep, int p)
        {
            _eventtype = et; _starttime = s; _endtime = e; _eventplace = ep; _members = new List<User>();
            if (p >= 0 || p <= 3) Priority = p;
        }
        public CalendarEvent(string d, EventType et, DateTime s, DateTime e, EventPlace ep, int p)
        {
            _description = d; _eventtype = et; _starttime = s; _endtime = e; _eventplace = ep; _members = new List<User>();
            if (p >= 0 || p <= 3) Priority = p;
        }
        public CalendarEvent(string d, EventType et, DateTime s, DateTime e, EventPlace ep, int p, List<User> n)
        {
            _description = d; _eventtype = et; _starttime = s; _endtime = e; _eventplace = ep;
            if (p >= 0 || p <= 3) Priority = p;
            if (n.Count > 0)
                _members = n;
        }
        #endregion

        #region Methods
        /// <summary>
        /// </summary>
        /// <param name="u">Current User</param>
        /// <param name="ct">Changing Type</param>
        /// <param name="newp">Type New Value</param>
        /// <returns>Returns -1 if User havent permission, and 0 if smtg going wrong</returns>
        public int Change(User u, string ct, object newp)
        {
            if ((int)u.Post > Priority) return -1;

            try
            {
                switch (ct.ToLower())
                {
                    case "Description":
                        _description = (string)newp;
                        break;
                    case "EventType":
                        _eventtype = (EventType)newp;
                        break;
                    case "StartTime":
                        _starttime = (DateTime)newp;
                        break;
                    case "EndTime":
                        _endtime = (DateTime)newp;
                        break;
                    case "EventPlace":
                        _eventplace = (EventPlace)newp;
                        break;
                    case "MemberName":
                        _members = (List<User>)newp;
                        break;
                    default: return 0;
                }
            }
            catch { return 0; }

            return 1;
        }


        public void AddMember(User add)
        {
            _members.Add(add);
        }

        /// <summary>
        /// </summary>
        /// <param name="u"></param>
        /// <returns>Returns -1 if user havent permission</returns>
        public int DeleteMember(User u, User del)
        {
            if ((int)u.Post == 3) return -1;
            _members.Remove(del);
            return 0;
        }

        public override string ToString()
        {
            return String.Format("{0} from {1} to {2} in {3}. {4}", _eventtype, _starttime, _endtime, _eventplace, _description);
        }
        #endregion
    }

    class DateList : IEnumerable<CalendarEvent> //enum?
    {
        List<CalendarEvent> l;
        public DateList()
        {
            l = new List<CalendarEvent>();
        }

        public void Add(CalendarEvent c)
        {           
            int i = 0;
            bool f = false;
            for(; i < l.Count; i++)
            {
                if (l[i].StartTime > c.StartTime)
                {
                    f = true;
                    break;
                }
            }
            l.Insert(f ? i-- : i, c);
        }
        public void Remove(CalendarEvent c)
        {
            l.Remove(c);
        }
        public CalendarEvent AtIndex(int index)
        {
            return l[index];
        }

        public IEnumerator<CalendarEvent> GetEnumerator()
        {
            return l.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return l.GetEnumerator();
        }
    }

    public class Calendar
    {
        public DateTime CurrentTime { get; }
        DateList Events { get; }

    }
}
