namespace common.Entities
{
    public class GenericResponse
    {
        private int code;
        private string message;

        public GenericResponse()
        {

        }

        public int Code
        {
            get
            {
                return code;
            }
            set
            {
                code = value;
            }
        }

        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
            }
        }

        public override string ToString()
        {
            return base.ToString() + ": " + "Code=" + Code + ", Message=" + Message;
        }
    }
}
