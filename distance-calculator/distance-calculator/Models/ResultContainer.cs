namespace distance_calculator.Models
{
    public class ResultContainer<T>
    {
        public bool Status { get; set; }

        public T Result { get; set; }

        public string Message { get; set; }
    }
}
