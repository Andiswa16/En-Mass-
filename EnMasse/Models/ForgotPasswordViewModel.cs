namespace EnMasse.Models
{
    public class ForgotPasswordViewModel
    {
        public string DUsername { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }

        public bool UsernameExists { get; set; }
    }
}