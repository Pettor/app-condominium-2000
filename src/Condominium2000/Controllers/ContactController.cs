using System.Linq;
using System.Web.Mvc;
using Condominium2000.Controllers.Interface;
using Condominium2000.Helpers;
using Condominium2000.Helpers.Models;
using Condominium2000.Helpers.Session;
using Condominium2000.Helpers.Tags;
using Condominium2000.Helpers.Translator;
using Condominium2000.Models;
using Condominium2000.Models.Context;
using Condominium2000.Models.ViewModels;
using Condominium2000.Resources.Contact;
using BoardForm = Condominium2000.Models.BoardForm;
using EntityState = System.Data.Entity.EntityState;
using ErrorForm = Condominium2000.Models.ErrorForm;

namespace Condominium2000.Controllers
{
	public class ContactController : BaseController
	{
		private Condominium2000Context _db = new Condominium2000Context();

		//
		// GET: /Contact/

		public ActionResult Index()
		{
			/**
			 * Set Menu in Session Object
			 */
			SessionHelper.SessionMenu = SessionHelper.Menu.Contact;

			/**
			 * Fill Model
			 */
			var contactTemplate = new ContactViewModel(ref _db, ContactViewModel.FormType.Error);

			return View(contactTemplate);
		}

		//
		// GET: /Contact/MailSuccessful

		public ActionResult MailSuccessful()
		{
			/**
			 * Set Menu in Session Object
			 */
			SessionHelper.SessionMenu = SessionHelper.Menu.Contact;

			/**
			 * Fill Model
			 */
			var contactTemplate = new ContactViewModel(ref _db, ContactViewModel.FormType.Error);

			return View(contactTemplate);
		}

		//
		// GET: /Contact/ErrorForm

		public ActionResult ErrorForm()
		{
			/**
			 * Set Menu in Session Object
			 */
			SessionHelper.SessionMenu = SessionHelper.Menu.Contact;

			/**
			 * Fill Model
			 */
			var contactTemplate = new ContactViewModel(ref _db, ContactViewModel.FormType.Error);

			return View(contactTemplate);
		}

		//
		// POST: /Contact/ErrorForm

		[HttpPost]
		public ActionResult ErrorForm(ErrorForm errorForm)
		{
			if (ContactHelper.ValidateErrorForm(errorForm) == ErrorCodes.EditErrorCodes.Success)
			{
				var mainRecipients = errorForm.GetMainRecipientsFromCategory((ErrorFormCategory)errorForm.SelectedCategory);
				//var ccRecipients = new List<string>();
				//ccRecipients.Add(Constants.MAIL_ERROR_FORM_TO);
				var mail = new ContactMailTemplate(errorForm, mainRecipients, null);

				if (MailHelper.SendMail(mail))
				{
					return RedirectToAction("MailSuccessful");
				}
				ModelState.AddModelError("", SharedErrors.ERROR_INTERNAL_SEND_MAIL_UNSUCCESSFUL);
			}
			else
			{
				ModelState.AddModelError("", SharedErrors.ERROR_INTERNAL_NOT_VALID_FORM);
			}

			/**
			 * Fill Model
			 */
			var contactTemplate = new ContactViewModel(ref _db, ContactViewModel.FormType.Error);
			contactTemplate.ErrorForm = errorForm;
			contactTemplate.ErrorForm.CategoryList = errorForm.GetSelectList();
			return View(contactTemplate);
		}

		//
		// GET: /Contact/BoardForm

		public ActionResult BoardForm()
		{
			/**
			 * Set Menu in Session Object
			 */
			SessionHelper.SessionMenu = SessionHelper.Menu.Contact;

			/**
			 * Fill Model
			 */
			var contactTemplate = new ContactViewModel(ref _db, ContactViewModel.FormType.Board);

			return View(contactTemplate);
		}

		//
		// POST: /Contact/BoardForm

		[HttpPost]
		public ActionResult BoardForm(BoardForm boardForm)
		{
			if (ContactHelper.ValidateBoardForm(boardForm) == ErrorCodes.EditErrorCodes.Success)
			{
				var mainRecipients = boardForm.GetMainRecipientsFromCategory((BoardFormCategory)boardForm.SelectedCategory);
				//List<string> ccRecipients = new List<string>();
				//ccRecipients.Add(Constants.MAIL_ERROR_FORM_TO);
				var mail = new ContactMailTemplate(boardForm, mainRecipients, null);

				if (MailHelper.SendMail(mail))
				{
					return RedirectToAction("MailSuccessful");
				}
				ModelState.AddModelError("", SharedErrors.ERROR_INTERNAL_SEND_MAIL_UNSUCCESSFUL);
			}
			else
			{
				ModelState.AddModelError("", SharedErrors.ERROR_INTERNAL_NOT_VALID_FORM);
			}

			/**
			 * Fill Model
			 */
			var contactTemplate = new ContactViewModel(ref _db, ContactViewModel.FormType.Board) {BoardForm = boardForm};
			contactTemplate.BoardForm.CategoryList = boardForm.GetSelectList();
			return View(contactTemplate);
		}

