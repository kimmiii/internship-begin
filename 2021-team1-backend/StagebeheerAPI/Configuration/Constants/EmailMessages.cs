namespace StagebeheerAPI.Configuration.Constants
{
    public class EmailMessages
    {
        // Company
        public static string companyActivedSubject = "Bedrijfsprofiel geactiveerd";
        public static string companyActivedBody =
            "De stagecoördinator van de PXL Hogeschool heeft uw bedrijfsprofiel binnen de stagebeheerapplicatie geactiveerd. " +
            "Vanaf nu kan u stageaanvragen indienen en stageopdrachten beheren. <br><br>" +
            "Bedankt voor uw registratie. Veel succes!";


        public static string companyRejectedSubject = "Bedrijfsprofiel afgekeurd";
        public static string companyRejectedBody(string evaluationFeedback)
        {
            return "De stagecoördinator van de PXL Hogeschool heeft uw bedrijfsprofiel binnen de stagebeheerapplicatie afgekeurd. <br>" +
                   "Bericht van de stagecoördinator: <br><br>" +
                   $"<span style=\"font-style: italic;\">{evaluationFeedback.Replace("\n", "<br>")}</span> <br><br>" +
                   "Voor vragen omtrent de afkeuring van uw bedrijf: <br>" +
                   "<ul><li><strong>Stagecoördinator Toegepaste Informatica:</strong> marijke.willems@pxl.be</li>" +
                   "<li><strong>Stagecoördinator Elektronica-ICT:</strong> bart.stukken@pxl.be</li></ul>";
        }


        // Internship
        public static string hireRequestedSubject = "Een bedrijf wil jou als stagiair";
        public static string hireRequestedBody(string companyName, string researchTopicTitle)
        {
            return $"Het bedrijf <span style=\"font-weight: bold;\">{companyName}</span> wil jou als stagiair voor de stageopdracht met " +
                   $"titel <span style=\"font-weight: bold;\">{researchTopicTitle}</span>. Ga naar de stagebeheerapplicatie om aan te geven " +
                   "of je deze opdracht als definitieve stageopdracht wilt uitvoeren. Pas na bevestiging van de stagecoördinator word je definitief toegewezen " +
                   "aan bovenvernoemde stageopdracht. <br><br>";
        }


        public static string hireApprovedToCompanySubject = "Stagiair toegewezen aan stageopdracht";
        public static string hireApprovedToCompanyBody(string studentFirstname, string studentSurname, string researchTopicTitle)
        {
            return $"Student <span style=\"font-weight: bold;\">{studentFirstname} {studentSurname}</span> is aan " +
                   $"uw stageopdracht met titel <span style=\"font-weight: bold;\">{researchTopicTitle}</span> toegewezen. Deze student is vanaf nu officieel uw stagiair voor" +
                   " bovenvernoemde stageopdracht. Proficiat! <br><br>" +
                   "Ga naar de stagebeheerapplicatie om de stagecontracten te ondertekenen. <br><br>";
        }


        public static string hireApprovedToStudentSubject = "Je bent toegewezen aan een stageopdracht";
        public static string hireApprovedToStudentBody(string researchTopicTitle, string companyName)
        {
            return $"Goed nieuws! Je bent toegewezen aan de stageopdracht met titel <span style=\"font-weight: bold;\">{researchTopicTitle}</span> " +
                   $"van bedrijf <span style=\"font-weight: bold;\">{companyName}</span>. Proficiat! <br><br>" +
                   "Ga naar de stagebeheerapplicatie om de stagecontracten te ondertekenen. <br><br>";
        }


        // Internship Service
        public static string internshipApproved(string researchTopicTitle)
        {
            return $"Uw stageaanvraag met titel <span style=\"font-weight: bold;\">{researchTopicTitle}</span> is goedgekeurd door de stagecoördinator van de PXL Hogeschool. " +
                    "Vanaf nu kunnen studenten solliciteren voor deze stageopdracht en kunnen de nodige stagecontracten gegenereerd worden.";
        }

        public static string hireRejectedSubject = "Je bent niet toegelaten voor een stageopdracht";
        public static string hireRejectedBody(string researchTopicTitle, string companyName, string rejectionFeedback)
        {
            return $"Je bent niet toegelaten om de stageopdracht met titel <span style=\"font-weight: bold;\">{researchTopicTitle}</span> " +
                   $"van bedrijf <span style=\"font-weight: bold;\">{companyName}</span> uit te voeren. <br><br> " +
                   "Bericht van de stagecoördinator: <br>" +
                   $"<span style=\"font-style: italic;\">{rejectionFeedback.Replace("\n", "<br>")}</span>";
        }

        public static string internshipRejected(string researchTopicTitle)
        {
            return $"Uw stageaanvraag met titel <span style=\"font-weight: bold;\">{researchTopicTitle}</span> is geweigerd door de stagecoördinator van de PXL Hogeschool. <br><br> " +
                   "Bericht van de stagecoördinator: <br>";
        }

        public static string internshipMoreInfo(string researchTopicTitle)
        {
            return "De stagecoördinator van de PXL Hogeschool heeft een opmerking over uw stageaanvraag met titel " +
                   $"<span style=\"font-weight: bold;\">{researchTopicTitle}</span>. <br><br> " +
                   "Bericht van de stagecoördinator: <br>";
        }

        public static string internshipReviewAskedBody(string researchTopicTitle, string internalFeedback)
        {
            if (internalFeedback != null)
            {
                return $"De stagecoördinator wenst dat u de stageaanvraag met titel <span style=\"font-weight: bold;\">{researchTopicTitle}</span> beoordeelt. " +
                    "Ga naar de stagebeheerapplicatie om de evaluatie uit te voeren. <br><br>" +
                    "Bericht van de stagecoördinator: <br>" +
                    $"<span style=\"font-style: italic;\">{internalFeedback.Replace("\n", "<br>")}</span>";
            }

            return $"De stagecoördinator wenst dat u de stageaanvraag met titel <span style=\"font-weight: bold;\">{researchTopicTitle}</span> beoordeelt. " +
                    "Ga naar de stagebeheerapplicatie om de evaluatie uit te voeren. <br><br>";
        }


        // Reminder
        public static string reminderSubject = "Herinnering";
        public static string reminderBody(string researchTopicTitle)
        {
            return "De stagecoördinator van de PXL Hogeschool heeft u een herinnering gestuurd om de stageaanvraag met titel " +
                   $"<span style=\"font-weight: bold;\">{researchTopicTitle}</span> te evalueren. Ga naar de stagebeheerapplicatie " +
                   "om deze evaluatie uit te voeren. <br><br>";
        }
    }
}
