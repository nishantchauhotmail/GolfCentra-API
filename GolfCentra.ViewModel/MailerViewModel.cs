using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel
{
    /// <summary>
    /// Properties For Mailer's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class MailerViewModel
    {
        public string ToEmails { get; set; }

        public string Subject { get; set; }

        public string Attachment { get; set; }

        public string Body { get; set; }

        public string From { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Message { get; set; }

        public string Contact { get; set; }

        public string CCMail { get; set; }

        public string Path { get; set; }

    }
}
