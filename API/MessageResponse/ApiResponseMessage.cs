namespace API.MessageResponse
{
    public class ApiResponseMessage
    {
        public const string MSG01 = "MSG01";
        public const string MSG02 = "MSG02";
        public const string MSG03 = "MSG03";
        public const string MSG04 = "MSG04";
        public const string MSG05 = "MSG05";
        public const string MSG06 = "MSG06";
        public const string MSG07 = "MSG07";
        public const string MSG08 = "MSG08";
        public const string MSG09 = "MSG09";
        public const string MSG10 = "MSG10";
        public const string MSG11 = "MSG11";
        public const string MSG12 = "MSG12";
        public const string MSG13 = "MSG13";
        public const string MSG14 = "MSG14";
        public const string MSG15 = "MSG15";
        public const string MSG16 = "MSG16";
        public const string MSG17 = "MSG17";
        public const string MSG18 = "MSG18";
        public const string MSG19 = "MSG19";
        public const string MSG20 = "MSG20";
        public const string MSG21 = "MSG21";
        public const string MSG22 = "MSG22";
        public const string MSG23 = "MSG23";
        public const string MSG24 = "MSG24";
        public const string MSG25 = "MSG25";
        public const string MSG26 = "MSG26";


        public string StatusCode { get; set; }
        public string Message { get; set; }
        public ApiResponseMessage(string statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetMessageForMessageCode(statusCode);
        }

        private string GetMessageForMessageCode(string messageCode) => messageCode switch
        {
            MSG01 => "No search results for your key search",
            MSG02 => "The * field is required",
            MSG03 => "Update information successfully. ",
            MSG04 => "Add new account successfully. ",
            MSG05 => "Add new Auction successfully. ",
            MSG06 => "Reset password successfully. ",
            MSG07 => "Update information transaction successfully. ",
            MSG08 => "The amount you are bidding on is invalid. Please re-enter the amount you want to bid. ",
            MSG09 => "Incorrect user name or password. Please check again. ",
            MSG10 => "The amount you bid is less than the floor amount. Please enter larger than the base amount. ",
            MSG11 => "The amount you bid is less than the current largest bid amount. Please enter an amount greater than the above amount. ",
            MSG12 => "The amount you are bidding on is invalid. Please re-enter the amount you want to bid. ",
            MSG13 => "You have successfully set the automatic auction amount. ",
            MSG14 => "The system has recorded that you have registered to participate in the auction.",
            MSG15 => "The system has recorded your successful registration to participate in the auction.",
            MSG16 => "Your real estate has been recorded in the system, please wait for admin to confirm.",
            MSG17 => "Change status account successfully. ",
            MSG18 => "Create new rule successfully",
            MSG19 => "Your real estate has uploaded on website.",
            MSG20 => "The amount you entered to pay does not match the suggested amount.",
            MSG21 => "Create new news successfully",
            MSG22 => "Username already exists",
            MSG23 => "Email already exists",
            MSG24 => "Create task successfully.",
            MSG25 => "Update task successfully.",
            MSG26 => "Refund money successfully.",
            _ => null
        };
    }
}
