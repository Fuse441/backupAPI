namespace colab_api.Responses.@base
{
    public class ResponseDataPagination<T> : ResponseData<T>
    {
        public int current { get; set; }

        public int total { get; set; }

        public List<T> responseDatas { get; set; }
      

        public ResponseDataPagination(Enum responseCode, List<T> results, int current, int total)
        {
            responseDatas = results;
            this.current = current;
            this.total = total;
        }
    }

}
