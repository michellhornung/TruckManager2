using System;

namespace TruckManager.Domain.ViewModels
{
    public class ResulViewModel<T>
    {
        public ResulViewModel(Exception ex)
        {
            this.status = false;
            this.errorMsg = ex.Message;
            this.data = default(T);
        }
        public ResulViewModel()
        {
            this.status = true;
            this.data = default(T);
        }
        public ResulViewModel(T data) {
            this.status = true;
            this.data = data;
        }

        public bool status { get; set; }
        public string errorMsg { get; set; }
        public T data { get; set; }
    }
}
