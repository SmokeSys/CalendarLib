using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
        Marketing, IT, Security, Development, Research, Finance, Transport, Other
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
            return String.Format("{0}\n{1} in {2} Department", Name, Post, Department);
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
        public ObservableCollection<User> Members { get { return _members; } } 
        private ObservableCollection<User> _members;

        /// <summary>
        /// From 0(highest) to 3(lowest). Default Priority = 3
        /// </summary>
        public int Priority { get; } = 3;
        #endregion

        #region Constructors

        public CalendarEvent(EventType et, DateTime s, DateTime e, EventPlace ep)
        {
            _eventtype = et; _starttime = s; _endtime = e; _eventplace = ep; _members = new ObservableCollection<User>();
        }

        public CalendarEvent(EventType et, DateTime s, DateTime e, EventPlace ep, ObservableCollection<User> n)
        {
            _eventtype = et; _starttime = s; _endtime = e; _eventplace = ep;
            if (n.Count > 0) _members = n;
        }

        public CalendarEvent(EventType et, DateTime s, DateTime e, EventPlace ep, int p)
        {
            _eventtype = et; _starttime = s; _endtime = e; _eventplace = ep; _members = new ObservableCollection<User>();
            if (p >= 0 || p <= 3) Priority = p;
        }
        public CalendarEvent(string d, EventType et, DateTime s, DateTime e, EventPlace ep, int p)
        {
            _description = d; _eventtype = et; _starttime = s; _endtime = e; _eventplace = ep; _members = new ObservableCollection<User>();
            if (p >= 0 || p <= 3) Priority = p;
        }
        public CalendarEvent(string d, EventType et, DateTime s, DateTime e, EventPlace ep, int p, ObservableCollection<User> n)
        {
            _description = d; _eventtype = et; _starttime = s; _endtime = e; _eventplace = ep;
            if (p >= 0 || p <= 3) Priority = p;
            if (n != null && n.Count > 0)
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
                        _members = (ObservableCollection<User>)newp;
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

        public void AddMembers(ObservableCollection<User> m)
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
            return String.Format("{0} \nfrom {1} \nto {2} \nin {3}. \n{4}", _eventtype, _starttime.ToString("HH:mm dd/MM/yyyy"), _endtime.ToString("HH:mm dd/MM/yyyy"), _eventplace, _description);
        }
        #endregion
    }



    public class CalendarL: IEnumerable<CalendarEvent>/*, INotifyCollectionChanged*/
    {
        #region Fields
        public ObservableCollection<CalendarEvent> Events { get { return _events; } }
        private ObservableCollection<CalendarEvent> _events;

        #endregion

        #region Constructors

        public CalendarL()
        {
            _events = new ObservableCollection<CalendarEvent>();
        }

        public CalendarL(ObservableCollection<CalendarEvent> e)
        {
            _events = e ?? new ObservableCollection<CalendarEvent>();
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
        public CalendarEvent Remove(CalendarEvent e)
        {
            _events.Remove(e);
            return e;
        }

        public CalendarEvent RemoveAt(int index)
        {
            CalendarEvent temp = _events[index];
            _events.RemoveAt(index);
            return temp;
        }

        public IEnumerator<CalendarEvent> GetEnumerator()
        {
            return _events.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator) GetEnumerator();
        }

        #endregion
    }
}
