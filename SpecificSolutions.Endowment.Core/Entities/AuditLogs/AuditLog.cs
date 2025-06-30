using SpecificSolutions.Endowment.Core.Enums;
using SpecificSolutions.Endowment.Core.Helpers;

namespace SpecificSolutions.Endowment.Core.Entities.AuditLogs
{
    public sealed class AuditLog
    {
        public AuditLog() { }

        public Guid Id { get; private set; } = Guid.NewGuid();
        public string UserId { get; private set; }
        public string Context { get; private set; }
        public EventType EventType { get; private set; }
        public string? Data { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
        public DateOnly CreatedOn { get; private set; } = DateOnly.FromDateTime(DateTime.Now);

        private AuditLog(string userId, EventType eventType, string context, string? data)
        {
            UserId = userId;
            Context = context;
            EventType = eventType;
            Data = data;
        }

        public static AuditLog Create(string userId, EntityContext context, string data)
            => new(userId, EventType.Created, context.ToString(), data);
    }

    public class AuditLog<T>
    {
        public static AuditLog ForCreate<T>(string userId, EntityContext context, T data)
        {
            //ApplyChange(data);

            return AuditLog.Create(userId, context, Helper.Serialize(data));
        }

        //public static AuditLog ForLogin(int userId)
        // => new(userId, EventType.LoggedIn, EntityContext.Login.ToString(), null);

        //public static AuditLog ForLoginOut(int userId)
        // => new(userId, EventType.LoggedOut, EntityContext.Login.ToString(), null);

        //public static AuditLog ForUpdate<T>(int userId, EntityContext context, T data)
        // => new(userId, EventType.Updated, context.ToString(), data.Serialize());

        //public static AuditLog ForDelete<T>(int userId, EntityContext context, T data)
        // => new(userId, EventType.Deleted, context.ToString(), data.Serialize());

        // get current instance of the audit log


        public void ApplyChange(dynamic dynamic)
        {
            //if (@event.Sequence == 1)
            //{
            //    Id = @event.AggregateId;
            //}

            //Sequence++;

            //if (string.IsNullOrWhiteSpace(Id))
            //    throw new InvalidOperationException("Id == Guid.Empty");

            //if (@event.Sequence != Sequence)
            //    throw new InvalidOperationException("@event.Sequence != Sequence");

            ((dynamic)this).Apply(dynamic);

            //if (isNew)
            //    _uncommittedEvents.Add(@event);
        }

        // implement the audit log properties here

        //public AuditLog(T command)
        //{
        //    // implement the audit log properties here
        //}

        //public void Update(T command)
        //{
        //    // implement the audit log properties here
        //}

        //public void Delete(T command)
        //{
        //    // implement the audit log properties here
        //}

        //public T Get(T command)
        //{
        //    // implement the audit log properties here
        //    return null;

        //}
    }
}
