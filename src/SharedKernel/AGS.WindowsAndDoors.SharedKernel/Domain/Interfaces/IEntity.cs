namespace AGS.WindowsAndDoors.SharedKernel.Domain.Interfaces;

public interface IEntity<TId>
{
    TId Id { get; }
}
