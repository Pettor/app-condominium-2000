using System;
using System.Collections.Generic;
using System.Net.Mail;
using Condominium2000.Models;

namespace Condominium2000.Helpers.Models
{
	public interface IMailTemplate
	{
		List<string> mainReceivers { get; set; }
		List<string> ccReceivers { get; set; }
		string from { get; set; }
		string subject { get; set; }
		string content { get; set; }
	}

	public class ContactMailTemplate : IMailTemplate
	{
		public ContactMailTemplate(IContactForm contactForm, List<string> mainRecipients, List<string> ccAddresses)
		{
			from = contactForm.Mail;

			if (ccAddresses == null)
			{
				mainReceivers = mainRecipients;
			}
			else if (mainRecipients.Count > 0)
			{
				mainReceivers = mainRecipients;
				ccReceivers = ccAddresses;
			}
			else
			{
				mainReceivers = ccAddresses;
			}

			// Kind of ugly hack - always send the English version of the string
			var lang = LanguageHelper.GetLanguage();
			LanguageHelper.SetLanguage(LanguageHelper.Language.En);
			var category = contactForm.ConvertFormCategoryToLanguageString(contactForm.SelectedCategory).ToUpper();
			LanguageHelper.SetLanguage(lang);

			// Subject
			subject = category + ": " + contactForm.Title;

			// Content
			var contactInformation =
				"\n" +
				"NAME: " + contactForm.Name + "\n" +
				"CATEGORY: " + category + "\n" +
				"APPARTMENTNR: " + contactForm.AppartmentNr + "\n" +
				"MAIL: " + contactForm.Mail;

			content = contactForm.Content + "\n" + contactInformation;
		}

		public string subject { get; set; }

		public string content { get; set; }

		public List<string> mainReceivers { get; set; }

		public List<string> ccReceivers { get; set; }

		public string from { get; set; }
	}

	public class MailHelper
	{
		public static bool SendMail(IMailTemplate mailTemplate)
		{
			var result = false;

			var mail = new MailMessage();
			var SmtpServer = new SmtpClient("smtp.Condominium2000.se", 587);

			mail.From = new MailAddress("no_reply@Condominium2000.se");

			foreach (var recipient in mailTemplate.mainReceivers)
			{
				mail.To.Add(recipient);
			}

			if (mailTemplate.ccReceivers != null)
			{
				foreach (var recipient in mailTemplate.ccReceivers)
				{
					mail.CC.Add(recipient);
				}
			}

			mail.Subject = mailTemplate.subject;
			mail.Body = mailTemplate.content;
			mail.ReplyToList.Add(mailTemplate.from);

			try
			{
				SmtpServer.Send(mail);
				result = true;
			}
			catch (ArgumentNullException)
			{
				result = false;
			}
			catch (InvalidOperationException)
			{
				result = false;
			}
			catch (SmtpFailedRecipientsException)
			{
				result = false;
			}
			catch (SmtpException)
			{
				result = false;
			}

			return result;
		}
	}
}