		//
		// GET: /Contact/ITForm

		public ActionResult ITForm()
		{
			/**
			 * Set Menu in Session Object
			 */
			SessionHelper.SessionMenu = SessionHelper.Menu.Contact;

			/**
			 * Fill Model
			 */
			var contactTemplate = new ContactViewModel(ref _db, ContactViewModel.FormType.IT);

			return View(contactTemplate);
		}

		//
		// POST: /Contact/ITForm

		[HttpPost]
		public ActionResult ITForm(ItForm itForm)
		{
			if (ContactHelper.ValidateITForm(itForm) == ErrorCodes.EditErrorCodes.Success)
			{
				var mainRecipients = itForm.GetMainRecipientsFromCategory((ItFormCategory)itForm.SelectedCategory);
				//List<string> ccRecipients = new List<string>();
				//ccRecipients.Add(Constants.MAIL_ERROR_FORM_TO);
				var mail = new ContactMailTemplate(itForm, mainRecipients, null);

				if (MailHelper.SendMail(mail))
				{
					return RedirectToAction("MailSuccessful");
				}
				ModelState.AddModelError("", SharedErrors.ERROR_INTERNAL_SEND_MAIL_UNSUCCESSFUL);
			}
			else
			{
				ModelState.AddModelError("", SharedErrors.ERROR_INTERNAL_NOT_VALID_FORM);
			}

			/**
			 * Fill Model
			 */
			var contactTemplate = new ContactViewModel(ref _db, ContactViewModel.FormType.IT) {ItForm = itForm};
			contactTemplate.ItForm.CategoryList = itForm.GetSelectList();
			return View(contactTemplate);
		}

		//
		// GET: /Contact/EditContactInfo/5

		public ActionResult EditContactInfo(int id)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			/**
			 * Fill Model
			 */
			var ci = _db.ContactInfos.FirstOrDefault(a => a.Id == id);

			return View(ci);
		}

		//
		// POST: /Contact/EditContactInfo/5

