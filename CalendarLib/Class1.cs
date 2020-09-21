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
            return String.Format("{0}, {1} in {2} Department", Name, Post, Department);
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

     
    public class CalendarEvent
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
        public List<User> Members { get { return _members; } } 
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
                    case "description":
                        _description = (string)newp;
                        break;
                    case "eventtype":
                        _eventtype = (EventType)newp;
                        break;
                    case "starttime":
                        _starttime = (DateTime)newp;
                        break;
                    case "endtime":
                        _endtime = (DateTime)newp;
                        break;
                    case "eventplace":
                        _eventplace = (EventPlace)newp;
                        break;
                    case "membername":
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

        public void AddMembers(List<User> m)
        {
            if (m == null) return;
            _members = m;
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



    public class Calendar
    {
        #region Fields
        public DateTime CurrentTime { get { return _currenttime; } }
        private DateTime _currenttime;
        public List<CalendarEvent> Events { get { return _events; } }
        private List<CalendarEvent> _events;

        #endregion

        #region Constructors

        public Calendar()
        {
            _currenttime = new DateTime(2020, 1, 1, 0, 0, 0);
            _events = new List<CalendarEvent>();
        }

        public Calendar(List<CalendarEvent> e)
        {
            _currenttime = new DateTime(2020, 1, 1, 0, 0, 0);
            _events = e ?? new List<CalendarEvent>();
        }

        public Calendar(DateTime dt)
        {
            _currenttime = dt;
            _events = new List<CalendarEvent>();
        }
        public Calendar(DateTime dt, List<CalendarEvent> e)
        {
            _currenttime = dt;
            _events = e ?? new List<CalendarEvent>();
        }

        #endregion

        #region Methods


        ///// <summary>
        ///// Trouble list format "{index in Calendar.Events} {conflict field name}"
        ///// </summary>
        ///// <returns>Returns trouble list if conflict included, last item - index</returns>
        public void AddEvent(CalendarEvent e)
        {
            int ind = 0;
            int i = 0;
            for (; i < _events.Count; i++)
            {
                if (_events[i].StartTime > e.StartTime)
                {
                    ind = i == 0 ? i : --i;
                    break;
                }
            }
            if (i == _events.Count)
            {
                _events.Add(e);
                return;
            }
            _events.Insert(ind, e);

        }

        public void AddEvents(List<CalendarEvent> l)
        {
            if (l.Count != 0)
                foreach (var t in l)
                    AddEvent(t);
        }

        public void AddAt(CalendarEvent e, int index)
        {
            _events.Insert(index, e);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="e">Уже измененное событие</param>
        /// <returns></returns>
        public void ChangeCharacteric(CalendarEvent old, CalendarEvent n)
        {
            Remove(old);
            AddEvent(n);
        }


        public int IndexOf(CalendarEvent e)
        {
            return _events.IndexOf(e);
        }

        /// <summary>
        /// Removing nearest event
        /// </summary>
        /// <returns></returns>
        public CalendarEvent Remove()
        {
            CalendarEvent temp = _events[0];
            _events.RemoveAt(0);
            return temp;
        }
        

        /// <summary>
        /// Removing specifies element
        /// </summary>
        /// <param name="e"></param>
        public void Remove(CalendarEvent e)
        {
            _events.Remove(e);
        }

        public void RemoveAt(int index)
        {
            _events.RemoveAt(index);
        }

        #endregion
    }
}
