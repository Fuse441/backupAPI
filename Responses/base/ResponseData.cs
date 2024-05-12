namespace colab_api.Responses.@base
{
    public class ResponseData<T>
    {
 
      
        public T responseDatas { get; set; }
        public httpCodeResponse responseCode { get; set; }

        public ResponseData(httpCodeResponse code, T responseDatas)
        {
            this.responseCode = code;
            this.responseDatas = responseDatas;

        }
        public ResponseData(httpCodeResponse code)
        {
            this.responseCode = code;

       
        }
        public ResponseData()
        {
            

        }

      


    }

}
