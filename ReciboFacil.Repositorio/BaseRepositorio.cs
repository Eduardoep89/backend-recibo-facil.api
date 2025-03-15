public abstract class BaseRepositorio
{
    protected readonly ReciboFacilContexto _contexto;

    protected BaseRepositorio(ReciboFacilContexto contexto)
    {
        _contexto = contexto;
    }
}