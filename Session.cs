using System;
using System.Collections.Generic;
using System.Linq;

namespace SignalRChat
{
   

    public class Sessions : ISessions
    {
        Guid toBeRemoved = Guid.NewGuid(); //TODO : Remove this when you are removing defatl settion
        public Sessions()
        {
            _sessionUsers = new List<ISession>();
            //TODO : Remove this when the part implemented
            //Right now we  create  one default session to be able to test the rest
            _sessionUsers.Add(new Session(toBeRemoved));
        }

        private List<ISession> _sessionUsers { get; set; }
        public IReadOnlyList<ISession> Users => _sessionUsers.AsReadOnly();


        public ISession Get(Guid sessionId) //TODO :  Fix this also then you remove the default settion
        {
            return _sessionUsers.First(x => x.SessionID == toBeRemoved); //
        }

        public void Add(ISession session)
        {
            _sessionUsers.Add(session);
        }

        public void Remove(ISession session)
        {
            _sessionUsers.Remove(session);
        }

        public void Remove(Guid sessionId)
        {
            var session = _sessionUsers.First(x => x.SessionID == sessionId);
            _sessionUsers.Remove(session);
        }


    }


    public interface ISessions
    {
        IReadOnlyList<ISession> Users { get; }

        void Add(ISession session);
        void Remove(Guid sessionId);
        void Remove(ISession session);
        ISession Get(Guid sessionId);
    }
    public interface ISession
    {
        DateTimeOffset dateTime { get; }
        Guid SessionID { get; }
        IReadOnlyList<ISessionUser> Users { get; }

        void Add(ISessionUser user);
        void Remove(Guid TrackingId);
        void Remove(ISessionUser user);
    }

    public class Session : ISession
    {
        public Guid SessionID { get; private set; }

        public DateTimeOffset dateTime { get; private set; }
        private List<ISessionUser> _sessionUsers { get; set; }
        public IReadOnlyList<ISessionUser> Users => _sessionUsers.AsReadOnly();

        public Session(Guid sessionID)
        {
            SessionID = sessionID;
            _sessionUsers = new List<ISessionUser>();
        }

        public void Add(ISessionUser user)
        {
            _sessionUsers.Add(user);
        }

        public void Remove(ISessionUser user)
        {
            _sessionUsers.Remove(user);
        }

        public void Remove(Guid TrackingId)
        {
            var session = _sessionUsers.First(x => x.TrackingId == TrackingId);
            _sessionUsers.Remove(session);
        }
    }

    public interface ISessionUser
    {
        string Avartar { get; set; }
        string ConnectionId { get; set; }
        string NicName { get; set; }
        Guid TrackingId { get; set; }
    }

    public class SessionUser : ISessionUser
    {
        public SessionUser(Guid trackingId, string nicName, string connectionId, string avartar)
        {
            TrackingId = trackingId;
            NicName = nicName;
            ConnectionId = connectionId;
            Avartar = avartar;
        }

        public Guid TrackingId { get; set; }
        public string NicName { get; set; }
        public string ConnectionId { get; set; }
        public string Avartar { get; set; }


        public void UpdateConnectionId(string conectionId) 
        {
            ConnectionId = conectionId;
        }
    }
}
