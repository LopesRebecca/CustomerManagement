namespace CustomerManagement.Application.Mediator
{
    /// <summary>
    /// Interface marcadora para requests que retornam um resultado
    /// </summary>
    public interface IRequest<TResponse>
    {
    }

    /// <summary>
    /// Interface marcadora para requests sem retorno (void)
    /// </summary>
    public interface IRequest : IRequest<Unit>
    {
    }

    /// <summary>
    /// Representa um tipo void para operações sem retorno
    /// </summary>
    public readonly struct Unit : IEquatable<Unit>
    {
        public static readonly Unit Value = new();

        public bool Equals(Unit other) => true;
        public override bool Equals(object? obj) => obj is Unit;
        public override int GetHashCode() => 0;
        public override string ToString() => "()";
    }
}
