namespace PhoneLibrary.service
{
    public class EntityChangedEventArgs<TEntity>
    {
        public TEntity Entity { get; set; }

        public EntityChangedEventArgs(TEntity entity)
        {
            this.Entity = entity;
        }
    }
}