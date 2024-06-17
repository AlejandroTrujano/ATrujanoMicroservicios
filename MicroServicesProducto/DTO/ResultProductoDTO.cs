namespace MicroServicesProducto.DTO
{
    public class ResultProductoDTO
    {
        public bool Correct { get; set; } = true;
        public string ErrorMessage { get; set; }
        public Exception Exception { get; set; }
        public object  Object { get; set; }
        public List<object> Objects { get; set; }
    }
}