		[HttpPost]
		public ActionResult EditContactInfo(ContactInfo ci)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			if (ModelState.IsValid)
			{
				var dbCi = _db.ContactInfos.FirstOrDefault(a => a.Id == ci.Id);
				if (dbCi != null)
				{
					// Validate the user
					var status = ContactHelper.ValidateChangedContactInfo(dbCi);

					if (status == ErrorCodes.EditErrorCodes.Success)
					{
						if (TryUpdateModel(dbCi))
						{
							// Save HTML content
							dbCi.HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(ci.ContentEn);
							dbCi.HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(ci.ContentSv);

							_db.Entry(dbCi).State = EntityState.Modified;
							_db.SaveChanges();
							TempData["EditContactInfoSuccess"] = EditInfo.SUCCESS_INFO_SAVED;
						}
						else
						{
							ModelState.AddModelError("", EditInfo.ERROR_INTERNAL_NOT_SAVE_MODEL);
						}
					}
					else
					{
						ModelState.AddModelError("", ErrorCodes.ErrorCodeToString(status));
					}
				}
				else
				{
					ModelState.AddModelError("", EditInfo.ERROR_INTERNAL_NOT_FIND_INFO);
				}
			}
			return View(ci);
		}

		//
		// GET: /Contact/PreviewContactInfo/5

		public ActionResult PreviewContactInfo(int id)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			var info = _db.ContactInfos.Find(id);

			// Convert Input to valid HTML
			info.HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(info.ContentSv);
			info.HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(info.ContentEn);
			return View(info);
		}

		//
		// GET: /Contact/EditErrorFormInfo/5

		public ActionResult EditErrorFormInfo(int id)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			/**
			 * Fill Model
			 */
			var ci = _db.ErrorFormInfos.FirstOrDefault(a => a.Id == id);

			return View(ci);
		}

		//
		// POST: /Contact/EditErrorFormInfo/5

		[HttpPost]
		public ActionResult EditErrorFormInfo(ErrorFormInfo efi)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			if (ModelState.IsValid)
			{
				var dbCi = _db.ErrorFormInfos.FirstOrDefault(a => a.Id == efi.Id);
				if (dbCi != null)
				{
					// Validate the user
					var status = ContactHelper.ValidateChangedErrorFormInfo(dbCi);

					if (status == ErrorCodes.EditErrorCodes.Success)
					{
						if (TryUpdateModel(dbCi))
						{
							// Save HTML content
							dbCi.HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(efi.ContentEn);
							dbCi.HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(efi.ContentSv);

							_db.Entry(dbCi).State = EntityState.Modified;
							_db.SaveChanges();
							TempData["EditErrorFormInfoSuccess"] = EditInfo.SUCCESS_INFO_SAVED;
						}
						else
						{
							ModelState.AddModelError("", EditInfo.ERROR_INTERNAL_NOT_SAVE_MODEL);
						}
					}
					else
					{
						ModelState.AddModelError("", ErrorCodes.ErrorCodeToString(status));
					}
				}
				else
				{
					ModelState.AddModelError("", EditInfo.ERROR_INTERNAL_NOT_FIND_INFO);
				}
			}
			return View(efi);
		}

		//
		// GET: /Contact/PreviewErrorFormInfo/5

		public ActionResult PreviewErrorFormInfo(int id)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			var info = _db.ErrorFormInfos.Find(id);

			// Convert Input to valid HTML
			info.HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(info.ContentSv);
			info.HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(info.ContentEn);
			return View(info);
		}

		//
		// GET: /Contact/EditBoardFormInfo/5

		public ActionResult EditBoardFormInfo(int id)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			/**
			 * Fill Model
			 */
			var ci = _db.BoardFormInfos.FirstOrDefault(a => a.Id == id);

			return View(ci);
		}

		//
		// POST: /Contact/EditBoardFormInfo/5

		[HttpPost]
		public ActionResult EditBoardFormInfo(BoardFormInfo bfi)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			if (ModelState.IsValid)
			{
				var dbBfi = _db.BoardFormInfos.FirstOrDefault(a => a.Id == bfi.Id);
				if (dbBfi != null)
				{
					// Validate the user
					var status = ContactHelper.ValidateChangedBoardFormInfo(dbBfi);

					if (status == ErrorCodes.EditErrorCodes.Success)
					{
						if (TryUpdateModel(dbBfi))
						{
							// Save HTML content
							dbBfi.HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(bfi.ContentEn);
							dbBfi.HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(bfi.ContentSv);

							_db.Entry(dbBfi).State = EntityState.Modified;
							_db.SaveChanges();
							TempData["EditBoardFormInfoSuccess"] = EditInfo.SUCCESS_INFO_SAVED;
						}
						else
						{
							ModelState.AddModelError("", EditInfo.ERROR_INTERNAL_NOT_SAVE_MODEL);
						}
					}
					else
					{
						ModelState.AddModelError("", ErrorCodes.ErrorCodeToString(status));
					}
				}
				else
				{
					ModelState.AddModelError("", EditInfo.ERROR_INTERNAL_NOT_FIND_INFO);
				}
			}
			return View(bfi);
		}

		//
		// GET: /Contact/PreviewBoardFormInfo/5

		public ActionResult PreviewBoardFormInfo(int id)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			var info = _db.BoardFormInfos.Find(id);

			// Convert Input to valid HTML
			info.HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(info.ContentSv);
			info.HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(info.ContentEn);
			return View(info);
		}

		//
		// GET: /Contact/EditITFormInfo/5

		public ActionResult EditITFormInfo(int id)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			/**
			 * Fill Model
			 */
			var ci = _db.ItFormInfos.FirstOrDefault(a => a.Id == id);

			return View(ci);
		}

		//
		// POST: /Contact/EditITFormInfo/5

		[HttpPost]
		public ActionResult EditITFormInfo(ItFormInfo ifi)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			if (ModelState.IsValid)
			{
				var dbIfi = _db.ItFormInfos.FirstOrDefault(a => a.Id == ifi.Id);
				if (dbIfi != null)
				{
					// Validate the user
					var status = ContactHelper.ValidateChangedITFormInfo(dbIfi);

					if (status == ErrorCodes.EditErrorCodes.Success)
					{
						if (TryUpdateModel(dbIfi))
						{
							// Save HTML content
							dbIfi.HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(ifi.ContentEn);
							dbIfi.HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(ifi.ContentSv);

							_db.Entry(dbIfi).State = EntityState.Modified;
							_db.SaveChanges();
							TempData["EditITFormInfoSuccess"] = EditInfo.SUCCESS_INFO_SAVED;
						}
						else
						{
							ModelState.AddModelError("", EditInfo.ERROR_INTERNAL_NOT_SAVE_MODEL);
						}
					}
					else
					{
						ModelState.AddModelError("", ErrorCodes.ErrorCodeToString(status));
					}
				}
				else
				{
					ModelState.AddModelError("", EditInfo.ERROR_INTERNAL_NOT_FIND_INFO);
				}
			}
			return View(ifi);
		}

		//
		// GET: /Contact/PreviewITFormInfo/5

		public ActionResult PreviewITFormInfo(int id)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			var info = _db.ItFormInfos.Find(id);

			// Convert Input to valid HTML
			info.HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(info.ContentSv);
			info.HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(info.ContentEn);
			return View(info);
		}

		//
		// POST: /Contact/TranslateText
		// AJAX Operation

		[HttpPost]
		public ActionResult TranslateText(GoogleTranslator.TranslateObject obj)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			if (obj.Lang == null)
			{
				return Json(obj);
			}

			return Json(TranslationHelper.TranslateText(obj));
		}

		protected override void Dispose(bool disposing)
		{
			_db.Dispose();
			base.Dispose(disposing);
		}
	}
}