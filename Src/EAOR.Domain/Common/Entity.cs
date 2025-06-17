namespace EAOR.Domain.Common
{
    public abstract class Entity
    {
        public Guid Id { get; private set; }
        public DateTime CreatedDate { get; private set; }

        protected Entity()
        {

        }

        protected Entity(Guid id)
        {
            Id = id;
            CreatedDate = DateTime.UtcNow;
        }
    }
}
